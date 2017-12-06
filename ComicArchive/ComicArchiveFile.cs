
using System.IO;
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