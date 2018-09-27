using NUnit.Framework;
using Wikiled.Text.Analysis.Twitter;

namespace Wikiled.Text.Analysis.Tests.Twitter
{
    [TestFixture]
    public class MessageCleanupTests
    {
        private MessageCleanup instance;

        [SetUp]
        public void Setup()
        {
            instance = CreateMessageCleanup();
        }

        [TestCase(
            @"#CD #Musics Ariana Grande Sweet Like Candy 3.4 oz 100 ML Sealed In Box 100% Authenic New https://t.co/oFmp0bOvZy… https://t.co/WIHLch9KtK",
            "#cd #musics ariana grande sweet like candy 3.4 oz 100 ml sealed in box 100% authenic new URL_URL")]
        [TestCase(@"Hi http://www.wikiled.com trump", "hi URL_URL trump")]
        [TestCase(@"asd:/xx", "asd:/xx")]
        [TestCase(@"Hi @mister trump", "hi @mister trump")]
        [TestCase(@"Hi @mister!!! trump????", "hi @mister! trump?")]
        [TestCase(@"Hi :)", "hi EMOTICON_slightly_smiling_face")]
        [TestCase(@">:-( Hi :)", "EMOTICON_angry hi EMOTICON_slightly_smiling_face")]
        [TestCase(@"Hiiii suuuperrrr TRRRump", "hii suuperr trrump")]
        [TestCase(
            @"@realDonaldTrump @seanhannity WAR ZONE IN N.C.❗TRUMP - @rudygiulianiGOP & amp; @PaulBabeuAZ R THE BEST 4 PROTECTING FOLKS❗NEED ALL OF U IN DC NOW❗",
            "@realdonaldtrump @seanhannity war zone in n.c. EMOTICON_exclamation trump - @rudygiulianigop & amp; @paulbabeuaz r the best 4 protecting folks EMOTICON_exclamation need all of u in dc now EMOTICON_exclamation")]
        [TestCase(
            @"Did @realDonaldTrump just claim to speak for all black people? Even while applauding #StopandFrisk ? #STFUDonny #Debate2016",
            "did @realdonaldtrump just claim to speak for all black people? even while applauding #stopandfrisk ? #stfudonny #debate2016")]
        [TestCase(
            @"Donald Trump Sighting: Cleveland, Ohio/ New Spirit Revival Center https://t.co/a762lFPC6T @realDonaldTrump",
            "donald trump sighting: cleveland, ohio/ new spirit revival center URL_URL @realdonaldtrump")]
        [TestCase(@"up 💪🙏👊🏻⚖😍 donald j.trump for president 2016",
            "up EMOTICON_muscle EMOTICON_pray EMOTICON_facepunch EMOTICON_skin-tone-2 EMOTICON_scales EMOTICON_heart_eyes donald j.trump for president 2016")]
        [TestCase(@"⚖#⃣💪#⃣", "EMOTICON_scales EMOTICON_hash EMOTICON_muscle EMOTICON_hash")]
        [TestCase(@"#melaniatrump campaigning for anti-bullying", "#melaniatrump campaigning for anti-bullying")]
        [TestCase(
            @"���� Democracy inTRUMPtion ���� #trump #usa #notmypresident #urlo #munch @ Via Roma Cuneo https://t.co/DJ1K1TQnt4",
            "� democracy intrumption � #trump #usa #notmypresident #urlo #munch @ via roma cuneo URL_URL")]
        [TestCase(@"ariela:(singing", "ariela:(singing")]
        [TestCase(@"how graphic ahs is :)", "how graphic ahs is EMOTICON_slightly_smiling_face")]
        public void Cleanup(string message, string expected)
        {
            var result = instance.Cleanup(message);
            Assert.AreEqual(expected, result);
        }

        [TestCase(@"https://t.co/a762lFPC6T $AAPL good", @"https:/t.co/a762lFPC6T $AAPL good", false, false, false)]
        [TestCase(@"https://t.co/a762lFPC6T $AAPL good", @"https:/t.co/a762lfpc6t $aapl good", true, false, false)]
        [TestCase(@"https://t.co/a762lFPC6T $AAPL good", @"https:/t.co/a762lfpc6t INDEX_INDEX good", true, true, false)]
        [TestCase(@"https://t.co/a762lFPC6T $AAPL good", @"URL_URL INDEX_INDEX good", true, true, true)]
        public void TestSettings(string message, string expected, bool lower, bool cash, bool url)
        {
            instance.LowerCase = lower;
            instance.CleanCashTags = cash;
            instance.CleanUrl = url;
            var result = instance.Cleanup(message);
            Assert.AreEqual(expected, result);
        }

        private MessageCleanup CreateMessageCleanup()
        {
            return new MessageCleanup();
        }
    }
}
