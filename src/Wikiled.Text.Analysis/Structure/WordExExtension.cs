using System;
using System.Collections.Generic;
using Wikiled.Text.Analysis.POS;
using Wikiled.Text.Analysis.Words;

namespace Wikiled.Text.Analysis.Structure
{
    public static class WordExExtension
    {
        public static T GetPossibleVariation<T>(this WordEx word, Func<string, T> getter, Func<T, T> invert = null)
            where T : class
        {
            if (word == null)
            {
                throw new ArgumentNullException(nameof(word));
            }

            if (getter == null)
            {
                throw new ArgumentNullException(nameof(getter));
            }

            T record = default;
            foreach (var text in word.GetPossibleText())
            {
                record = getter(text);
                if (record != default)
                {
                    break;
                }
            }

            if (record == default)
            {
                return null;
            }

            if (word.IsInverted &&
                invert != null)
            {
                record = invert(record);
            }

            return record;
        }

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
            return word.POSType.WordType == WordType.Conjunction ||
                   WordTypeResolver.Instance.IsInvertingConjunction(word.Text) ||
                   WordTypeResolver.Instance.IsSpecialEndSymbol(word.Text) ||
                   WordTypeResolver.Instance.IsRegularConjunction(word.Text) ||
                   WordTypeResolver.Instance.IsSubordinateConjunction(word.Text);
        }
    }
}
