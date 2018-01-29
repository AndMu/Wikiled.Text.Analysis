using System.ComponentModel;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Wikiled.Text.Analysis.POS;
using Wikiled.Text.Analysis.POS.Tags;

namespace Wikiled.Text.Analysis.Structure
{
    [XmlRoot("Word")]
    public class WordEx
    {
        private BasePOSType tag;

        public WordEx()
        {
        }

        public WordEx(IItem item)
        {
            UnderlyingWord = item;
            Text = item.Text;
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Phrase { get; set; }

        [DefaultValue(5)]
        public double CalculatedValue { get; set; }

        [DefaultValue(NamedEntities.None)]
        public NamedEntities EntityType { get; set; }

        [JsonIgnore]
        public int Id { get; set; }
        
        [DefaultValue(false)]
        public bool IsAspect { get; set; }

        [DefaultValue(false)]
        public bool IsStop { get; set; }

        public string ItemText
        {
            get => UnderlyingWord.Text;
            set => UnderlyingWord = new SimpleWord(value);
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string NormalizedEntity { get; set; }
        
        public string Text { get; set; }

        [DefaultValue(0)]
        public double Theta { get; set; }

        public string Type
        {
            get => Tag.Tag;
            set => tag = POSTags.Instance.FindType(value);
        }

        [JsonIgnore]
        public BasePOSType Tag => tag ?? POSTags.Instance.UnknownWord;

        [JsonIgnore]
        public IItem UnderlyingWord { get; private set; }
        
        [DefaultValue(0)]
        public double Value { get; set; }

        public override string ToString()
        {
            return $"{Text}:{Tag.Tag}";
        }
    }
}