using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;
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

        public int Index { get; set; }

        public string Text { get; set; }

        public List<WordEx> Words { get; set; }

        [JsonIgnore]
        public int this[WordEx word]
        {
            get
            {
                Guard.NotNull(() => word, word);
                return Words.IndexOf(word);
            }
        }

        public override string ToString()
        {
            return $"[{Words.Count}]: {Text}";
        }

        public void Add(string word)
        {
            Add(new WordEx(new SimpleWord(word)));
        }

        public void Add(WordEx word)
        {
            Words.Add(word);
        }

        public double CalculateSentiment()
        {
            return Words.Sum(x => x.CalculatedValue);
        }
    }
}
