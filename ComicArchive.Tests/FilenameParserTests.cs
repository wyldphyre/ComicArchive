using Shouldly;
using System;
using Xunit;

namespace ComicArchive.Tests
{
    public partial class FilenameParserTests
    {
        [Theory]
        [MemberData(nameof(ParseSampleData))]
        public void TestParse(string filename, ParsedFilenameData data)
        {
            FilenameParser.Parse(filename).ShouldBeEquivalentTo(data);
        }

        [Theory]
        [MemberData(nameof(TokenisedFilenameData))]
        public void TestTokeniseToWords(string filename, string[] tokens)
        {
            FilenameParser.TokeniseToWords(filename).ShouldBe(tokens);
        }

        [Theory]
        [MemberData(nameof(ParseSampleData))]
        public void TestParseOfVolume(string filename, ParsedFilenameData data)
        {
            FilenameParser.Parse(filename).Volume.ShouldBe(data.Volume);
        }

        [Theory]
        [MemberData(nameof(ParseSampleData))]
        public void TestParseOfNumber(string filename, ParsedFilenameData data)
        {
            FilenameParser.Parse(filename).Number.ShouldBe(data.Number);
        }

        [Theory]
        [MemberData(nameof(ParseSampleData))]
        public void TestParseOfSeries(string filename, ParsedFilenameData data)
        {
            FilenameParser.Parse(filename).Series.ShouldBe(data.Series);
        }

        [Theory]
        [MemberData(nameof(ParseSampleData))]
        public void TestParseOfYear(string filename, ParsedFilenameData data)
        {
            FilenameParser.Parse(filename).Year.ShouldBe(data.Year);
        }

        [Theory]
        [MemberData(nameof(ParseSampleData))]
        public void TestParseOfArtist(string filename, ParsedFilenameData data)
        {
            FilenameParser.Parse(filename).Artist.ShouldBe(data.Artist);
        }

        [Theory]
        [MemberData(nameof(ParseSampleData))]
        public void TestParseOfName(string filename, ParsedFilenameData data)
        {
            FilenameParser.Parse(filename).Name.ShouldBe(data.Name);
        }

