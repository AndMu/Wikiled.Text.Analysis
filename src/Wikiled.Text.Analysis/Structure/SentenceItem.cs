using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Wikiled.Core.Utility.Arguments;

namespace Wikiled.Text.Analysis.Structure
{
    [XmlRoot("Sentence")]
    public class SentenceItem
    {
        public SentenceItem()
        {
            Words = new List<WordEx>();
        }

        public SentenceItem(string text)
            : this()
        {
            Guard.NotNullOrEmpty(() => text, text);
            Text = text;
        }

        public void Add(string word)
        {
            Add(new WordEx(new SimpleWord(word)));
        }

        public void Add(WordEx word)
        {
            Words.Add(word);
        }

        [XmlIgnore]
        public int this[WordEx word]
        {
            get
            {
                Guard.NotNull(() => word, word);
                return Words.IndexOf(word);
            }
        }

        [XmlElement]
        public int Index { get; set; }

        [XmlElement]
        public string Text { get; set; }

        [XmlArray]
        [XmlArrayItem("Word")]
        public List<WordEx> Words { get; set; }

        public double CalculateSentiment()
        {
            return Words.Sum(x => x.CalculatedValue);
        }

        public override string ToString()
        {
            return $"[{Words.Count}]: {Text}";
        }
    }
}
