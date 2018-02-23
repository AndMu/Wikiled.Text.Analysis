using Wikiled.Common.Arguments;
using Wikiled.Common.Extensions;
using Wikiled.Text.Analysis.POS.Tags;
using Wikiled.Text.Analysis.Words;

namespace Wikiled.Text.Analysis.POS
{
    public class NaivePOSTagger : IPOSTagger
    {
        private readonly IPosTagResolver frequentList;

        private readonly IWordTypeResolver wordType;

        public NaivePOSTagger(IPosTagResolver frequentList, IWordTypeResolver wordType)
        {
            this.frequentList = frequentList;
            this.wordType = wordType;
        }

        public BasePOSType GetTag(string word)
        {
            Guard.NotNullOrEmpty(() => word, word);
            BasePOSType wordPosType = POSTags.Instance.UnknownWord;
            if(!word.HasLetters() &&
                POSTags.Instance.Contains(word))
            {
                wordPosType = POSTags.Instance.FindType(word);
            }
            else if(wordType.IsArticle(word))
            {
                wordPosType = POSTags.Instance.RP;
            }
            else if(wordType.IsCoordinatingConjunctions(word))
            {
                wordPosType = POSTags.Instance.CC;
            }
            else if(wordType.IsPronoun(word))
            {
                wordPosType = POSTags.Instance.PRP;
            }
            else if(wordType.IsAdverb(word))
            {
                wordPosType = POSTags.Instance.RB;
            }
            else if(wordType.IsAdjective(word))
            {
                wordPosType = POSTags.Instance.JJ;
            }
            else if(wordType.IsVerb(word))
            {
                wordPosType = POSTags.Instance.VB;
            }
            else if(wordType.IsNoun(word))
            {
                wordPosType = POSTags.Instance.NN;
            }
            else if(!word.HasLetters())
            {
                wordPosType = POSTags.Instance.SYM;
            }
            else if(frequentList != null)
            {
                wordPosType = frequentList.GetPOS(word);
            }

            return wordPosType;
        }
    }
}
