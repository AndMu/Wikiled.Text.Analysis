using System;
using System.Linq;
using NUnit.Framework;
using Wikiled.Common.Extensions;
using Wikiled.Text.Analysis.Emojis;

namespace Wikiled.Text.Analysis.Tests.Emojis
{
    [TestFixture]
    public class EmojiSentimentTests
    {
        [Test]
        public void Generate()
        {
            var result = Emoji.CHART_WITH_DOWNWARDS_TREND;
            var positive = EmojiSentiment.Positive.Distinct().Select(item => $"EMOTICON_{item.AsShortcode()}\t2").AccumulateItems(Environment.NewLine);
            var negative = EmojiSentiment.Negative.Distinct().Select(item => $"EMOTICON_{item.AsShortcode()}\t-2").AccumulateItems(Environment.NewLine);
            Assert.IsNotNull(positive);
            Assert.IsNotNull(negative);
        }
    }
}
