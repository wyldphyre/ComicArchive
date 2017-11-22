using System;

namespace ComicArchiveCLI
{
  class Program
  {
    static void Main(string[] args)
    {
      var converter = new ComicArchive.Converter();
      var options = new ComicArchive.ConverterOptions();

      // TODO: Implement command arguments of: Convert, Read, and (eventually) Write

      //converter.ConvertToZipArchive(@"F:\Media\Comics\Testing\Big Man Plans\Big Man Plans 01 (of 04) (2015) (digital) (Son of Ultron-Empire).cbr", options, out var convertedPath);

      Console.WriteLine("Hello World!");
    }
  }
}
