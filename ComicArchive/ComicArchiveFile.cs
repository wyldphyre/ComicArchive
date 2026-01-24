
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

using ComicArchive.ComicRack;
using SharpCompress.Readers;

namespace ComicArchive
{
    /// <summary>
    /// Represents a comic archive (.cbz, .cbr, etc)
    /// </summary>
    public sealed class File
    {
        private MemoryStream metadataStream;

        public File()
        {
            ComicInfo = new ComicInfo();
        }

        /// <summary>
        /// The filesystem path of the archive
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Metadata extracted from the archive
        /// </summary>
        public ComicInfo ComicInfo { get; private set; }

        /// <summary>
        /// Indicates whether a metadata file was found in the archive
        /// </summary>
        public bool HasMetadataStream => metadataStream != null;

        public MemoryStream MetadataStream => metadataStream;

        public void ReadMetadataFromArchive()
        {
            metadataStream = ArchiveHelper.GetComicRackMetadataFile(Path);

            if (metadataStream != null)
            {
                var serializer = new XmlSerializer(typeof(ComicInfo));
                ComicInfo = (ComicInfo)serializer.Deserialize(metadataStream);
            }
        }

        public void SaveMetadataToArchive()
        {
            ArchiveHelper.WriteComicRackMetadataFile(Path, ComicInfo);
        }

        public static ComicInfo ParseFilename()
        {
            // TODO: Implement parsing metadata from filename
            return null;
        }

        public int CountPagesInArchive()
        {
            int pageCount = 0;

            using (Stream stream = System.IO.File.OpenRead(Path))
            using (var reader = ReaderFactory.Open(stream))
            {
                while (reader.MoveToNextEntry())
                {
                    if (!reader.Entry.IsDirectory)
                    {
                        string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp", ".tiff", ".tif" };
                        var extension = System.IO.Path.GetExtension(reader.Entry.Key);

                        if (imageExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
                        {
                            pageCount++;
                        }
                    }
                }
            }

            return pageCount;
        }

        public string MetadataAsText()
        {
            if (!HasMetadataStream)
            {
                ReadMetadataFromArchive();
            }

            return ComicInfo.FormatAsText();
        }

        public void UpdateMetadata(Dictionary<string, string> newMetadata)
        {
            foreach (var kvp in newMetadata)
            {
                switch (kvp.Key.ToLower())
                {
                    case "title":
                        ComicInfo.Title = kvp.Value;
                        break;

                    case "series":
                        ComicInfo.Series = kvp.Value;
                        break;

                    case "number":
                        ComicInfo.Number = kvp.Value;
                        break;

                    case "volume":
                        if (!int.TryParse(kvp.Value, out int number))
                        {
                            throw new ArgumentOutOfRangeException(kvp.Key, "Not a valid integer");
                        }

                        ComicInfo.Volume = number;
                        break;

                    case "writer":
                        ComicInfo.Writer = kvp.Value;
                        break;

                    case "artist": // a special case to replicate the behaviour of ComicTagger
                        ComicInfo.Inker = kvp.Value;
                        ComicInfo.Penciller = kvp.Value;
                        break;

                    case "tags":
                        ComicInfo.Tags = kvp.Value;
                        break;

                    case "penciller":
                        ComicInfo.Penciller = kvp.Value;
                        break;

                    case "inker":
                        ComicInfo.Inker = kvp.Value;
                        break;

                    case "web":
                        ComicInfo.Web = kvp.Value;
                        break;

                    case "manga":
                        ComicInfo.Manga = MangaMapper.Map(kvp.Value);
                        break;

                    case "publisher":
                        ComicInfo.Publisher = kvp.Value;
                        break;

                    case "year":
                        if (int.TryParse(kvp.Value, out int year))
                        {
                            ComicInfo.Year = year;
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException(kvp.Key, "Not a valid integer");
                        }
                        break;

                    case "month":
                        if (int.TryParse(kvp.Value, out int month))
                        {
                            ComicInfo.Month = month;
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException(kvp.Key, "Not a valid integer");
                        }
                        break;

                    case "day":
                        if (int.TryParse(kvp.Value, out int day))
                        {
                            ComicInfo.Day = day;
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException(kvp.Key, "Not a valid integer");
                        }
                        break;

                    case "summary":
                        ComicInfo.Summary = kvp.Value;
                        break;

                    case "genre":
                        ComicInfo.Genre = kvp.Value;
                        break;

                    case "imprint":
                        ComicInfo.Imprint = kvp.Value;
                        break;

                    case "pagecount":
                        if (int.TryParse(kvp.Value, out int pageCount))
                        {
                            ComicInfo.PageCount = pageCount;
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException(kvp.Key, "Not a valid integer");
                        }
                        break;

                    case "agerating":
                        if (AgeRatingMapper.TryMap(kvp.Value, out var ageRating))
                        {
                            ComicInfo.AgeRating = ageRating;
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException(kvp.Key, $"Not a valid age rating: {kvp.Value}");
                        }
                        break;

                    default: throw new ArgumentException($"Metadata property not supported: {kvp.Key}");
                }
            }
        }
    }
}