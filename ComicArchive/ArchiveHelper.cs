
using System.IO;
using SharpCompress.Archives.Rar;
using SharpCompress.Archives.Zip;
using SharpCompress.Archives.GZip;
using SharpCompress.Archives.SevenZip;
using SharpCompress.Archives.Tar;
using SharpCompress.Readers;
using System;

namespace ComicArchive
{
  public static class ArchiveHelper
  {
    public const string comicRackMetadataFilename = "Comicinfo.xml";

    public static bool IsArchive(string path)
    {
      if (Directory.Exists(path))
        return false;

      return ZipArchive.IsZipFile(path) ||
        RarArchive.IsRarFile(path) ||
        SevenZipArchive.IsSevenZipFile(path) ||
        GZipArchive.IsGZipFile (path) ||
        TarArchive.IsTarFile(path);
    }

    public static MemoryStream GetComicRackMetadataFile(string path)
    {
      return GetFile(path, comicRackMetadataFilename);
    }

    public static MemoryStream GetFile(string path, string filename)
    {
      MemoryStream fileStream = null;

      using (Stream stream = System.IO.File.OpenRead(path))
      using (var reader = ReaderFactory.Open(stream))
      {
        while (reader.MoveToNextEntry() && fileStream == null)
        {
          if (string.Equals(reader.Entry.Key, filename, StringComparison.InvariantCultureIgnoreCase))
          {
            fileStream = new MemoryStream();
            reader.WriteEntryTo(fileStream);
            fileStream.Position = 0;
          }
        }
      }

      return fileStream;
    }
  }
}