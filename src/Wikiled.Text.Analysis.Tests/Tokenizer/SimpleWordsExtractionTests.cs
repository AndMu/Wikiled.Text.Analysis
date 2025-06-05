using NUnit.Framework;
using NUnit.Framework.Legacy;
using Wikiled.Text.Analysis.Structure;
using Wikiled.Text.Analysis.Tokenizer;

namespace Wikiled.Text.Analysis.Tests.Tokenizer
{
    [TestFixture]
    public class SimpleWordsExtractionTests
    {
        [Test]
        public void GetDocument1Simple()
        {
            SimpleWordsExtraction extraction = new SimpleWordsExtraction(Global.Factory.Create(true, false));
            Document document = extraction.GetDocument("I went to forest and don't know what I thought. But that is ok and not so bad and ok");
            ClassicAssert.AreEqual("I went to forest and don't know what I thought. But that is ok and not so bad and ok", document.Text);
            ClassicAssert.AreEqual(20, document.TotalWords);
            ClassicAssert.AreEqual(2, document.Sentences.Count);
            ClassicAssert.AreEqual("I went to forest and don't know what I thought.", document.Sentences[0].Text);
            ClassicAssert.AreEqual(10, document.Sentences[0].Words.Count);
            ClassicAssert.AreEqual("i", document.Sentences[0].Words[0].Text);
            ClassicAssert.AreEqual("went", document.Sentences[0].Words[1].Text);
            ClassicAssert.AreEqual("to", document.Sentences[0].Words[2].Text);
            ClassicAssert.AreEqual("forest", document.Sentences[0].Words[3].Text);
            ClassicAssert.AreEqual("and", document.Sentences[0].Words[4].Text);
            ClassicAssert.AreEqual("don't", document.Sentences[0].Words[5].Text);
            ClassicAssert.AreEqual("know", document.Sentences[0].Words[6].ItemText);
            ClassicAssert.AreEqual("what", document.Sentences[0].Words[7].Text);
            ClassicAssert.AreEqual("i", document.Sentences[0].Words[8].Text);
            ClassicAssert.AreEqual("thought", document.Sentences[0].Words[9].Text);

            ClassicAssert.AreEqual("But that is ok and not so bad and ok", document.Sentences[1].Text);
            ClassicAssert.AreEqual(10, document.Sentences[1].Words.Count);
            ClassicAssert.AreEqual("but", document.Sentences[1].Words[0].Text);
            ClassicAssert.AreEqual("that", document.Sentences[1].Words[1].Text);
            ClassicAssert.AreEqual("is", document.Sentences[1].Words[2].Text);
            ClassicAssert.AreEqual("ok", document.Sentences[1].Words[3].Text);
            ClassicAssert.AreEqual("and", document.Sentences[1].Words[4].Text);
            ClassicAssert.AreEqual("not", document.Sentences[1].Words[5].Text);
            ClassicAssert.AreEqual("so", document.Sentences[1].Words[6].Text);
            ClassicAssert.AreEqual("bad", document.Sentences[1].Words[7].Text);
            ClassicAssert.AreEqual("and", document.Sentences[1].Words[8].Text);
            ClassicAssert.AreEqual("ok", document.Sentences[1].Words[9].Text);
        }

        [Test]
        public void GetDocument1()
        {
            SimpleWordsExtraction extraction = new SimpleWordsExtraction(Global.Factory.Create(false, false));
            Document document = extraction.GetDocument("I went to forest and don't know what I thought. But that is ok and not so bad and ok");
            ClassicAssert.AreEqual(18, document.TotalWords);
            ClassicAssert.AreEqual(2, document.Sentences.Count);
            ClassicAssert.AreEqual("I went to forest and don't know what I thought.", document.Sentences[0].Text);
            ClassicAssert.AreEqual(9, document.Sentences[0].Words.Count);
            ClassicAssert.AreEqual("i", document.Sentences[0].Words[0].Text);
            ClassicAssert.AreEqual("went", document.Sentences[0].Words[1].Text);
            ClassicAssert.AreEqual("to", document.Sentences[0].Words[2].Text);
            ClassicAssert.AreEqual("forest", document.Sentences[0].Words[3].Text);
            ClassicAssert.AreEqual("and", document.Sentences[0].Words[4].Text);
            ClassicAssert.AreEqual("not_know", document.Sentences[0].Words[5].Text);
            ClassicAssert.AreEqual("know", document.Sentences[0].Words[5].ItemText);
            ClassicAssert.AreEqual("not_what", document.Sentences[0].Words[6].Text);
            ClassicAssert.AreEqual("not_i", document.Sentences[0].Words[7].Text);
            ClassicAssert.AreEqual("not_thought", document.Sentences[0].Words[8].Text);

            ClassicAssert.AreEqual("But that is ok and not so bad and ok", document.Sentences[1].Text);
            ClassicAssert.AreEqual(9, document.Sentences[1].Words.Count);
            ClassicAssert.AreEqual("but", document.Sentences[1].Words[0].Text);
            ClassicAssert.AreEqual("that", document.Sentences[1].Words[1].Text);
            ClassicAssert.AreEqual("is", document.Sentences[1].Words[2].Text);
            ClassicAssert.AreEqual("ok", document.Sentences[1].Words[3].Text);
            ClassicAssert.AreEqual("and", document.Sentences[1].Words[4].Text);
            ClassicAssert.AreEqual("not_so", document.Sentences[1].Words[5].Text);
            ClassicAssert.AreEqual("not_bad", document.Sentences[1].Words[6].Text);
            ClassicAssert.AreEqual("and", document.Sentences[1].Words[7].Text);
            ClassicAssert.AreEqual("ok", document.Sentences[1].Words[8].Text);
        }

