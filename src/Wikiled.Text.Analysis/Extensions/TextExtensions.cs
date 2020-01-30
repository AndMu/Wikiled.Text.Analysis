using Wikiled.Common.Extensions;

namespace Wikiled.Text.Analysis.Extensions
{
    public static class TextExtensions
    {
        public static string GenerateKey(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return "NULL";
            }

            int total = text.Length < 10 ? text.Length : 10;
            var beggining = text.Substring(0, total).CreatePureLetterText();
            var ending = text.Substring(text.Length - total, total).CreatePureLetterText();
            var length = text.Length;
            return string.Format(
                "{0}{3}{1}{4}{2}",
                beggining,
                ending,
                length,
                "__End__",
                "__Len__");
        }

        public static bool IsVowel(this char letter)
        {
            switch (letter)
            {
                case 'a':
                case 'A':
                case 'e':
                case 'E':
                case 'i':
                case 'I':
                case 'o':
                case 'O':
                case 'u':
                case 'U':
                    return true;
                default:
                    return false;
            }
        }
    }
}
