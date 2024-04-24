using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Seedworks.Lib
{
    [Serializable]
    public enum StringEnsureOptions
    {
        None = 0,
        IgnoreNullOrEmpty = 1
    }

    public static class StringExtension
    {
        /// <summary>
        /// Return a list of wrapped lines
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maxLineLength"></param>
        /// <returns></returns>
        public static List<string> WordWrap(this string text, int maxLineLength)
        {
            if (String.IsNullOrEmpty(text))
                return new List<string>();

            text = text.Replace(Environment.NewLine, "\n");

            var lines = text.Split('\n').ToList();

            bool lengthsTrimmed = false;
            while (!lengthsTrimmed)
            {
                var longLines = lines.Where(line => line.Length > maxLineLength).ToList();
                if (longLines.Count() == 0) lengthsTrimmed = true;
                foreach (var longLine in longLines)
                {
                    // get the largest index in the line for which the char is a space
                    // and the index is smaller than the max line length
                    var length = longLine
                        .Select((chr, i) => (chr == ' ' && i <= maxLineLength) ? i : -1).Max();

                    // If no space found, force wrap in the middle of a word.
                    if (length == -1)
                        length = maxLineLength;

                    var newLine1 = longLine.Substring(0, length);
                    var newLine2 = longLine.Substring(length);

                    // RemoveAsync space at beginning of wrapped line. Looks better.
                    if (newLine2[0] == ' ')
                        newLine2 = newLine2.Substring(1);

                    lines.InsertRange(lines.IndexOf(longLine),
                        new[] { newLine1, newLine2 });
                    lines.Remove(longLine);
                }
            }

            return lines;
        }

        /// <summary>
        /// Returns the given text with lines separated by <see cref="Environment.NewLine"/>
        /// and a maximum length of <paramref name="maxLineLength"/>.
        /// Uses <see cref="WordWrap"/> internally.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maxLineLength"></param>
        /// <returns></returns>
        public static string Wrap(this string text, int maxLineLength)
        {
            var lines = text.WordWrap(maxLineLength);

            var output = new StringBuilder();

            for (int i = 0; i < lines.Count; i++)
            {
                output.Append(lines[i]);
                if (i < lines.Count - 1)
                    output.AppendLine();
            }

            return output.ToString();
        }

        public static bool IsNumeric(this string value)
        {
            var regex = new Regex(@"^\-?[0-9]+$");
            return regex.IsMatch(value);
        }

        public static int ToInt(this string value)
        {
            return Int32.Parse(value);
        }

        public static int ToInt32(this string value)
        {
            return Int32.Parse(value);
        }

        /// <summary>
        /// Returns a new string which is guaranteed to end with the given suffix.
        /// If the suffix is already present, the same string is returned.
        /// </summary>
        public static string EnsureEndsWith(this string value, string suffix)
        {
            return EnsureEndsWith(value, suffix, StringEnsureOptions.None);
        }

        public static string EnsureEndsWith(
            this string value,
            string suffix,
            StringEnsureOptions options)
        {
            if (options == StringEnsureOptions.IgnoreNullOrEmpty && string.IsNullOrEmpty(value))
                return value;

            if (value.EndsWith(suffix))
                return value;

            return value + suffix;
        }
    }
}