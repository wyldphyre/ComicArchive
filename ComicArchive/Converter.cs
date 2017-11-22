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

      if (File.Exists(convertedPath))
        throw new Exception($"Cannot convert path '{sourcePath} to Zip. Target path '{convertedPath}' already exists");

      //TODO: check if file is already a zip archive and has the proper extension do nothing

      var workingPath = Path.Combine(options.WorkingPath ?? AppContext.BaseDirectory, sourceFilename);

      // TODO: If file is a zip and just needs the extension changes, then duplicate to new file with the proper extension. If Options.ReplaceOriginalFile is true then just rename the file instead of duplicating
      // If file is not already a zip archive then extract and create a new zip archive with the contents of the original archive. If Options.ReplaceOriginalFile is set then delete original file

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
  }
}
