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
    public bool ReplaceOriginalFile { get; set; }
    public string WorkingPath { get; set; }
    public bool OverwriteExisting { get; set; }
  }

  public class Converter
  {
    public void ConvertToZipArchive(string sourcePath, ConverterOptions options, out string convertedPath)
    {
      if (!File.Exists(sourcePath))
        throw new Exception($"Cannot convert '{sourcePath}. File does not exist.");

      var sourceDirectory = Path.GetDirectoryName(sourcePath);
      var sourceFilename = Path.GetFileNameWithoutExtension(sourcePath);
      convertedPath = Path.Combine(sourceDirectory, sourceFilename + ".cbz");

      if (File.Exists(convertedPath) && !options.OverwriteExisting)
        throw new Exception($"Cannot convert path '{sourcePath} to Zip. Target path '{convertedPath}' already exists");

      var FileIsZip = ZipArchive.IsZipFile(sourcePath);

      //TODO: Confirm that if an archive is already a zip file and has the proper extension then it is skipped
      if (FileIsZip && string.Equals(Path.GetExtension(sourcePath), ".cbz", StringComparison.CurrentCultureIgnoreCase))
        return;

      if (FileIsZip)
      {
        if (File.Exists(convertedPath) && options.OverwriteExisting)
          File.Delete(convertedPath);

        if (options.OverwriteExisting)
        {
          // TODO: rename existing file
        }
        else
        {
          // TODO: copy existing file to converted path
        }
      }
      else
      {
        var workingPath = Path.Combine(options.WorkingPath ?? AppContext.BaseDirectory, sourceFilename);

        using (Stream stream = File.OpenRead(sourcePath))
        using (var reader = ReaderFactory.Open(stream))
        {
          while (reader.MoveToNextEntry())
          {
            if (!reader.Entry.IsDirectory)
            {
              Console.WriteLine(reader.Entry.Key);

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
        }

        Directory.Delete(workingPath, true);
      }

      if (options.ReplaceOriginalFile && File.Exists(sourcePath))
        File.Delete(sourcePath);
    }
  }
}
