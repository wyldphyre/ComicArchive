using System;
using System.Text;

namespace ComicArchive.ComicRack
{
    public static class ComicInfoHelper
    {
        public static string FormatAsText(this ComicInfo comicInfo)
        {
            var builder = new StringBuilder();

            BuildMetadataDisplayString(builder, "Title", comicInfo.Title);
            BuildMetadataDisplayString(builder, "Series", comicInfo.Series);
            BuildMetadataDisplayString(builder, "Number", comicInfo.Number);
            BuildMetadataDisplayString(builder, "Count", comicInfo.Count);
            BuildMetadataDisplayString(builder, "Volume", comicInfo.Volume);
            BuildMetadataDisplayString(builder, "Alternate Series", comicInfo.AlternateSeries);
            BuildMetadataDisplayString(builder, "Alternate Number", comicInfo.AlternateNumber);
            BuildMetadataDisplayString(builder, "Alternate Count", comicInfo.AlternateCount);
            BuildMetadataDisplayString(builder, "Summary", comicInfo.Summary);
            BuildMetadataDisplayString(builder, "Notes", comicInfo.Notes);
            BuildMetadataDisplayString(builder, "Year", comicInfo.Year);
            BuildMetadataDisplayString(builder, "Month", comicInfo.Month);
            BuildMetadataDisplayString(builder, "Day", comicInfo.Day);
            BuildMetadataDisplayString(builder, "Writer", comicInfo.Writer);
            BuildMetadataDisplayString(builder, "Penciller", comicInfo.Penciller);
            BuildMetadataDisplayString(builder, "Inker", comicInfo.Inker);
            BuildMetadataDisplayString(builder, "Colorist", comicInfo.Colorist);
            BuildMetadataDisplayString(builder, "Letterer", comicInfo.Letterer);
            BuildMetadataDisplayString(builder, "Cover Artist", comicInfo.CoverArtist);
            BuildMetadataDisplayString(builder, "Editor", comicInfo.Editor);
            BuildMetadataDisplayString(builder, "Translator", comicInfo.Translator);
            BuildMetadataDisplayString(builder, "Publisher", comicInfo.Publisher);
            BuildMetadataDisplayString(builder, "Imprint", comicInfo.Imprint);
            BuildMetadataDisplayString(builder, "Genre", comicInfo.Genre);
            BuildMetadataDisplayString(builder, "Tags", comicInfo.Tags);
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
            BuildMetadataDisplayString(builder, "Story Arc Number", comicInfo.StoryArcNumber);
            BuildMetadataDisplayString(builder, "Series Group", comicInfo.SeriesGroup);
            BuildMetadataDisplayString(builder, "Age Rating", comicInfo.AgeRating);

            if (comicInfo.CommunityRatingSpecified)
            {
                BuildMetadataDisplayString(builder, "Community Rating", comicInfo.CommunityRating);
            }

            BuildMetadataDisplayString(builder, "Main Character or Team", comicInfo.MainCharacterOrTeam);
            BuildMetadataDisplayString(builder, "Review", comicInfo.Review);
            BuildMetadataDisplayString(builder, "GTIN", comicInfo.GTIN);

            return builder.ToString();
        }

        private static void BuildMetadataDisplayString(StringBuilder builder, string caption, string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                builder.AppendLine($"{caption}: {data}");
            }
        }
        private static void BuildMetadataDisplayString(StringBuilder builder, string caption, int data)
        {
            if (data > -1)
            {
                builder.AppendLine($"{caption}: {data}");
            }
        }
        private static void BuildMetadataDisplayString(StringBuilder builder, string caption, decimal data)
        {
            if (data > -1)
            {
                builder.AppendLine($"{caption}: {data}");
            }
        }
        private static void BuildMetadataDisplayString(StringBuilder builder, string caption, YesNo data)
        {
            if (data == YesNo.Unknown)
            {
                return;
            }

            builder.Append($"{caption}: ");
            builder.AppendLine(data == YesNo.Yes ? "Yes" : "No");
        }
        private static void BuildMetadataDisplayString(StringBuilder builder, string caption, Manga data)
        {
            if (data == Manga.Unknown)
            {
                return;
            }

            builder.AppendLine($"{caption}: {data.Map()}");
        }
        private static void BuildMetadataDisplayString(StringBuilder builder, string caption, AgeRating data)
        {
            if (data == AgeRating.Unknown)
            {
                return;
            }

            builder.AppendLine($"{caption}: {data.Map()}");
        }
    }

    public static class AgeRatingMapper
    {
        public static string Map(this AgeRating rating)
        {
            return rating switch
            {
                AgeRating.Unknown => "Unknown",
                AgeRating.AdultsOnly18 => "Adults Only 18+",
                AgeRating.EarlyChildhood => "Early Childhood",
                AgeRating.Everyone => "Everyone",
                AgeRating.Everyone10 => "Everyone 10+",
                AgeRating.G => "G",
                AgeRating.KidstoAdults => "Kids to Adults",
                AgeRating.M => "M",
                AgeRating.MA15 => "MA 15+",
                AgeRating.Mature17 => "Mature 17+",
                AgeRating.PG => "PG",
                AgeRating.R18 => "R18+",
                AgeRating.RatingPending => "Rating Pending",
                AgeRating.Teen => "Teen",
                AgeRating.X18 => "X18+",
                _ => throw new Exception($"Unhandled Age Rating: {rating}"),
            };
        }
    }

    public static class MangaMapper
    {
        public static string Map(this Manga manga)
        {
            return manga switch
            {
                Manga.No => "No",
                Manga.Yes => "Yes",
                Manga.YesAndRightToLeft => "Yes (right to left)",
                Manga.Unknown => "Unknown",
                _ => throw new Exception($"Unhandled Manga value: {manga}"),
            };
        }
        
        public static Manga Map(this string manga)
        {
            return manga switch
            {
                "No" => Manga.No,
                "Yes" => Manga.Yes ,
                "YesAndRightToLeft" => Manga.YesAndRightToLeft ,
                "Yes (right to left)" => Manga.YesAndRightToLeft ,
                "Unknown" => Manga.Unknown ,
                _ => throw new Exception($"Unhandled Manga value: {manga}"),
            };
        }
    }
}