using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Wikiled.Text.Analysis.NLP.NRC
{
    [XmlRoot("NRC")]
    public class NRCRecord : ICloneable
    {
        public NRCRecord(string word)
        {
            Word = word;
        }

        [JsonIgnore]
        public string Word { get; set; }

        [JsonIgnore]
        public bool HasAnyValue => IsAnger ||
                                   IsAnticipation ||
                                   IsDisgust ||
                                   IsFear ||
                                   IsJoy ||
                                   IsNegative ||
                                   IsPositive ||
                                   IsSadness ||
                                   IsSurprise ||
                                   IsTrust;

        [XmlAttribute("Anger")]
        [DefaultValue(false)]
        [JsonProperty("Anger")]
        public bool IsAnger { get; set; }

        [XmlAttribute("Anticipation")]
        [DefaultValue(false)]
        [JsonProperty("Anticipation")]
        public bool IsAnticipation { get; set; }

        [XmlAttribute("Disgust")]
        [DefaultValue(false)]
        [JsonProperty("Disgust")]
        public bool IsDisgust { get; set; }

        [XmlAttribute("Fear")]
        [DefaultValue(false)]
        [JsonProperty("Fear")]
        public bool IsFear { get; set; }

        [XmlAttribute("Joy")]
        [DefaultValue(false)]
        public bool IsJoy { get; set; }

        [XmlAttribute("Negative")]
        [JsonProperty("Negative")]
        [DefaultValue(false)]
        public bool IsNegative { get; set; }

        [XmlAttribute("Positive")]
        [JsonProperty("Positive")]
        [DefaultValue(false)]
        public bool IsPositive { get; set; }

        [XmlAttribute("Sadness")]
        [JsonProperty("Sadness")]
        [DefaultValue(false)]
        public bool IsSadness { get; set; }

        [XmlAttribute("Surprise")]
        [JsonProperty("Surprise")]
        [DefaultValue(false)]
        public bool IsSurprise { get; set; }

        [XmlAttribute("Trust")]
        [JsonProperty("Trust")]
        [DefaultValue(false)]
        public bool IsTrust { get; set; }

        public object Clone()
        {
            NRCRecord record = new NRCRecord(Word);
            record.IsAnger = IsAnger;
            record.IsAnticipation = IsAnticipation;
            record.IsDisgust = IsDisgust;
            record.IsFear = IsFear;
            record.IsJoy = IsJoy;
            record.IsNegative = IsNegative;
            record.IsPositive = IsPositive;
            record.IsSadness = IsSadness;
            record.IsSurprise = IsSurprise;
            record.IsTrust = IsTrust;
            return record;
        }

        public IEnumerable<SentimentCategory> GetDefinedCategories()
        {
            return Enum.GetValues(typeof(SentimentCategory)).Cast<SentimentCategory>().Where(HasValue);
        }

        public bool HasValue(SentimentCategory category)
        {
            switch (category)
            {
                case SentimentCategory.Anger:
                    return IsAnger;
                case SentimentCategory.Anticipation:
                    return IsAnticipation;
                case SentimentCategory.Disgust:
                    return IsDisgust;
                case SentimentCategory.Fear:
                    return IsFear;
                case SentimentCategory.Joy:
                    return IsJoy;
                case SentimentCategory.Sadness:
                    return IsSadness;
                case SentimentCategory.Surprise:
                    return IsSurprise;
                case SentimentCategory.Trust:
                    return IsTrust;
                case SentimentCategory.None:
                    return !HasAnyValue;
                default:
                    throw new ArgumentOutOfRangeException("category");
            }
        }

        public void Invert()
        {
            if (IsJoy)
            {
                IsJoy = false;
                IsSadness = true;
            }
            else if (IsSadness)
            {
                IsJoy = true;
                IsSadness = false;
            }

            if (IsAnger)
            {
                IsAnger = false;
                IsFear = true;
            }
            else if (IsFear)
            {
                IsAnger = true;
                IsFear = false;
            }

            if (IsTrust)
            {
                IsTrust = false;
                IsDisgust = true;
            }
            else if (IsDisgust)
            {
                IsTrust = true;
                IsDisgust = false;
            }

            if (IsAnticipation)
            {
                IsAnticipation = false;
                IsSurprise = true;
            }
            else if (IsSurprise)
            {
                IsAnticipation = true;
                IsSurprise = false;
            }

            if (IsPositive)
            {
                IsPositive = false;
                IsNegative = true;
            }
            else if (IsNegative)
            {
                IsPositive = true;
                IsNegative = false;
            }
        }
    }
}
