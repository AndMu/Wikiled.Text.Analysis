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

        [TestCase(@"Hi http://www.wikiled.com trump", "hi :URL: trump")]
        [TestCase(@"Hi @mister trump", "hi @mister trump")]
        [TestCase(@"Hi @mister!!! trump????", "hi @mister! trump?")]
        [TestCase(@"Hiiii suuuperrrr TRRRump", "hii suuperr trrump")]
        [TestCase(@"@realDonaldTrump @seanhannity WAR ZONE IN N.C.❗TRUMP - @rudygiulianiGOP & amp; @PaulBabeuAZ R THE BEST 4 PROTECTING FOLKS❗NEED ALL OF U IN DC NOW❗",
                    "@realdonaldtrump @seanhannity war zone in n.c. :exclamation: trump - @rudygiulianigop & amp; @paulbabeuaz r the best 4 protecting folks :exclamation: need all of u in dc now :exclamation:")]
        [TestCase(@"Did @realDonaldTrump just claim to speak for all black people? Even while applauding #StopandFrisk ? #STFUDonny #Debate2016",
                    "did @realdonaldtrump just claim to speak for all black people? even while applauding #stopandfrisk ? #stfudonny #debate2016")]
        [TestCase(@"Donald Trump Sighting: Cleveland, Ohio/ New Spirit Revival Center https://t.co/a762lFPC6T @realDonaldTrump", "donald trump sighting: cleveland, ohio/ new spirit revival center :URL: @realdonaldtrump")]
        [TestCase(@"up 💪🙏👊🏻⚖😍 donald j.trump for president 2016", "up :muscle: :pray: :facepunch: :skin-tone-2: :scales: :heart_eyes: donald j.trump for president 2016")]
        [TestCase(@"⚖#⃣💪#⃣", ":scales: :hash: :muscle: :hash:")]
        [TestCase(@"#melaniatrump campaigning for anti-bullying", "#melaniatrump campaigning for anti-bullying")]
        [TestCase(@"���� Democracy inTRUMPtion ���� #trump #usa #notmypresident #urlo #munch @ Via Roma Cuneo https://t.co/DJ1K1TQnt4", "� democracy intrumption � #trump #usa #notmypresident #urlo #munch @ via roma cuneo :URL:")]
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
