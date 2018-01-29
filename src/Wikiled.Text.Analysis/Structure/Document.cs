using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
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

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Author { get; set; }

        public DateTime? DocumentTime { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        public string Text { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? Stars { get; set; }
     
        public List<SentenceItem> Sentences { get; set; }

        [JsonIgnore]
        public int TotalWords
        {
            get { return Sentences.Sum(item => item.Words.Count); }
        }

        [JsonIgnore]
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