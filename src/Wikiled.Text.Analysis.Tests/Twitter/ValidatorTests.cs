using System.Text;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Wikiled.Text.Analysis.Twitter;

namespace Wikiled.Text.Analysis.Tests.Twitter
{
    [TestFixture]
    public class ValidatorTests
    {
        private Validator validator;

        [SetUp]
        public void Setup()
        {
            validator = new Validator();
        }

        [Test]
        public void BOMCharacterTest()
        {
            ClassicAssert.IsFalse(validator.IsValidTweet("test \uFFFE"));
            ClassicAssert.IsFalse(validator.IsValidTweet("test \uFEFF"));
        }

        [Test]
        public void InvalidCharacterTest()
        {
            ClassicAssert.IsFalse(validator.IsValidTweet("test \uFFFF"));
            ClassicAssert.IsFalse(validator.IsValidTweet("test \uFEFF"));
        }

        [Test]
        public void DirectionChangeCharactersTest()
        {
            ClassicAssert.IsFalse(validator.IsValidTweet("test \u202A test"));
            ClassicAssert.IsFalse(validator.IsValidTweet("test \u202B test"));
            ClassicAssert.IsFalse(validator.IsValidTweet("test \u202C test"));
            ClassicAssert.IsFalse(validator.IsValidTweet("test \u202D test"));
            ClassicAssert.IsFalse(validator.IsValidTweet("test \u202E test"));
        }

        [Test]
        public void AccentCharactersTest()
        {
            string c = "\u0065\u0301";
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < 139; i++)
            {
                builder.Append(c);
            }
            ClassicAssert.IsTrue(validator.IsValidTweet(builder.ToString()));
            ClassicAssert.IsTrue(validator.IsValidTweet(builder.Append(c).ToString()));
            ClassicAssert.IsFalse(validator.IsValidTweet(builder.Append(c).ToString()));
        }

        [Test]
        public void MutiByteCharactersTest()
        {
            string c = "\ud83d\ude02";
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < 139; i++)
            {
                builder.Append(c);
            }
            ClassicAssert.IsTrue(validator.IsValidTweet(builder.ToString()));
            ClassicAssert.IsTrue(validator.IsValidTweet(builder.Append(c).ToString()));
            ClassicAssert.IsFalse(validator.IsValidTweet(builder.Append(c).ToString()));
        }
    }
}