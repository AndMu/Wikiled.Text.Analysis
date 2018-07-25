using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Wikiled.Text.Analysis.Structure
{
    public class Document
    {
        public Document()
        {
            Sentences = new List<SentenceItem>();
            Text = String.Empty;
        }

        public Document(string text)
            : this()
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(text));
            }

            Text = text;
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement]
        public string Author { get; set; }

        [XmlElement(IsNullable = true)]
        public DateTime? DocumentTime { get; set; }

        [XmlElement]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        [XmlAttribute]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [XmlElement]
        public string Text { get; set; }

        [XmlElement(IsNullable = true)]
        public double? Stars { get; set; }

        [XmlArray]
        [XmlArrayItem("Sentence")]
        public List<SentenceItem> Sentences { get; set; }

        [JsonIgnore]
        [XmlIgnore]
        public int TotalWords
        {
            get { return Sentences.Sum(item => item.Words.Count); }
        }

        [JsonIgnore]
        [XmlIgnore]
        public IEnumerable<WordEx> Words
        {
            get { return Sentences.SelectMany(sentenceItem => sentenceItem.Words); }
        }

        public void Add(SentenceItem sentence)
        {
            if (sentence == null)
            {
                throw new ArgumentNullException(nameof(sentence));
            }

            sentence.Index = Sentences.Count;
            Sentences.Add(sentence);
            if (!string.IsNullOrEmpty(Text))
            {
                Text += " ";
            }

            Text += sentence.Text;
            Text = Text.Trim();
        }
    }
}