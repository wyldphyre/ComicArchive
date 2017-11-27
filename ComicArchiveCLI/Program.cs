using System;
using CLAP;

namespace ComicArchiveCLI
{
  class Program
  {
    static void Main(string[] args)
    {
      Parser.Run<ComicArchiveCLIApp>(args);

#if DEBUG
      Console.WriteLine("Press ENTER to exit");
      Console.ReadLine();
#endif
    }
  }
}
