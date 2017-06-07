namespace Wikiled.Text.Analysis.Extensions
{
    public static class TextExtensions
    {
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
