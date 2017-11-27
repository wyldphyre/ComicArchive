using System;
using System.Runtime.InteropServices;
using CLAP;

namespace ComicArchiveCLI
{
  class Program
  {
    static void Main(string[] args)
    {
      Parser.Run<ComicArchiveCLIApp>(args);

#if DEBUG
      if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
      {
        Console.WriteLine("Press ENTER to exit");
        Console.ReadLine();
      }
#endif
    }
  }
}
