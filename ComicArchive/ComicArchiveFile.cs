
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

    public string MetadataAsDisplayText()
    {
      var builder = new StringBuilder();

      if (!HasMetadataStream)
        ReadMetadataFromArchive();

      BuildMetadataDisplayString(builder , "Title", ComicInfo.Title);
      BuildMetadataDisplayString(builder , "Series", ComicInfo.Series);
      BuildMetadataDisplayString(builder , "Series Group", ComicInfo.SeriesGroup);
      BuildMetadataDisplayString(builder, "Issue", ComicInfo.Number);
      BuildMetadataDisplayString(builder, "# Issues", ComicInfo.Count);
      BuildMetadataDisplayString(builder, "Volume", ComicInfo.Volume);
      BuildMetadataDisplayString(builder, "Alternate Series", ComicInfo.AlternateSeries);
      BuildMetadataDisplayString(builder, "Alternate Number", ComicInfo.AlternateNumber);
      BuildMetadataDisplayString(builder, "Alternate Count", ComicInfo.AlternateCount);
      BuildMetadataDisplayString(builder, "Year", ComicInfo.Year);
      BuildMetadataDisplayString(builder, "Month", ComicInfo.Month);
      BuildMetadataDisplayString(builder, "Writer", ComicInfo.Writer);
      BuildMetadataDisplayString(builder, "Penciller", ComicInfo.Penciller);
      BuildMetadataDisplayString(builder, "Inker", ComicInfo.Inker);
      BuildMetadataDisplayString(builder, "Colorist", ComicInfo.Colorist);
      BuildMetadataDisplayString(builder, "Letterer", ComicInfo.Letterer);
      BuildMetadataDisplayString(builder, "Cover Artist", ComicInfo.CoverArtist);
      BuildMetadataDisplayString(builder, "Editor", ComicInfo.Editor);
      BuildMetadataDisplayString(builder, "Publisher", ComicInfo.Publisher);
      BuildMetadataDisplayString(builder, "Imprint", ComicInfo.Imprint);
      BuildMetadataDisplayString(builder, "Genre", ComicInfo.Genre);
      BuildMetadataDisplayString(builder, "Web", ComicInfo.Web);
      BuildMetadataDisplayString(builder, "Page Count", ComicInfo.PageCount);
      BuildMetadataDisplayString(builder, "Language ISO", ComicInfo.LanguageISO);
      BuildMetadataDisplayString(builder, "Format", ComicInfo.Format);
      BuildMetadataDisplayString(builder, "Black And White", ComicInfo.BlackAndWhite);
      BuildMetadataDisplayString(builder, "Manga", ComicInfo.Manga);
      BuildMetadataDisplayString(builder, "Characters", ComicInfo.Characters);
      BuildMetadataDisplayString(builder, "Teams", ComicInfo.Teams);
      BuildMetadataDisplayString(builder, "Locations", ComicInfo.Locations);
      BuildMetadataDisplayString(builder, "Scan Information", ComicInfo.ScanInformation);
      BuildMetadataDisplayString(builder, "Story Arc", ComicInfo.StoryArc);
      BuildMetadataDisplayString(builder, "Age Rating", ComicInfo.AgeRating);
      BuildMetadataDisplayString(builder, "Summary", ComicInfo.Summary);
      BuildMetadataDisplayString(builder, "Notes", ComicInfo.Notes);
      
      return builder.ToString();
    }

    private void BuildMetadataDisplayString(StringBuilder builder, string caption, string data)
    {
      if (!string.IsNullOrEmpty(data))
        builder.AppendLine($"{caption}: {data}");
    }
    private void BuildMetadataDisplayString(StringBuilder builder, string caption, int data)
    {
      if (data > -1)
        builder.AppendLine($"{caption}: {data}");
    }
    private void BuildMetadataDisplayString(StringBuilder builder, string caption, YesNo data)
    {
      if (data == YesNo.Unknown)
        return;
      
      builder.Append($"{caption}: ");
      builder.AppendLine(data == YesNo.Yes ? "Yes" : "No");
    }
    private void BuildMetadataDisplayString(StringBuilder builder, string caption, Manga data)
    {
      if (data == Manga.Unknown)
        return;

      builder.AppendLine($"{caption}: {data.DisplayString()}");
    }
    private void BuildMetadataDisplayString(StringBuilder builder, string caption, AgeRating data)
    {
      if (data == AgeRating.Unknown)
        return;

      builder.AppendLine($"{caption}: {data.DisplayString()}");
    }
  }
}