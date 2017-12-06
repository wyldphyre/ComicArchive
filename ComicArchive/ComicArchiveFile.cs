
using System.IO;
using System.Xml.Serialization;

namespace ComicArchive
{
  public sealed class ComicArchiveFile
  {
    private MemoryStream metadataStream;

    public ComicArchiveFile() 
    {
      ComicInfo = new ComicInfo();
    }

    public string FilePath { get; set; }
    public ComicInfo ComicInfo { get; private set; }

    public void ReadMetadataFromArchive()
    {
      metadataStream = ArchiveHelper.GetComicRackMetadataFile(FilePath);

      var serializer = new XmlSerializer(typeof(ComicInfo));
      ComicInfo = (ComicInfo)serializer.Deserialize(metadataStream);
    }

    public bool HasMetadataStream
    {
      get { return metadataStream != null; }
    }
    public MemoryStream MetadataStream
    {
      get { return metadataStream; }
    }
  }
}