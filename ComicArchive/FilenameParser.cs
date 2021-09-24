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
                else if (TryVolumeParse(token, out var parsedVolume))
                {
                    result.Volume = parsedVolume;
                    index++;
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

        private static bool TryVolumeParse(string token, out int volume)
        {
            if (token.StartsWith("v", StringComparison.CurrentCultureIgnoreCase) && int.TryParse(token.Substring(1, token.Length - 1), out volume))
               return true;
            
            if (token.StartsWith("vol", StringComparison.CurrentCultureIgnoreCase) && int.TryParse(token.Substring(3, token.Length - 3), out volume))
                return true;

            volume = 0;
            return false;
        }
    }
}
