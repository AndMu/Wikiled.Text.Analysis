﻿using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Wikiled.MachineLearning.Mathematics;

namespace Wikiled.Text.Analysis.Structure
{
    [XmlRoot("Sentence")]
    public class SentenceItem : ICloneable
    {
        public SentenceItem()
        {
            Words = new List<WordEx>();
        }

        public SentenceItem(string text)
            : this()
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(text));
            }

            Text = text;
        }

        [XmlElement]
        public int Index { get; set; }

        [XmlElement]
        public string Text { get; set; }

        [XmlArray]
        [XmlArrayItem("Word")]
        public List<WordEx> Words { get; set; }

        [JsonIgnore]
        [XmlIgnore]
        public int this[WordEx word]
        {
            get
            {
                if (word is null)
                {
                    throw new ArgumentNullException(nameof(word));
                }

                return Words.IndexOf(word);
            }
        }

        public override string ToString()
        {
            return $"[{Words.Count}]: {Text}";
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public void Add(string word)
        {
            Add(new WordEx(new SimpleWord(word)));
        }

        public void Add(WordEx word)
        {
            Words.Add(word);
        }

        public RatingData CalculateSentiment()
        {
            RatingData data = new RatingData();
            foreach (var wordEx in Words)
            {
                if (wordEx.CalculatedValue.HasValue)
                {
                    data.AddSetiment(wordEx.CalculatedValue.Value);
                }
                else if (wordEx.Value.HasValue)
                {
                    data.AddSetiment(wordEx.Value.Value);
                }
            }

            return data;
        }
    }
}
