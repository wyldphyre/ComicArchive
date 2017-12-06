using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CLAP;
using CLAP.Validation;
using ComicArchive;

namespace ComicArchiveCLI
{
  class ComicArchiveCLIApp
  {
    [Verb(Description = "Read the metadata for a comic archive. Only supports reading Comic Rack metadata.")]
    public static void Read(
      [Required]
      [FileExists] 
      string path
    )
    {
      if (Directory.Exists(path))
      {
        Console.WriteLine($"{path} is a directory. Pass a file path instead.");
        return;
      }

      if (!ComicArchive.ArchiveHelper.IsArchive(path))
      {
        Console.WriteLine($"{path} is not an archive.");
        return;
      }

      try
      {
        var comic = new ComicArchiveFile { FilePath = path };
        comic.ReadMetadataFromArchive();

        // TODO: Create a helper class for ComicInfo, along with an extension method to generate a text
        // representation of the metadata present.
        Console.WriteLine($"Title: {comic.ComicInfo.Title}");

        if (!comic.HasMetadataStream || comic.MetadataStream.Length == 0)
        {
          // TODO: Extend the library to parse metadata from the file name
          Console.WriteLine($"Comic Rack metadata file '{ArchiveHelper.comicRackMetadataFilename}' missing.");
        }
        else
          Console.WriteLine(Encoding.UTF8.GetString(comic.MetadataStream.GetBuffer()));
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
      }
    }

    [Verb(Description = "Convert non-zip comic archives into zip archives.")]
    public static void Convert(
      [Required] 
	  [PathExists] 
      [Description("The file or folder of files to convert.")]
      string path,
      [Description("If a file with the same name as the conversion target exists, replace it.")]
      bool overwrite,
      [Description("Delete the original file once the conversion is complete.")]
      bool replace,
      [Description("Show full file paths instead of just the files name.")]
      bool showFullPaths
    )
    {
      var options = new ComicArchive.ConverterOptions
      {
        OverwriteExisting = overwrite,
        ReplaceOriginalFile = replace
      };
      options.LogOptions.ShowFullPaths = showFullPaths;

      var converter = new ComicArchive.Converter();

      converter.LogActivityEvent += (message) => Console.WriteLine(message);

      var filePathsToConvert = new List<string>();

      if (Directory.Exists(path))
      {
        var archivePaths = Directory.GetFiles(path)
          .Where(p => !Path.GetFileName(p).StartsWith(".") && ComicArchive.ArchiveHelper.IsArchive(p));

        filePathsToConvert.AddRange(archivePaths);
      }
      else if (File.Exists(path))
        filePathsToConvert.Add(path);

      try
      {
        foreach (var filePath in filePathsToConvert)
        {
          var (result, message) = converter.ConvertToZipArchive(filePath, options, out var convertedPath);

          if (!string.IsNullOrWhiteSpace(message))
            Console.WriteLine(message);
        }
      }
      catch (Exception e)
      {
        Console.WriteLine($"Exception: {e.Message}\n\n");
        Console.WriteLine(e.StackTrace);
      }

      Console.WriteLine(string.Empty);
      Console.WriteLine("Conversion finshed!");
      // TODO: output some stats on files skipped and files converted
    }

    [Empty, Help]
    public static void Help(string help)
    {
      Console.WriteLine(help);
    }

    [Error]
    public static void Error(CLAP.ExceptionContext context)
    {
      Console.WriteLine(context.Exception.Message);
      //Console.WriteLine(context.Exception.StackTrace);
    }
  }
}