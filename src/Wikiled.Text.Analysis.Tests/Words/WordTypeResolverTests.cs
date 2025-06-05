using NUnit.Framework;
using NUnit.Framework.Legacy;
using Wikiled.Text.Analysis.Words;

namespace Wikiled.Text.Analysis.Tests.Words
{
    [TestFixture]
    public class WordTypeResolverTests
    {
         private IWordTypeResolver instance;

        [OneTimeSetUp]
        public void Setup()
        {
            instance = WordTypeResolver.Instance;
        }

        [Test]
        public void IsSpecialEndSymbol()
        {
            string item = ".";
            ClassicAssert.IsFalse(instance.IsSpecialEndSymbol(item));
            item = "...";
            ClassicAssert.IsTrue(instance.IsSpecialEndSymbol(item));
            item = "!!";
            ClassicAssert.IsTrue(instance.IsSpecialEndSymbol(item));
            item = "??";
            ClassicAssert.IsTrue(instance.IsSpecialEndSymbol(item));
        }

        [Test]
        public void IsArticle()
        {
            bool value = instance.IsArticle("a");
            ClassicAssert.IsTrue(value);
            value = instance.IsArticle("there");
            ClassicAssert.IsFalse(value);
        }

        [Test]
        public void IsConjunction()
        {
            bool value = instance.IsConjunction("and");
            ClassicAssert.IsTrue(value);
            value = instance.IsConjunction("there");
            ClassicAssert.IsFalse(value);
        }

        [Test]
        public void IsConjunctiveAdverbs()
        {
            bool value = instance.IsConjunctiveAdverbs("incidentally");
            ClassicAssert.IsTrue(value);
            value = instance.IsConjunctiveAdverbs("there");
            ClassicAssert.IsFalse(value);
        }

        [Test]
        public void IsCoordinatingConjunctions()
        {
            bool value = instance.IsCoordinatingConjunctions("and");
            ClassicAssert.IsTrue(value);
            value = instance.IsCoordinatingConjunctions("there");
            ClassicAssert.IsFalse(value);
        }

        [Test]
        public void IsInvertingConjunction()
        {
            bool value = instance.IsInvertingConjunction("nevertheless");
            ClassicAssert.IsTrue(value);
            value = instance.IsInvertingConjunction("there");
            ClassicAssert.IsFalse(value);
        }

        [Test]
        public void IsPreposition()
        {
            bool value = instance.IsPreposition("against");
            ClassicAssert.IsTrue(value);
            value = instance.IsPreposition("there");
            ClassicAssert.IsFalse(value);
        }

        [Test]
        public void IsPronoun()
        {
            bool value = instance.IsPronoun("hers");
            ClassicAssert.IsTrue(value);
            value = instance.IsPronoun("there");
            ClassicAssert.IsFalse(value);
        }

        [Test]
        public void IsRegularConjunction()
        {
            bool value = instance.IsRegularConjunction(",");
            ClassicAssert.IsTrue(value);
            value = instance.IsRegularConjunction("there");
            ClassicAssert.IsFalse(value);
        }

        [Test]
        public void IsSubordinateConjunction()
        {
            bool value = instance.IsSubordinateConjunction("even");
            ClassicAssert.IsTrue(value);
            value = instance.IsSubordinateConjunction("there");
            ClassicAssert.IsFalse(value);
        }
    }
}
