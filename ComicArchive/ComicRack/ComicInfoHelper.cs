using System;

namespace ComicArchive
{
  public static class ComicInfoHelper
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