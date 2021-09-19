
using System.IO;
using System.Text;
using System.Xml.Serialization;

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

        public static ComicInfo ParseFilename()
        {
            return null;
        }

        public string MetadataAsText()
        {
            var builder = new StringBuilder();

            if (!HasMetadataStream)
                ReadMetadataFromArchive();

            return ComicInfo.FormatAsText();
        }
    }
}