        public static TheoryData<string, ParsedFilenameData> ParseSampleData = new TheoryData<string, ParsedFilenameData>()
        {
            // Common filename patterns
            { "Vampeerz v1 ch01.cbz", new ParsedFilenameData { Volume = 1, Number = "1", Series = "Vampeerz" } },
            { "Vampeerz volume 1 ch01.cbz", new ParsedFilenameData { Volume = 1, Number = "1", Series = "Vampeerz" } },
            { "Vampeerz volume. 1 ch01.cbz", new ParsedFilenameData { Volume = 1, Number = "1", Series = "Vampeerz" } },
            { "Assassination Classroom v01 (2014) (Digital) (Lovag-Empire).cbz", new ParsedFilenameData { Volume = 1, Year = 2014, Series = "Assassination Classroom" } },
            { "Assassination Classroom v01.cbz", new ParsedFilenameData { Volume = 1, Series = "Assassination Classroom" } },
            { "Assassination Classroom v10.cbz", new ParsedFilenameData { Volume = 10, Series = "Assassination Classroom" } },
            { "Assassination Classroom vol01.cbz", new ParsedFilenameData { Volume = 1, Series = "Assassination Classroom" } },
            { "Assassination Classroom vol10.cbz", new ParsedFilenameData { Volume = 10, Series = "Assassination Classroom" } },
            { "Assassination Classroom vol 10.cbz", new ParsedFilenameData { Volume = 10, Series = "Assassination Classroom" } },
            { "Assassination Classroom vol. 10.cbz", new ParsedFilenameData { Volume = 10, Series = "Assassination Classroom" } },
            { "Assassination Classroom vol NotA Volume.cbz", new ParsedFilenameData { Series = "Assassination Classroom" } },
            { "Assassination Classroom 01.cbz", new ParsedFilenameData { Number = "1", Series = "Assassination Classroom" } },
            { "Assassination Classroom ch01.cbz", new ParsedFilenameData { Number = "1", Series = "Assassination Classroom" } },
            { "Assassination Classroom c01.cbz", new ParsedFilenameData { Number = "1", Series = "Assassination Classroom" } },
            { "Assassination Classroom ch 01.cbz", new ParsedFilenameData { Number = "1", Series = "Assassination Classroom" } },
            { "Assassination Classroom ch. 01.cbz", new ParsedFilenameData { Number = "1", Series = "Assassination Classroom" } },
            { "Assassination Classroom ch 10.1.cbz", new ParsedFilenameData { Number = "10.1", Series = "Assassination Classroom" } },
            { "Assassination Classroom ch. 10.1.cbz", new ParsedFilenameData { Number = "10.1", Series = "Assassination Classroom" } },
            { "Assassination Classroom chp. 10.1.cbz", new ParsedFilenameData { Number = "10.1", Series = "Assassination Classroom" } },
            { "Assassination Classroom Chp. 10.1.cbz", new ParsedFilenameData { Number = "10.1", Series = "Assassination Classroom" } },
            { "Assassination Classroom chapter 01.cbz", new ParsedFilenameData { Number = "1", Series = "Assassination Classroom" } },
            { "Assassination Classroom Chapter 10.1.cbz", new ParsedFilenameData { Number = "10.1", Series = "Assassination Classroom" } },
            { "Assassination Classroom Vol. 001 Ch. 001.cbz", new ParsedFilenameData { Number = "1", Volume=1, Series = "Assassination Classroom" } },
            { "Ah My Goddess 1.cbz", new ParsedFilenameData { Number = "1", Series = "Assassination Ah My Goddess" } },
            { "Ah My Goddess 10.cbz", new ParsedFilenameData { Number = "10", Series = "Assassination Ah My Goddess" } },
            { "Claymore 001 - Silver-eyed Slayer[m-s].cbz", new ParsedFilenameData { Number = "1", Name = "Silver-eyed Slayer", Series = "Claymore" } }, // : includes chapter name
            { "Claymore 002 - Claws in the Sky [m-s].cbz", new ParsedFilenameData { Number = "2", Name = "Claws in the Sky", Series = "Claymore" } }, // : includes chapter name
            { "Claymore 002 - [m-s] Claws in the Sky.cbz", new ParsedFilenameData { Number = "2", Name = "Claws in the Sky", Series = "Claymore" } }, // : includes chapter name
            { "[Tokuwotsumu] Tea Brown and Milk Tea [TZdY].cbz", new ParsedFilenameData { Series = "Tea Brown and Milk Tea", Artist = "Tokuwotsumu" } },
            { "(Isoya Yuki) The Day the Cherryfruit Ripens (Hirari 14) [project].cbz", new ParsedFilenameData { Series = "The Day the Cherryfruit Ripens", Artist = "Isoya Yuki" } },
            { "[Garun] I Could Just Tell.cbz", new ParsedFilenameData { Series = "I Could Just Tell", Artist = "Garun" } },
            { "[Takemiya Jin] Yaezakura Sympathy 1 [TZdY].cbz", new ParsedFilenameData { Artist = "Takemiya Jin", Series = "Yaesakura Sympathy", Number = "1" } },
            { "04.cbz", new ParsedFilenameData { Number = "4" } },
            { "vol04.cbz", new ParsedFilenameData { Volume = 4 } },
            { "vol 04.cbz", new ParsedFilenameData { Volume = 4 } },
            { "volume 04.cbz", new ParsedFilenameData { Volume = 4 } },
            { "volume. 04.cbz", new ParsedFilenameData { Volume = 4 } },
            { "05 - Let's Be Careful With Summer.cbz", new ParsedFilenameData { Number = "5", Name = "Let's Be Careful With Summer" } }, // : assume chapter number and name
            { "05 - Let's Be Careful With Summer (test) [test].cbz", new ParsedFilenameData { Number = "5", Name = "Let's Be Careful With Summer" } }, // : assume chapter number and name
            { "05 - Let's Be Careful With Summer(test)[test].cbz", new ParsedFilenameData { Number = "5", Name = "Let's Be Careful With Summer" } }, // : assume chapter number and name
            { "2000 AD 0001.cbz", new ParsedFilenameData { Number = "1", Series = "2000 AD" } },
            { "2000 AD 0345 (Cclay).cbz", new ParsedFilenameData { Number = "345", Series = "2000 AD" } },
            { "Ascender 001 (2019) (Digital) (Zone-Empire).cbz", new ParsedFilenameData { Number = "1", Series = "Ascender", Year = 2019 } },
        };

