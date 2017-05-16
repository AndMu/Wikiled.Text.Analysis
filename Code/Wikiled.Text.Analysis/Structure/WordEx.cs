using System.ComponentModel;
using System.Xml.Serialization;
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

        [XmlElement]
        public string Phrase { get; set; }

        [XmlElement]
        [DefaultValue(0)]
        public double CalculatedValue { get; set; }

        [XmlElement]
        [DefaultValue(NamedEntities.None)]
        public NamedEntities EntityType { get; set; }

        [XmlIgnore]
        public int Id { get; set; }

        [XmlElement]
        [DefaultValue(false)]
        public bool IsAspect { get; set; }

        [XmlElement]
        [DefaultValue(false)]
        public bool IsStop { get; set; }

        [XmlElement]
        public string ItemText
        {
            get => UnderlyingWord.Text;
            set => UnderlyingWord = new SimpleWord(value);
        }

        [XmlElement]
        public string NormalizedEntity { get; set; }
        

        [XmlElement]
        public string Text { get; set; }

        [XmlElement]
        [DefaultValue(0)]
        public double Theta { get; set; }

        [XmlElement]
        public string Type
        {
            get => Tag.Tag;
            set => tag = POSTags.Instance.FindType(value);
        }

        public BasePOSType Tag => tag ?? POSTags.Instance.UnknownWord;

        [XmlIgnore]
        public IItem UnderlyingWord { get; private set; }

        [XmlElement]
        [DefaultValue(0)]
        public double Value { get; set; }

        public override string ToString()
        {
            return $"{Text}:{Tag.Tag}";
        }
    }
}