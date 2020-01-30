using System.Linq;
using NUnit.Framework;
using Wikiled.Text.Analysis.Emojis;

namespace Wikiled.Text.Analysis.Tests.Emojis
{
    [TestFixture]
    public class EmojyCleanupTests
    {
        [TestCase("up 💪🙏👊🏻⚖😍 donald j.trump for president 2016", true, true, 6, "up donald j.trump for president 2016")]
        [TestCase("up 💪💪🙏👊🏻⚖😍 donald j.trump for president president 2016...", false, true, 7, "up EMOTICON_muscle EMOTICON_pray EMOTICON_facepunch EMOTICON_skin-tone-2 EMOTICON_scales EMOTICON_heart_eyes donald j.trump for president 2016.")]
        [TestCase("up 💪💪🙏👊🏻⚖😍 donald j.trump for president president 2016...", false, false, 7, "up EMOTICON_muscle EMOTICON_muscle EMOTICON_pray EMOTICON_facepunch EMOTICON_skin-tone-2 EMOTICON_scales EMOTICON_heart_eyes donald j.trump for president president 2016...")]
        public void Extract(string text, bool clean, bool normalize, int total, string result)
        {
            var instance = new EmojyCleanup();
            instance.Remove = clean;
            instance.NormalizeText = normalize;
            var extract = instance.Extract(text);
            Assert.AreEqual(total, extract.Emojis.Count());
            Assert.AreEqual(result, extract.Cleaned);
        }
    }
}