        public static TheoryData<string, string[]> TokenisedFilenameData = new TheoryData<string, string[]>()
        {
            // Common filename patterns
            { "Vampeerz v1 ch01.cbz", new[] { "Vampeerz", "v1", "ch01" } },
            { "Assassination Classroom v01 (2014) (Digital) (Lovag-Empire).cbz", new[] { "Assassination", "Classroom", "v01", "(2014)", "(Digital)", "(Lovag-Empire)" } },
            { "Assassination Classroom v01.cbz", new[] { "Assassination", "Classroom", "v01" } },
            { "Assassination Classroom v10.cbz", new[] { "Assassination", "Classroom", "v10" } },
            { "Assassination Classroom vol01.cbz", new[] { "Assassination", "Classroom", "vol01" } },
            { "Assassination Classroom vol 10.cbz", new[] { "Assassination", "Classroom", "vol", "10" } },
            { "Assassination Classroom ch01.cbz", new[] { "Assassination", "Classroom", "ch01" } },
            { "Assassination Classroom ch 01.cbz", new[] { "Assassination", "Classroom", "ch", "01" } },
            { "Ah My Goddess 1.cbz", new[] { "Ah", "My", "Goddess", "1" } },
            { "Ah My Goddess 10.cbz", new[] { "Ah", "My", "Goddess", "10" } },
            { "Claymore 001 - Silver-eyed Slayer[m-s].cbz", new[] { "Claymore", "001", "-", "Silver-eyed", "Slayer[m-s]" } }, // : includes chapter name
            { "[Tokuwotsumu] Tea Brown and Milk Tea [TZdY].cbz", new[] { "[Tokuwotsumu]", "Tea", "Brown", "and", "Milk", "Tea", "[TZdY]" } },
            { "(Isoya Yuki) The Day the Cherryfruit Ripens (Hirari 14) [project].cbz", new[] { "(Isoya Yuki)", "The", "Day", "the", "Cherryfruit", "Ripens", "(Hirari 14)", "[project]" } },
            { "[Garun] I Could Just Tell.cbz", new[] { "[Garun]", "I", "Could", "Just", "Tell" } },
            { "[Takemiya Jin] Yaezakura Sympathy 1 [TZdY].cbz", new[] { "[Takemiya Jin]", "Yaezakura", "Sympathy", "1", "[TZdY]" } },
            { "04.cbz", new[] { "04" } },
            { "05 - Let's Be Careful With Summer.cbz", new[] { "05", "-", "Let's", "Be", "Careful", "With", "Summer" } }, // : assume chapter number and name
            { "2000 AD 0001.cbz", new[] { "2000", "AD", "0001" } },
            { "2000 AD 0345 (Cclay).cbz", new[] { "2000", "AD", "0345", "(Cclay)" } },

            // missing closing brackets
            { "(Isoya Yuki) The Day (Hirari 14) [project].cbz", new[] { "(Isoya Yuki)", "The", "Day", "(Hirari 14)", "[project]" } },
            { "(Isoya Yuki) The Day (Hirari 14) [project.cbz", new[] { "(Isoya Yuki)", "The", "Day", "(Hirari 14)", "[project" } },
            { "(Isoya Yuki) The Day (Hirari 14) [project sd.cbz", new[] { "(Isoya Yuki)", "The", "Day", "(Hirari 14)", "[project sd" } },
            { "(Isoya Yuki The Day (Hirari 14) [project].cbz", new[] { "(Isoya Yuki The Day (Hirari 14)", "[project]" } },
            { "[Garun I Could Just Tell.cbz", new[] { "[Garun I Could Just Tell" } },

            // Uncommon patterns
            { "all.you.need.is.kill.001..kiriya.keij.cbz", new[] { "all", "you", "need", "is", "kill", "001", "kiriya", "keij" } },
            { "01 Prologue_-_Hidden_Name_[lililicious].cbz", new[] { "01", "Prologue", "-", "Hidden", "Name", "[lililicious]" } }
        };
    }
}
