using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Wikiled.Text.Analysis.Emojis
{
    public class EmojyCleanup
    {
        private readonly Dictionary<string, Emoji> complexEmojis;

        private readonly Dictionary<string, Emoji> textEmojis;

        private readonly Dictionary<uint, Emoji> emojis;

        private readonly int maxTextEmoji;

        private readonly int minTextEmoji;

        public EmojyCleanup()
        {
            textEmojis = new Dictionary<string, Emoji>(StringComparer.OrdinalIgnoreCase);
            foreach (var emoji in Emoji.All.Values)
            {
                foreach (var text in emoji.Texts)
                {
                    textEmojis[text] = emoji;
                }
            }

            maxTextEmoji = textEmojis.Keys.Max(item => item.Length);
            minTextEmoji = textEmojis.Keys.Min(item => item.Length);
            emojis = (from item in Emoji.All.Values
                      where !item.Unified.Contains('-')
                      select item).ToDictionary(item => uint.Parse(item.Unified, NumberStyles.AllowHexSpecifier), emoji => emoji);

            complexEmojis = (from item in Emoji.All.Values
                             select item).ToDictionary(item => item.Unified, emoji => emoji);
        }

        public bool Remove { get; set; }

        public bool NormalizeText { get; set; } = true;

        public EmojyExtractResult Extract(string text)
        {
            var result = new EmojyExtractResult(text);
            var builder = new StringBuilder();

            for (int i = 0; i < text.Length; i++)
            {
                var letter = text[i];

                if (letter == '…' ||
                    letter == '\r')
                {
                    continue;
                }

                if (letter == '\n' ||
                    letter == '\b')
                {
                    letter = ' ';
                }

                var emoji = GetEmoji(text, ref i);

                if (emoji != null)
                {
                    result.AddEmoji(emoji);
                    ReplaceEmoji(builder, emoji, text, i);

                    continue;
                }

                emoji = GetTextEmoji(text, ref i);

                if (emoji != null)
                {
                    ReplaceEmoji(builder, emoji, text, i);

                    continue;
                }

                if (NormalizeText)
                {
                    char? previous = builder.Length > 0 ? builder[builder.Length - 1] : (char?) null;
                    char? previousToPrevious = builder.Length > 1 ? builder[builder.Length - 2] : (char?) null;

                    if (letter == previous)
                    {
                        // not allow text with more than two repeating letter
                        // and not allow more than one symbol in sequence
                        if (!char.IsLetterOrDigit(letter) ||
                            (char.IsLetterOrDigit(letter) && letter == previousToPrevious))
                        {
                            continue;
                        }
                    }
                }

                builder.Append(letter);
            }

            text = builder.ToString().Trim();

            result.Cleaned =
                NormalizeText
                    ? Regex.Replace(text,
                                    @"\b(\w+)\s+\1\b",
                                    "$1",
                                    RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    : text;

            return result;
        }

        private void ReplaceEmoji(StringBuilder builder, Emoji emoji, string text, int index)
        {
            if (Remove)
            {
                return;
            }

            if (builder.Length > 0 &&
                builder[builder.Length - 1] != ' ')
            {
                builder.Append(' ');
            }

            builder.Append(emoji.AsShortcode());

            if (index < (text.Length - 1) &&
                text[index + 1] != ' ')
            {
                builder.Append(' ');
            }
        }

        private Emoji GetTextEmoji(string text, ref int index)
        {
            // if this is not the start or there is no space before
            if (index > 0 &&
                text[index - 1] != ' ')
            {
                return null;
            }

            int left = text.Length - index;
            if (left < minTextEmoji)
            {
                return null;
            }

            left = left < maxTextEmoji ? left : maxTextEmoji;

            for (int i = left; i >= minTextEmoji; i--)
            {
                var block = text.Substring(index, i);
                if (textEmojis.TryGetValue(block, out Emoji emoji))
                {
                    index += i;
                    if (index >= text.Length ||
                        text[index] != ' ')
                    {
                        index -= 1;
                    }

                    return emoji;
                }
            }

            return null;
        }

        private Emoji GetEmoji(string text, ref int index)
        {
            Emoji emoji;
            if (index < text.Length - 1)
            {
                try
                {
                    int unicodeCodePoint = char.ConvertToUtf32(text, index);
                    if (unicodeCodePoint > 0xffff)
                    {
                        var result = emojis.TryGetValue((uint)unicodeCodePoint, out emoji) ? emoji : null;
                        index++;
                        return result;
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }

            var letter = text[index];
            var map = $"{Convert.ToUInt16(letter):X4}";
            if (complexEmojis.TryGetValue(map, out emoji))
            {
                return emoji;
            }

            if (index < text.Length - 1)
            {
                map += $"-{Convert.ToUInt16(text[index + 1]):X4}";
                if (complexEmojis.TryGetValue(map, out emoji))
                {
                    index++;
                    return emoji;
                }
            }

            return null;
        }
    }
}
