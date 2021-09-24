using Shouldly;
using System;
using Xunit;

namespace ComicArchive.Tests
{
    public partial class FilenameParserTests
    {
        [Theory]
        [MemberData(nameof(TokenisedFilenameData))]
        public void TestTokeniseToWords(string filename, string[] tokens)
        {
            FilenameParser.TokeniseToWords(filename).ShouldBe(tokens);
        }

        [Theory]
        [MemberData(nameof(ParseSampleData))]
        public void TestParse(string filename, ParsedFilenameData data)
        {
            FilenameParser.Parse(filename).Volume.ShouldBe(data.Volume);
        }

        public static TheoryData<string, ParsedFilenameData> ParseSampleData = new TheoryData<string, ParsedFilenameData>()
        {
            // Common filename patterns
            { "Vampeerz v1 ch01.cbz", new ParsedFilenameData { Volume = 1 } },
            { "Assassination Classroom v01 (2014) (Digital) (Lovag-Empire).cbz", new ParsedFilenameData { Volume = 1 } },
            { "Assassination Classroom v01.cbz", new ParsedFilenameData { Volume = 1 } },
            { "Assassination Classroom v10.cbz", new ParsedFilenameData { Volume = 10 } },
            { "Assassination Classroom vol01.cbz", new ParsedFilenameData { Volume = 1 } },
            { "Assassination Classroom vol10.cbz", new ParsedFilenameData { Volume = 10 } },
            { "Assassination Classroom vol 10.cbz", new ParsedFilenameData { Volume = 10 } },
            { "Assassination Classroom vol NotA Volume.cbz", new ParsedFilenameData { } },
            { "Assassination Classroom 01.cbz", new ParsedFilenameData { } },
            { "Assassination Classroom ch01.cbz", new ParsedFilenameData { } },
            { "Assassination Classroom ch 01.cbz", new ParsedFilenameData { } },
            { "Ah My Goddess 1.cbz", new ParsedFilenameData { } },
            { "Ah My Goddess 10.cbz", new ParsedFilenameData { } },
            { "Claymore 001 - Silver-eyed Slayer[m-s].cbz", new ParsedFilenameData { } }, // : includes chapter name
            { "Claymore 002 - Claws in the Sky[m-s].cbz", new ParsedFilenameData { } }, // : includes chapter name
            { "[Tokuwotsumu] Tea Brown and Milk Tea [TZdY].cbz", new ParsedFilenameData { } },
            { "(Isoya Yuki) The Day the Cherryfruit Ripens (Hirari 14) [project].cbz", new ParsedFilenameData { } },
            { "[Garun] I Could Just Tell.cbz", new ParsedFilenameData { } },
            { "[Takemiya Jin] Yaezakura Sympathy 1 [TZdY].cbz", new ParsedFilenameData { } },
            { "04.cbz", new ParsedFilenameData { } },
            { "05 - Let's Be Careful With Summer.cbz", new ParsedFilenameData { } }, // : assume chapter number and name
            { "2000 AD 0001.cbz", new ParsedFilenameData { } },
            { "2000 AD 0345 (Cclay).cbz", new ParsedFilenameData { } },
            { "Ascender 001 (2019) (Digital) (Zone-Empire).cbz", new ParsedFilenameData { } },
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
