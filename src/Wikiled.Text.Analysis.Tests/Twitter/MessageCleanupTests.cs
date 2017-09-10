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

        [TestCase(@"#CD #Musics Ariana Grande Sweet Like Candy 3.4 oz 100 ML Sealed In Box 100% Authenic New https://t.co/oFmp0bOvZy… https://t.co/WIHLch9KtK", "#cd #musics ariana grande sweet like candy 3.4 oz 100 ml sealed in box 100% authenic new URL_URL")]
        [TestCase(@"Hi http://www.wikiled.com trump", "hi URL_URL trump")]
        [TestCase(@"Hi @mister trump", "hi @mister trump")]
        [TestCase(@"Hi @mister!!! trump????", "hi @mister! trump?")]
        [TestCase(@"Hi :)", "hi EMOTICON_slightly_smiling_face")]
        [TestCase(@">:-( Hi :)", "EMOTICON_angry hi EMOTICON_slightly_smiling_face")]
        [TestCase(@"Hiiii suuuperrrr TRRRump", "hii suuperr trrump")]
        [TestCase(@"@realDonaldTrump @seanhannity WAR ZONE IN N.C.❗TRUMP - @rudygiulianiGOP & amp; @PaulBabeuAZ R THE BEST 4 PROTECTING FOLKS❗NEED ALL OF U IN DC NOW❗",
                    "@realdonaldtrump @seanhannity war zone in n.c. EMOTICON_exclamation trump - @rudygiulianigop & amp; @paulbabeuaz r the best 4 protecting folks EMOTICON_exclamation need all of u in dc now EMOTICON_exclamation")]
        [TestCase(@"Did @realDonaldTrump just claim to speak for all black people? Even while applauding #StopandFrisk ? #STFUDonny #Debate2016",
                    "did @realdonaldtrump just claim to speak for all black people? even while applauding #stopandfrisk ? #stfudonny #debate2016")]
        [TestCase(@"Donald Trump Sighting: Cleveland, Ohio/ New Spirit Revival Center https://t.co/a762lFPC6T @realDonaldTrump", "donald trump sighting: cleveland, ohio/ new spirit revival center URL_URL @realdonaldtrump")]
        [TestCase(@"up 💪🙏👊🏻⚖😍 donald j.trump for president 2016", "up EMOTICON_muscle EMOTICON_pray EMOTICON_facepunch EMOTICON_skin-tone-2 EMOTICON_scales EMOTICON_heart_eyes donald j.trump for president 2016")]
        [TestCase(@"⚖#⃣💪#⃣", "EMOTICON_scales EMOTICON_hash EMOTICON_muscle EMOTICON_hash")]
        [TestCase(@"#melaniatrump campaigning for anti-bullying", "#melaniatrump campaigning for anti-bullying")]
        [TestCase(@"���� Democracy inTRUMPtion ���� #trump #usa #notmypresident #urlo #munch @ Via Roma Cuneo https://t.co/DJ1K1TQnt4", "� democracy intrumption � #trump #usa #notmypresident #urlo #munch @ via roma cuneo URL_URL")]
        [TestCase(@"ariela:(singing", "ariela EMOTICON_disappointed singing")]
        [TestCase(@"how graphic ahs is :)", "how graphic ahs is MOTICON_slightly_smiling_face")]
        public void Cleanup(string message, string expected)
        {
            var result = instance.Cleanup(message);
            Assert.AreEqual(expected, result);
        }

        private MessageCleanup CreateMessageCleanup()
        {
            return new MessageCleanup();
        }
    }
}
