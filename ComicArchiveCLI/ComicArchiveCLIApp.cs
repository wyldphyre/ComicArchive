using System;
using System.Collections.Generic;
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
      [FileExists] string filePath
      )
    {
    }

    [Verb(Description = "Convert non-zip comic archives into zip archives.")]
    public static void Convert(
      [Required]
      [Description("The file or folder of files to convert")]
      [PathExists] string sourcePath
      )
    {
      var options = new ComicArchive.ConverterOptions
      {
        OverwriteExisting = true
      };
      //options.LogOptions.ShowFullPaths = true;

      var converter = new ComicArchive.Converter();

      converter.LogActivityEvent += (message) => Console.WriteLine(message);

      try
      {
        var (result, message) = converter.ConvertToZipArchive(sourcePath, options, out var convertedPath);

        // Some paths for testing
        //@"F:\Media\Comics\Testing\Big Man Plans\Big Man Plans 01 (of 04) (2015) (digital) (Son of Ultron-Empire).cbr"
        //@"/Users/craigreynolds/Desktop/comictest/Big Hero 6 01 (of 05) (2008) (Digital) (AnPymGold-Empire).cbz"
        //(@"/Users/craigreynolds/Desktop/comictest/Big Hero 6 01 (of 05) (2008) (Digital) (AnPymGold-Empire).zip"

        if (!string.IsNullOrWhiteSpace(message))
          Console.WriteLine(message);
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
  }
}
