
using System.IO;
using SharpCompress.Archives.Rar;
using SharpCompress.Archives.Zip;
using SharpCompress.Archives.GZip;
using SharpCompress.Archives.SevenZip;
using SharpCompress.Archives.Tar;

namespace ComicArchive
{
  public static class ArchiveHelper
  {
    public static bool IsArchive(string path)
    {
      if (Directory.Exists(path))
        return false;

      return ZipArchive.IsZipFile(path) ||
        RarArchive.IsRarFile(path) ||
        GZipArchive.IsGZipFile (path) ||
        SevenZipArchive.IsSevenZipFile(path) ||
        TarArchive.IsTarFile(path);
    }
  }
}