        [Test]
        public void GetDocument1WithoutStop()
        {
            SimpleWordsExtraction extraction = new SimpleWordsExtraction(Global.Factory.Create(false, true));
            Document document = extraction.GetDocument("I went to forest and don't know what I thought. But that is ok and not so bad and ok");
            ClassicAssert.AreEqual(7, document.TotalWords);
            ClassicAssert.AreEqual(2, document.Sentences.Count);
            ClassicAssert.AreEqual("I went to forest and don't know what I thought.", document.Sentences[0].Text);
            ClassicAssert.AreEqual(4, document.Sentences[0].Words.Count);
            ClassicAssert.AreEqual("went", document.Sentences[0].Words[0].Text);
            ClassicAssert.AreEqual("forest", document.Sentences[0].Words[1].Text);
            ClassicAssert.AreEqual("not_know", document.Sentences[0].Words[2].Text);
            ClassicAssert.AreEqual("not_thought", document.Sentences[0].Words[3].Text);

            ClassicAssert.AreEqual("But that is ok and not so bad and ok", document.Sentences[1].Text);
            ClassicAssert.AreEqual(3, document.Sentences[1].Words.Count);
            ClassicAssert.AreEqual("ok", document.Sentences[1].Words[0].Text);
            ClassicAssert.AreEqual("not_bad", document.Sentences[1].Words[1].Text);
            ClassicAssert.AreEqual("ok", document.Sentences[1].Words[2].Text);
        }

        [Test]
        public void GetDocumentFromLDA()
        {
            SimpleWordsExtraction extraction = new SimpleWordsExtraction(Global.Factory.Create(true, false));
            Document document =
                extraction.GetDocument(
                    "Elizabeth Needham (died 3 May 1731), also known as Mother Needham");
            ClassicAssert.AreEqual(14, document.TotalWords);
            ClassicAssert.AreEqual(1, document.Sentences.Count);
            ClassicAssert.AreEqual(14, document.Sentences[0].Words.Count);
            ClassicAssert.AreEqual("elizabeth", document.Sentences[0].Words[0].Text);
            ClassicAssert.AreEqual("needham", document.Sentences[0].Words[1].Text);
            ClassicAssert.AreEqual("(", document.Sentences[0].Words[2].Text);
            ClassicAssert.AreEqual("died", document.Sentences[0].Words[3].Text);
            ClassicAssert.AreEqual("3", document.Sentences[0].Words[4].Text);
            ClassicAssert.AreEqual("may", document.Sentences[0].Words[5].Text);
            ClassicAssert.AreEqual("1731", document.Sentences[0].Words[6].Text);
            ClassicAssert.AreEqual(")", document.Sentences[0].Words[7].Text);
            ClassicAssert.AreEqual(",", document.Sentences[0].Words[8].Text);
            ClassicAssert.AreEqual("also", document.Sentences[0].Words[9].Text);
        }

        [Test]
        public void GetDocument2()
        {
            SimpleWordsExtraction extraction = new SimpleWordsExtraction(Global.Factory.Create(false, false));
            Document document = extraction.GetDocument("Not bad Not bad and defintle again will do that. For you my king. I spent that road.");
            ClassicAssert.AreEqual(16, document.TotalWords);
            ClassicAssert.AreEqual(3, document.Sentences.Count);

            ClassicAssert.AreEqual("Not bad Not bad and defintle again will do that.", document.Sentences[0].Text);
            ClassicAssert.AreEqual(8, document.Sentences[0].Words.Count);
            ClassicAssert.AreEqual("not_bad", document.Sentences[0].Words[0].Text);
            ClassicAssert.AreEqual("not_bad", document.Sentences[0].Words[1].Text);
            ClassicAssert.AreEqual("and", document.Sentences[0].Words[2].Text);
            ClassicAssert.AreEqual("defintle", document.Sentences[0].Words[3].Text);
            ClassicAssert.AreEqual("again", document.Sentences[0].Words[4].Text);
            ClassicAssert.AreEqual("will", document.Sentences[0].Words[5].Text);
            ClassicAssert.AreEqual("do", document.Sentences[0].Words[6].Text);
            ClassicAssert.AreEqual("that", document.Sentences[0].Words[7].Text);

            ClassicAssert.AreEqual("For you my king.", document.Sentences[1].Text);
            ClassicAssert.AreEqual(4, document.Sentences[1].Words.Count);
            ClassicAssert.AreEqual("for", document.Sentences[1].Words[0].Text);
            ClassicAssert.AreEqual("you", document.Sentences[1].Words[1].Text);
            ClassicAssert.AreEqual("my", document.Sentences[1].Words[2].Text);
            ClassicAssert.AreEqual("king", document.Sentences[1].Words[3].Text);

            ClassicAssert.AreEqual("I spent that road.", document.Sentences[2].Text);
            ClassicAssert.AreEqual(4, document.Sentences[2].Words.Count);
            ClassicAssert.AreEqual("i", document.Sentences[2].Words[0].Text);
            ClassicAssert.AreEqual("spent", document.Sentences[2].Words[1].Text);
            ClassicAssert.AreEqual("that", document.Sentences[2].Words[2].Text);
            ClassicAssert.AreEqual("road", document.Sentences[2].Words[3].Text);
        }
    }
}
