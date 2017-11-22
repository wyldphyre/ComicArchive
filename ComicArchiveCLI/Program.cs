using System;

namespace ComicArchiveCLI
{
  class Program
  {
    static void Main(string[] args)
    {
      var converter = new ComicArchive.Converter();
      var options = new ComicArchive.ConverterOptions();
      options.OverwriteExisting = true;
      //options.LogOptions.ShowFullPaths = true;

      // TODO: Implement command arguments of: Convert, Read, and (eventually) Write
      
      converter.LogActivityEvent += (message) => Console.WriteLine(message);

      try
      {
        //converter.ConvertToZipArchive(@"F:\Media\Comics\Testing\Big Man Plans\Big Man Plans 01 (of 04) (2015) (digital) (Son of Ultron-Empire).cbr", options, out var convertedPath);
        //var (result, message) = converter.ConvertToZipArchive(@"/Users/craigreynolds/Desktop/comictest/Big Hero 6 01 (of 05) (2008) (Digital) (AnPymGold-Empire).cbz", options, out var convertedPath);
        //var (result, message) = converter.ConvertToZipArchive(@"/Users/craigreynolds/Desktop/comictest/Big Hero 6 01 (of 05) (2008) (Digital) (AnPymGold-Empire).zip", options, out var convertedPath);

        // if (!string.IsNullOrWhiteSpace(message))
        //   Console.WriteLine(message);
      }
      catch (Exception e)
      {        
        Console.WriteLine($"Exception: {e.Message}\n\n");
        Console.WriteLine(e.StackTrace);
      }

      Console.WriteLine(string.Empty);
      Console.WriteLine("ComicArchive finshed!");
    }
  }
}
