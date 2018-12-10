using Autofac;
using Microsoft.Extensions.Caching.Memory;
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
    public class DefaultNlpModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BasicEnglishDictionary>().As<IWordsDictionary>().SingleInstance();
            builder.RegisterType<NRCDictionary>()
                .As<INRCDictionary>()
                .SingleInstance()
                .OnActivating(item => item.Instance.Load());

            builder.RegisterType<SentenceTokenizerFactory>().As<ISentenceTokenizerFactory>().SingleInstance();
            builder.RegisterType<NaivePOSTagger>().As<IPOSTagger>().SingleInstance();
            builder.RegisterType<BNCList>().As<IPosTagResolver>().As<IWordFrequencyList>().SingleInstance();
            builder.Register(c => WordTypeResolver.Instance).As<IWordTypeResolver>().SingleInstance();

            builder.RegisterType<MessageCleanup>()
                .As<IMessageCleanup>()
                .SingleInstance()
                .OnActivating(
                    item =>
                    {
                        item.Instance.CleanCashTags = false;
                        item.Instance.LowerCase = false;
                    });

            builder.RegisterType<RawWordExtractor>().As<IRawTextExtractor>().SingleInstance();
            builder.Register(c => new MemoryCache(new MemoryCacheOptions())).As<IMemoryCache>().SingleInstance();
        }
    }
}
