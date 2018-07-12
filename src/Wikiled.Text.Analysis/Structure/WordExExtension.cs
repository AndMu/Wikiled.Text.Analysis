using System.Collections.Generic;
using Wikiled.Text.Analysis.POS;
using Wikiled.Text.Analysis.Words;

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

        public static bool IsQuestion(this WordEx word)
        {
            return WordTypeResolver.Instance.IsQuestion(word.Text);
        }

        public static bool IsConjunction(this WordEx word)
        {
            return word.Tag.WordType == WordType.Conjunction ||
                   WordTypeResolver.Instance.IsInvertingConjunction(word.Text) ||
                   WordTypeResolver.Instance.IsSpecialEndSymbol(word.Text) ||
                   WordTypeResolver.Instance.IsRegularConjunction(word.Text) ||
                   WordTypeResolver.Instance.IsSubordinateConjunction(word.Text);
        }
    }
}
