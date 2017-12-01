using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CLAP;
using CLAP.Validation;

namespace ComicArchiveCLI
{
  class ComicArchiveCLIApp
  {
    [Verb(Description = "Read the metadata for a comic archive. Only supports reading Comic Rack metadata.")]
    public static void Read(
      [Required]
      [FileExists] string path
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

      const string metadataFilename = "Comicinfo.xml";

      var metadataFile = ComicArchive.ArchiveHelper.GetFile(path, metadataFilename);

      if (metadataFile == null)
        Console.WriteLine($"Could not locate '{metadataFilename}'");
      else if (metadataFile.Length == 0)
        Console.WriteLine($"Metadata file '{metadataFilename}' is empty");
      else
        Console.WriteLine(Encoding.UTF8.GetString(metadataFile));
    }

    [Verb(Description = "Convert non-zip comic archives into zip archives.")]
    public static void Convert(
      [Required] 
      [Description("The file or folder of files to convert.")]
      [PathExists] 
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