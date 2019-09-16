using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Wikiled.Common.Utilities.Modules;
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
    public class DefaultNlpModule : IModule
    {
        public IServiceCollection ConfigureServices(IServiceCollection services)
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

            services.AddSingleton<IWordTypeResolver>(ctx => WordTypeResolver.Instance);

            services.AddSingleton<IMessageCleanup>(ctx =>
            {
                var item = new MessageCleanup();
                item.CleanCashTags = false;
                item.LowerCase = false;
                return item;
            });

            services.AddSingleton<IRawTextExtractor, RawWordExtractor>();
            services.AddSingleton<IMemoryCache>(ctx => new MemoryCache(new MemoryCacheOptions()));
            return services;
        }
    }
}
