using System;
using Wikiled.Text.Analysis.Structure;

namespace Wikiled.Text.Analysis.NLP
{
    public class NGramBlock
    {
        private string posMask;

        private string wordMask;

        public NGramBlock(WordEx[] wordOccurrences)
        {
            WordOccurrences = wordOccurrences ?? throw new ArgumentNullException(nameof(wordOccurrences));
        }

        private void PopuplateMasks()
        {
            if (!string.IsNullOrEmpty(posMask))
            {
                return;
            }

            for (int i = 0; i < WordOccurrences.Length; i++)
            {
                if (i > 0)
                {
                    posMask += " ";
                    wordMask += " ";
                }

                posMask += WordOccurrences[i].Tag.Tag;
                wordMask += WordOccurrences[i].Text;
            }
        }

        public WordEx[] WordOccurrences { get; set; }

        public string PosMask
        {
            get
            {
                PopuplateMasks();
                return posMask;
            }
            set => posMask = value;
        }

        public string WordMask
        {
            get
            {
                PopuplateMasks();
                return wordMask;
            }
            set => wordMask = value;
        }
    }
}