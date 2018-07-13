namespace Wikiled.Text.Analysis.Words
{
    public interface IWordTypeResolver
    {
        bool IsInvertor(string text);

        bool IsStop(string text);

        bool IsQuestion(string text);

        bool IsVerb(string text);

        bool IsAdverb(string text);

        bool IsNoun(string text);

        bool IsArticle(string text);

        bool IsAdjective(string text);

        bool IsPreposition(string text);

        bool IsPronoun(string text);

        bool IsCoordinatingConjunctions(string text);

        bool IsConjunctiveAdverbs(string text);

        bool IsSubordinateConjunction(string text);

        bool IsInvertingConjunction(string text);

        bool IsRegularConjunction(string text);

        bool IsConjunction(string word);

        bool IsSpecialEndSymbol(string word);
    }
}