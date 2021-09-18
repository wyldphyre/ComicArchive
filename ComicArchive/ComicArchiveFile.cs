
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace ComicArchive
{
    public sealed class File
    {
        private MemoryStream metadataStream;

        public File()
        {
            ComicInfo = new ComicInfo();
        }

        public string Path { get; set; }
        public ComicInfo ComicInfo { get; private set; }

        public void ReadMetadataFromArchive()
        {
            metadataStream = ArchiveHelper.GetComicRackMetadataFile(Path);

            if (metadataStream != null)
            {
                var serializer = new XmlSerializer(typeof(ComicInfo));
                ComicInfo = (ComicInfo)serializer.Deserialize(metadataStream);
            }
        }

        public bool HasMetadataStream => metadataStream != null;
        public MemoryStream MetadataStream => metadataStream;

        public string MetadataAsText()
        {
            var builder = new StringBuilder();

            if (!HasMetadataStream)
                ReadMetadataFromArchive();

            return ComicInfo.FormatAsText();
        }
    }
}