using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ComicArchive
{
    public static class FilenameParser
    {
        private const string volumeRegex = @"(v|vol)(\d+)";

        [CanBeNull]
        public static ParsedFilenameData Parse(string filename)
        {
            var result = new ParsedFilenameData();
            var tokens = TokeniseToWords(filename);
            string previousToken = null;

            var index = 0;
            while (index < tokens.Length)
            {

                // TODO: Implement examination of tokens to extract data
                var token = tokens[index];
                var trimmedToken = token?.Trim();

                if (trimmedToken.Equals("vol", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (index + 1 < tokens.Length)
                    {
                        var volume = tokens[index + 1];
                        if (int.TryParse(volume.Trim(), out var parsedVoume))
                        {
                            result.Volume = parsedVoume;
                            index++;
                        }
                    }
                }
                else if (Regex.Match(token, volumeRegex).Success)
                {
                    var volumeMatch = Regex.Match(token, volumeRegex);
                    var group = volumeMatch.Groups.Last();
                    var volume = group.Value;

                    if (int.TryParse(volume.Trim(), out var parsedVoume))
                    {
                        result.Volume = parsedVoume;
                        index++;
                    }
                }

                previousToken = token;
                index++;
            }

            return result;
        }

        /// <summary>
        /// Attemmpt to split the filename up into parsable tokens/words.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string[] TokeniseToWords(string filename)
        {
            var name = System.IO.Path.GetFileNameWithoutExtension(filename)
                .Replace("_", " ");

            // handle the case where filename uses '.' instead of spaces
            if (name.Contains(".") && !name.Contains(" "))
                return name.Split(".").Where(t => !string.IsNullOrWhiteSpace(t)).ToArray();

            // Note: '.' not considered to be a token break unless used in place of spaces, as covered above
            var tokens = new List<string>();
            bool inBrackets = false;

            var index = 0;
            while (index < name.Length)
            {
                var character = name[index];

                if (!inBrackets && character == ' ')
                {
                    index++;
                    continue;
                }

                var startPos = index;

                while ((inBrackets || character != ' ') && index < name.Length)
                {
                    index++;

                    if (index >= name.Length)
                        continue;

                    if (inBrackets && character == ']' || character == ')')
                        inBrackets = false;
                    else if (!inBrackets && character == '[' || character == '(')
                        inBrackets = true;

                    character = name[index];
                }

                tokens.Add(name.Substring(startPos, index - startPos));
            }

            return tokens.ToArray();
        }
    }
}
