using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Wikiled.Text.Analysis.Structure
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum NamedEntities
    {
        None,
        Custom,
        Person,
        Location,
        Organization,
        Misc,
        Money,
        Time,
        Date,
        Percent,
        Number,
        Ordinal,
        Duration,
        Set,
        Hashtag
    }
}
