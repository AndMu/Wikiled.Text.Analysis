using System;
using System.Linq;
using NUnit.Framework;
using Wikiled.Core.Utility.Extensions;
using Wikiled.Text.Analysis.Twitter;

namespace Wikiled.Text.Analysis.Tests.Twitter
{
    [TestFixture]
    public class EmojiSentimentTests
    {
        [Test]
        public void Generate()
        {
            var positive = EmojiSentiment.Positive.Distinct().Select(item => $"EMOTICON_{item.AsShortcode()}\t2").AccumulateItems(Environment.NewLine);
            var negative = EmojiSentiment.Negative.Distinct().Select(item => $"EMOTICON_{item.AsShortcode()}\t-2").AccumulateItems(Environment.NewLine);
            Assert.IsNotNull(positive);
            Assert.IsNotNull(negative);
        }
    }
}
