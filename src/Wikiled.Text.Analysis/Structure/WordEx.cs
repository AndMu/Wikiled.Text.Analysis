﻿using System;
using System.ComponentModel;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Wikiled.Text.Analysis.NLP.NRC;
using Wikiled.Text.Analysis.POS;
using Wikiled.Text.Analysis.POS.Tags;
using Wikiled.Text.Analysis.Structure.Light;

namespace Wikiled.Text.Analysis.Structure
{
    [XmlRoot("Word")]
    public class WordEx : ICloneable
    {
        private BasePOSType tag;

        public WordEx()
        {
        }

        public WordEx(string text)
        : this(new SimpleWord(text))
        {
        }

        public WordEx(IItem item)
        {
            UnderlyingWord = item;
            Text = item.Text;
            Span = Text;
        }

        public WordEx(LightWord item)
        {
            UnderlyingWord = item;
            Text = item.Text;
            POS = item.Tag;
            Phrase = item.Phrase;
            Span = Text;
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement]
        public string Span { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement]
        public string[] Attributes { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement]
        public SentimentCategory[] Emotions { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement]
        public string Phrase { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        [XmlElement]
        public double? CalculatedValue { get; set; }

        [XmlElement]
        [DefaultValue(NamedEntities.None)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public NamedEntities EntityType { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement]
        public string CustomEntity { get; set; }

        [JsonIgnore]
        [XmlIgnore]
        public int Id { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement]
        public bool IsAspect { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement]
        public bool IsInvertor { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement]
        public bool IsInverted { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement]
        public bool IsStop { get; set; }

        [XmlElement]
        public string ItemText
        {
            get => UnderlyingWord.Text;
            set => UnderlyingWord = new SimpleWord(value);
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement]
        public string NormalizedEntity { get; set; }

        [XmlElement]
        public string Text { get; set; }

        [XmlElement]
        public string Raw { get; set; }

        [XmlElement]
        public string POS
        {
            get => POSType.Tag;
            set => tag = POSTags.Instance.FindType(value);
        }

        [JsonIgnore]
        [XmlIgnore]
        public BasePOSType POSType => tag ?? POSTags.Instance.UnknownWord;

        [JsonIgnore]
        [XmlIgnore]
        public IItem UnderlyingWord { get; private set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        [XmlElement]
        public double? Value { get; set; }

        public override string ToString()
        {
            return $"{Text}:{POSType.Tag}";
        }

        public object Clone()
        {
            return (WordEx)MemberwiseClone();
        }
    }
}