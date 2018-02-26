using System.Globalization;
using PluralizationService;
using PluralizationService.English;

namespace Wikiled.Text.Analysis.NLP
{
    public class PluralizationServiceInstance
    {
        private static readonly IPluralizationApi api;

        private static readonly CultureInfo cultureInfo;

        static PluralizationServiceInstance()
        {
            var builder = new PluralizationApiBuilder();
            builder.AddEnglishProvider();

            api = builder.Build();
            cultureInfo = new CultureInfo("en-US");
        }

        public bool IsPlural(string text)
        {
            return api.IsPlural(text, cultureInfo);
        }

        public string Pluralize(string text)
        {
            return api.Pluralize(text, cultureInfo) ?? text;
        }

        public string Singularize(string text)
        {
            return api.Singularize(text, cultureInfo) ?? text;
        }
    }
}
