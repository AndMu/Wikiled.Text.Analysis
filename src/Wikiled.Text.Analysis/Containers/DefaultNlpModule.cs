using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Wikiled.Text.Analysis.Dictionary;
using Wikiled.Text.Analysis.NLP;
using Wikiled.Text.Analysis.NLP.Frequency;
using Wikiled.Text.Analysis.NLP.NRC;
using Wikiled.Text.Analysis.POS;
using Wikiled.Text.Analysis.Tokenizer.Pipelined;
using Wikiled.Text.Analysis.Twitter;
using Wikiled.Text.Analysis.Words;

namespace Wikiled.Text.Analysis.Containers
{
    public static class DefaultNlpModule
    {
        public static IServiceCollection AddDefaultNlp(this IServiceCollection services)
        {
            services.AddSingleton<IWordsDictionary, BasicEnglishDictionary>();
            services.AddSingleton<INRCDictionary>(ctx =>
            {
                var dictionary = new NRCDictionary();
                dictionary.Load();
                return dictionary;
            });

            services.AddSingleton<ISentenceTokenizerFactory, SentenceTokenizerFactory>();
            services.AddSingleton<IPOSTagger, NaivePOSTagger>();
            services.AddSingleton<BNCList>();
            services.AddSingleton<IPosTagResolver>(ctx => ctx.GetService<BNCList>());
            services.AddSingleton<IWordFrequencyList>(ctx => ctx.GetService<BNCList>());

            services.AddSingleton(ctx => WordTypeResolver.Instance);

            services.AddSingleton<IMessageCleanup>(ctx =>
            {
                var item = new MessageCleanup();
                item.CleanCashTags = false;
                item.LowerCase = false;
                return item;
            });

            services.AddSingleton<IRawTextExtractor, RawWordExtractor>();
            return services;
        }
    }
}
