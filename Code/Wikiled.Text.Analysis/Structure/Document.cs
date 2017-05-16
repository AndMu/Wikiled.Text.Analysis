using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Wikiled.Core.Utility.Arguments;

namespace Wikiled.Text.Analysis.Structure
{
    public class Document
    {
        public Document()
        {
            Sentences = new List<SentenceItem>();
        }

        public Document(string text)
            : this()
        {
            Guard.NotNullOrEmpty(() => text, text);
            Text = text;
        }

        [XmlElement]
        public string Author { get; set; }

        [XmlElement(IsNullable = true)]
        public DateTime? DocumentTime { get; set; }

        [XmlElement]
        public string Title { get; set; }

        [XmlAttribute]
        public string Id { get; set; }

        [XmlElement]
        public string Text { get; set; }

        [XmlElement]
        public double? Stars { get; set; }
     
        [XmlArray]
        [XmlArrayItem("Sentence")]
        public List<SentenceItem> Sentences { get; set; }

        [XmlIgnore]
        public int TotalWords
        {
            get { return Sentences.Sum(item => item.Words.Count); }
        }

        [XmlIgnore]
        public IEnumerable<WordEx> Words
        {
            get { return Sentences.SelectMany(sentenceItem => sentenceItem.Words); }
        }

        public void Add(SentenceItem sentence)
        {
            Guard.NotNull(() => sentence, sentence);
            sentence.Index = Sentences.Count;
            Sentences.Add(sentence);
        }

        public void Init(Document document)
        {
            Stars = document.Stars;
            Author = document.Author;
            Id = document.Id;
        }
    }
}