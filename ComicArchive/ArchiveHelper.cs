
using System;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.PortableExecutable;
using System.Xml.Serialization;

using SharpCompress.Archives.GZip;
using SharpCompress.Archives.Rar;
using SharpCompress.Archives.SevenZip;
using SharpCompress.Archives.Tar;
using SharpCompress.Archives.Zip;
using SharpCompress.Common;
using SharpCompress.Readers;
using SharpCompress.Writers;
using SharpCompress.Writers.Zip;

namespace ComicArchive
{
    public static class ArchiveHelper
    {
        public const string comicRackMetadataFilename = "ComicInfo.xml";

        public static bool IsArchive(string path)
        {
            if (Directory.Exists(path))
            {
                return false;
            }

            return ZipArchive.IsZipFile(path) ||
              RarArchive.IsRarFile(path) ||
              SevenZipArchive.IsSevenZipFile(path) ||
              GZipArchive.IsGZipFile(path) ||
              TarArchive.IsTarFile(path);
        }

        public static bool IsZipArchive(string path)
        {
            return ZipArchive.IsZipFile(path);
        }

        public static MemoryStream GetComicRackMetadataFile(string path)
        {
            return GetFileFromArchive(path, comicRackMetadataFilename);
        }

        public static MemoryStream GetFileFromArchive(string path, string filename)
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

        internal static void WriteComicRackMetadataFile(string path, ComicInfo comicInfo)
        {
            var serializer = new XmlSerializer(typeof(ComicInfo));
            using var metadataStream = new MemoryStream();
            serializer.Serialize(metadataStream, comicInfo);
            metadataStream.Position = 0;

            const string updatedText = "_updated";
            var originalPathFolder = Path.GetDirectoryName(path);
            var originalFileName = Path.GetFileNameWithoutExtension(path);
            var originalExtension = Path.GetExtension(path);
            var newAchivePath = Path.Join(originalPathFolder, originalFileName + updatedText + originalExtension);

            if (System.IO.File.Exists(newAchivePath))
            {
                System.IO.File.Delete(newAchivePath);
            }

            var zipWriterOptions = new ZipWriterOptions(CompressionType.Deflate);

            using (Stream updatedFileStream = System.IO.File.OpenWrite(newAchivePath))
            using (var writer = new ZipWriter(updatedFileStream, zipWriterOptions))
            using (var archive = ZipArchive.Open(path))
            {
                var metadataWasFound = false;

                foreach (var entry in archive.Entries)
                {
                    if (entry.IsDirectory)
                    {
                        continue;
                    }

                    var isMetadataEntry = string.Equals(entry.Key, comicRackMetadataFilename, StringComparison.InvariantCultureIgnoreCase);

                    if (isMetadataEntry)
                    {
                        metadataWasFound = true;
                        writer.Write(entry.Key, metadataStream, DateTime.Now);
                    }
                    else
                    {
                        var entryStream = entry.OpenEntryStream();

                        writer.Write(entry.Key, entryStream, entry.LastAccessedTime);
                    }
                }

                if (!metadataWasFound)
                {
                    writer.Write(comicRackMetadataFilename, metadataStream, DateTime.Now);
                }
            }

            System.IO.File.Copy(newAchivePath, path, overwrite: true);
            System.IO.File.Delete(newAchivePath);
        }
    }
}