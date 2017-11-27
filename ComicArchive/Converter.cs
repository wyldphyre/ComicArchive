using System;
using System.IO;
using SharpCompress;
using SharpCompress.Archives;
using SharpCompress.Archives.Zip;
using SharpCompress.Common;
using SharpCompress.Readers;

namespace ComicArchive
{
  public class ConverterOptions
  {
    public ConverterOptions()
    {
      this.LogOptions = new ConverterLogOptions();
    }
    public bool ReplaceOriginalFile { get; set; }
    public string WorkingPath { get; set; }
    public bool OverwriteExisting { get; set; }
    public readonly ConverterLogOptions LogOptions;
  }

  public class ConverterLogOptions
  {
    public bool ShowFullPaths { get; set; }
    public bool Verbose { get; set; }
  }

  public class Converter
  {
    void LogActivity(string message)
    {
      LogActivityEvent?.Invoke(message);
    }

    public (bool, string) ConvertToZipArchive(string sourcePath, ConverterOptions options, out string convertedPath)
    {
      convertedPath = string.Empty;

      if (!File.Exists(sourcePath))
        return (false, $"Cannot convert '{sourcePath}. File does not exist.");
      
      var sourceDirectory = Path.GetDirectoryName(sourcePath);
      var sourceFilename = Path.GetFileNameWithoutExtension(sourcePath);
      convertedPath = Path.Combine(sourceDirectory, sourceFilename + ".cbz");

      var sourceDisplayPath = sourcePath;
      var convertedDisplayPath = convertedPath;

      if (!options.LogOptions.ShowFullPaths)
      {
        sourceDisplayPath = Path.GetFileName(sourceDisplayPath);
        convertedDisplayPath = Path.GetFileName(convertedDisplayPath);
      }

      var FileIsZip = ZipArchive.IsZipFile(sourcePath);

      if (FileIsZip && string.Equals(Path.GetExtension(sourcePath), ".cbz", StringComparison.CurrentCultureIgnoreCase))
      {
        LogActivity($"Skipping '{sourceDisplayPath}'. Already a zip archive");
        return (true, string.Empty);
      }

      // This is mostly a sanity check. Shouldn't come up.
      if (string.Equals(sourcePath, convertedPath, StringComparison.CurrentCultureIgnoreCase))
        return (false, $"Skipping because target and destination are the same: '{sourceDisplayPath}'");

      if (File.Exists(convertedPath) && !options.OverwriteExisting)
        return (false, $"Cannot convert path '{sourceDisplayPath} to Zip. Target path '{convertedDisplayPath}' already exists");

      if (FileIsZip)
      {
        if (File.Exists(convertedPath) && options.OverwriteExisting)
        {
          File.Delete(convertedPath);
          File.Copy(sourcePath, convertedPath);
          LogActivity($"Replaced '{sourceDisplayPath}' with '{convertedDisplayPath}'");
        }
        else
        {
          File.Copy(sourcePath, convertedPath);
          LogActivity($"Copied '{sourceDisplayPath}' to '{convertedDisplayPath}'");
        }
      }
      else
      {
        var workingPath = Path.Combine(options.WorkingPath ?? AppContext.BaseDirectory, sourceFilename);

        LogActivity($"Processing {sourcePath}");

        using (Stream stream = File.OpenRead(sourcePath))
        using (var reader = ReaderFactory.Open(stream))
        {
          while (reader.MoveToNextEntry())
          {
            if (!reader.Entry.IsDirectory)
            {
              //Console.WriteLine(reader.Entry.Key);

              reader.WriteEntryToDirectory(workingPath, new ExtractionOptions()
              {
                ExtractFullPath = true,
                Overwrite = true
              });
            }
          }
        }

        using (var archive = ZipArchive.Create())
        {
          archive.AddAllFromDirectory(workingPath);
          archive.DeflateCompressionLevel = SharpCompress.Compressors.Deflate.CompressionLevel.None;
          archive.SaveTo(convertedPath, CompressionType.Deflate);
          LogActivity($"Created '{convertedDisplayPath}'");
        }

        Directory.Delete(workingPath, true);
      }

      // TODO: Check replacement functionality works
      if (options.ReplaceOriginalFile && File.Exists(sourcePath))
      {
        File.Delete(sourcePath);
        LogActivity($"Deleted '{sourceDisplayPath}'");
      }

      return (true, string.Empty);
    }
    public event Action<string> LogActivityEvent;
  }
}
