using System.Collections.Generic;

namespace Wikiled.Text.Analysis.Structure
{
    public static class WordExExtension
    {
        public static IEnumerable<string> GetPossibleText(this WordEx word)
        {
            yield return word.Text;
            
            if (!string.IsNullOrEmpty(word.Raw) &&
                word.Raw != word.Text)
            {
                yield return word.Raw;
            }

            if (word.EntityType == NamedEntities.Hashtag &&
                word.Text.Length > 1)
            {
                yield return word.Text.Substring(1);
            }
        }
    }
}
