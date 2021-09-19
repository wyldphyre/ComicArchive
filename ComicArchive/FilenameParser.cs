using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComicArchive
{
    public static class FilenameParser
    {
        [CanBeNull]
        public static ParsedFilenameData Parse(string filename)
        {
            var result = new ParsedFilenameData();
            var tokens = TokeniseToWords(filename);

            var index = 0;
            while (index < tokens.Length)
            {
                index++;

                // TODO: Implement examination of tokens to extract data
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
