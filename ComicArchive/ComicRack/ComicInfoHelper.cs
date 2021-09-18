using System;
using System.Text;

namespace ComicArchive
{
    public static class ComicInfoHelper
    {
        public static string FormatAsText(this ComicInfo comicInfo)
        {
            var builder = new StringBuilder();

            BuildMetadataDisplayString(builder, "Title", comicInfo.Title);
            BuildMetadataDisplayString(builder, "Series", comicInfo.Series);
            BuildMetadataDisplayString(builder, "Series Group", comicInfo.SeriesGroup);
            BuildMetadataDisplayString(builder, "Issue", comicInfo.Number);
            BuildMetadataDisplayString(builder, "# Issues", comicInfo.Count);
            BuildMetadataDisplayString(builder, "Volume", comicInfo.Volume);
            BuildMetadataDisplayString(builder, "Alternate Series", comicInfo.AlternateSeries);
            BuildMetadataDisplayString(builder, "Alternate Number", comicInfo.AlternateNumber);
            BuildMetadataDisplayString(builder, "Alternate Count", comicInfo.AlternateCount);
            BuildMetadataDisplayString(builder, "Year", comicInfo.Year);
            BuildMetadataDisplayString(builder, "Month", comicInfo.Month);
            BuildMetadataDisplayString(builder, "Writer", comicInfo.Writer);
            BuildMetadataDisplayString(builder, "Penciller", comicInfo.Penciller);
            BuildMetadataDisplayString(builder, "Inker", comicInfo.Inker);
            BuildMetadataDisplayString(builder, "Colorist", comicInfo.Colorist);
            BuildMetadataDisplayString(builder, "Letterer", comicInfo.Letterer);
            BuildMetadataDisplayString(builder, "Cover Artist", comicInfo.CoverArtist);
            BuildMetadataDisplayString(builder, "Editor", comicInfo.Editor);
            BuildMetadataDisplayString(builder, "Publisher", comicInfo.Publisher);
            BuildMetadataDisplayString(builder, "Imprint", comicInfo.Imprint);
            BuildMetadataDisplayString(builder, "Genre", comicInfo.Genre);
            BuildMetadataDisplayString(builder, "Web", comicInfo.Web);
            BuildMetadataDisplayString(builder, "Page Count", comicInfo.PageCount);
            BuildMetadataDisplayString(builder, "Language ISO", comicInfo.LanguageISO);
            BuildMetadataDisplayString(builder, "Format", comicInfo.Format);
            BuildMetadataDisplayString(builder, "Black And White", comicInfo.BlackAndWhite);
            BuildMetadataDisplayString(builder, "Manga", comicInfo.Manga);
            BuildMetadataDisplayString(builder, "Characters", comicInfo.Characters);
            BuildMetadataDisplayString(builder, "Teams", comicInfo.Teams);
            BuildMetadataDisplayString(builder, "Locations", comicInfo.Locations);
            BuildMetadataDisplayString(builder, "Scan Information", comicInfo.ScanInformation);
            BuildMetadataDisplayString(builder, "Story Arc", comicInfo.StoryArc);
            BuildMetadataDisplayString(builder, "Age Rating", comicInfo.AgeRating);
            BuildMetadataDisplayString(builder, "Summary", comicInfo.Summary);
            BuildMetadataDisplayString(builder, "Notes", comicInfo.Notes);

            return builder.ToString();
        }

        private static void BuildMetadataDisplayString(StringBuilder builder, string caption, string data)
        {
            if (!string.IsNullOrEmpty(data))
                builder.AppendLine($"{caption}: {data}");
        }
        private static void BuildMetadataDisplayString(StringBuilder builder, string caption, int data)
        {
            if (data > -1)
                builder.AppendLine($"{caption}: {data}");
        }
        private static void BuildMetadataDisplayString(StringBuilder builder, string caption, YesNo data)
        {
            if (data == YesNo.Unknown)
                return;

            builder.Append($"{caption}: ");
            builder.AppendLine(data == YesNo.Yes ? "Yes" : "No");
        }
        private static void BuildMetadataDisplayString(StringBuilder builder, string caption, Manga data)
        {
            if (data == Manga.Unknown)
                return;

            builder.AppendLine($"{caption}: {data.DisplayString()}");
        }
        private static void BuildMetadataDisplayString(StringBuilder builder, string caption, AgeRating data)
        {
            if (data == AgeRating.Unknown)
                return;

            builder.AppendLine($"{caption}: {data.DisplayString()}");
        }
    }

    public static class AgeRatingHelper
    {
        public static string DisplayString(this AgeRating rating)
        {
            switch (rating)
            {
                case AgeRating.Unknown: return "Unknown";
                case AgeRating.AdultsOnly18: return "Adults Only 18+";
                case AgeRating.EarlyChildhood: return "Early Childhood";
                case AgeRating.Everyone: return "Everyone";
                case AgeRating.Everyone10: return "Everyone 10+";
                case AgeRating.G: return "G";
                case AgeRating.KidstoAdults: return "Kids to Adults";
                case AgeRating.M: return "M";
                case AgeRating.MA15: return "MA 15+";
                case AgeRating.Mature17: return "Mature 17+";
                case AgeRating.PG: return "PG";
                case AgeRating.R18: return "R18+";
                case AgeRating.RatingPending: return "Rating Pending";
                case AgeRating.Teen: return "Teen";
                case AgeRating.X18: return "X18+";

                default:
                    throw new Exception($"Unhandled Age Rating: {rating}");
            }
        }
    }

    public static class MangaHelper
    {
        public static string DisplayString(this Manga manga)
        {
            switch (manga)
            {
                case Manga.No: return "No";
                case Manga.Yes: return "Yes";
                case Manga.YesAndRightToLeft: return "Yes (right to left)";
                default:
                    throw new Exception($"Unhandled Age Rating: {manga}");
            }
        }
    }
}