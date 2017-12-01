
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
        GZipArchive.IsGZipFile (path) ||
        SevenZipArchive.IsSevenZipFile(path) ||
        TarArchive.IsTarFile(path);
    }

    public static byte[] GetComicRackMetadataFile(string path)
    {
      return GetFile(path, comicRackMetadataFilename);
    }

    public static byte[] GetFile(string path, string filename)
    {
      byte[] fileData = null;

      using (Stream stream = File.OpenRead(path))
      using (var reader = ReaderFactory.Open(stream))
      {
        while (reader.MoveToNextEntry() && fileData == null)
        {
          if (string.Equals(reader.Entry.Key, filename, StringComparison.InvariantCultureIgnoreCase))
          {
            var fileStream = new MemoryStream();
            reader.WriteEntryTo(fileStream);
            fileData = fileStream.GetBuffer();
          }
        }
      }

      return fileData;
    }
  }
}