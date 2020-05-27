using NUnit.Framework;
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
            Assert.AreEqual("I went to forest and don't know what I thought. But that is ok and not so bad and ok", document.Text);
            Assert.AreEqual(20, document.TotalWords);
            Assert.AreEqual(2, document.Sentences.Count);
            Assert.AreEqual("I went to forest and don't know what I thought.", document.Sentences[0].Text);
            Assert.AreEqual(10, document.Sentences[0].Words.Count);
            Assert.AreEqual("i", document.Sentences[0].Words[0].Text);
            Assert.AreEqual("went", document.Sentences[0].Words[1].Text);
            Assert.AreEqual("to", document.Sentences[0].Words[2].Text);
            Assert.AreEqual("forest", document.Sentences[0].Words[3].Text);
            Assert.AreEqual("and", document.Sentences[0].Words[4].Text);
            Assert.AreEqual("don't", document.Sentences[0].Words[5].Text);
            Assert.AreEqual("know", document.Sentences[0].Words[6].ItemText);
            Assert.AreEqual("what", document.Sentences[0].Words[7].Text);
            Assert.AreEqual("i", document.Sentences[0].Words[8].Text);
            Assert.AreEqual("thought", document.Sentences[0].Words[9].Text);

            Assert.AreEqual("But that is ok and not so bad and ok", document.Sentences[1].Text);
            Assert.AreEqual(10, document.Sentences[1].Words.Count);
            Assert.AreEqual("but", document.Sentences[1].Words[0].Text);
            Assert.AreEqual("that", document.Sentences[1].Words[1].Text);
            Assert.AreEqual("is", document.Sentences[1].Words[2].Text);
            Assert.AreEqual("ok", document.Sentences[1].Words[3].Text);
            Assert.AreEqual("and", document.Sentences[1].Words[4].Text);
            Assert.AreEqual("not", document.Sentences[1].Words[5].Text);
            Assert.AreEqual("so", document.Sentences[1].Words[6].Text);
            Assert.AreEqual("bad", document.Sentences[1].Words[7].Text);
            Assert.AreEqual("and", document.Sentences[1].Words[8].Text);
            Assert.AreEqual("ok", document.Sentences[1].Words[9].Text);
        }

        [Test]
        public void GetDocument1()
        {
            SimpleWordsExtraction extraction = new SimpleWordsExtraction(Global.Factory.Create(false, false));
            Document document = extraction.GetDocument("I went to forest and don't know what I thought. But that is ok and not so bad and ok");
            Assert.AreEqual(18, document.TotalWords);
            Assert.AreEqual(2, document.Sentences.Count);
            Assert.AreEqual("I went to forest and don't know what I thought.", document.Sentences[0].Text);
            Assert.AreEqual(9, document.Sentences[0].Words.Count);
            Assert.AreEqual("i", document.Sentences[0].Words[0].Text);
            Assert.AreEqual("went", document.Sentences[0].Words[1].Text);
            Assert.AreEqual("to", document.Sentences[0].Words[2].Text);
            Assert.AreEqual("forest", document.Sentences[0].Words[3].Text);
            Assert.AreEqual("and", document.Sentences[0].Words[4].Text);
            Assert.AreEqual("not_know", document.Sentences[0].Words[5].Text);
            Assert.AreEqual("know", document.Sentences[0].Words[5].ItemText);
            Assert.AreEqual("not_what", document.Sentences[0].Words[6].Text);
            Assert.AreEqual("not_i", document.Sentences[0].Words[7].Text);
            Assert.AreEqual("not_thought", document.Sentences[0].Words[8].Text);

            Assert.AreEqual("But that is ok and not so bad and ok", document.Sentences[1].Text);
            Assert.AreEqual(9, document.Sentences[1].Words.Count);
            Assert.AreEqual("but", document.Sentences[1].Words[0].Text);
            Assert.AreEqual("that", document.Sentences[1].Words[1].Text);
            Assert.AreEqual("is", document.Sentences[1].Words[2].Text);
            Assert.AreEqual("ok", document.Sentences[1].Words[3].Text);
            Assert.AreEqual("and", document.Sentences[1].Words[4].Text);
            Assert.AreEqual("not_so", document.Sentences[1].Words[5].Text);
            Assert.AreEqual("not_bad", document.Sentences[1].Words[6].Text);
            Assert.AreEqual("and", document.Sentences[1].Words[7].Text);
            Assert.AreEqual("ok", document.Sentences[1].Words[8].Text);
        }

        [Test]
        public void GetDocument1WithoutStop()
        {
            SimpleWordsExtraction extraction = new SimpleWordsExtraction(Global.Factory.Create(false, true));
            Document document = extraction.GetDocument("I went to forest and don't know what I thought. But that is ok and not so bad and ok");
            Assert.AreEqual(7, document.TotalWords);
            Assert.AreEqual(2, document.Sentences.Count);
            Assert.AreEqual("I went to forest and don't know what I thought.", document.Sentences[0].Text);
            Assert.AreEqual(4, document.Sentences[0].Words.Count);
            Assert.AreEqual("went", document.Sentences[0].Words[0].Text);
            Assert.AreEqual("forest", document.Sentences[0].Words[1].Text);
            Assert.AreEqual("not_know", document.Sentences[0].Words[2].Text);
            Assert.AreEqual("not_thought", document.Sentences[0].Words[3].Text);

            Assert.AreEqual("But that is ok and not so bad and ok", document.Sentences[1].Text);
            Assert.AreEqual(3, document.Sentences[1].Words.Count);
            Assert.AreEqual("ok", document.Sentences[1].Words[0].Text);
            Assert.AreEqual("not_bad", document.Sentences[1].Words[1].Text);
            Assert.AreEqual("ok", document.Sentences[1].Words[2].Text);
        }

        [Test]
        public void GetDocumentFromLDA()
        {
            SimpleWordsExtraction extraction = new SimpleWordsExtraction(Global.Factory.Create(true, false));
            Document document =
                extraction.GetDocument(
                    "Elizabeth Needham (died 3 May 1731), also known as Mother Needham");
            Assert.AreEqual(14, document.TotalWords);
            Assert.AreEqual(1, document.Sentences.Count);
            Assert.AreEqual(14, document.Sentences[0].Words.Count);
            Assert.AreEqual("elizabeth", document.Sentences[0].Words[0].Text);
            Assert.AreEqual("needham", document.Sentences[0].Words[1].Text);
            Assert.AreEqual("(", document.Sentences[0].Words[2].Text);
            Assert.AreEqual("died", document.Sentences[0].Words[3].Text);
            Assert.AreEqual("3", document.Sentences[0].Words[4].Text);
            Assert.AreEqual("may", document.Sentences[0].Words[5].Text);
            Assert.AreEqual("1731", document.Sentences[0].Words[6].Text);
            Assert.AreEqual(")", document.Sentences[0].Words[7].Text);
            Assert.AreEqual(",", document.Sentences[0].Words[8].Text);
            Assert.AreEqual("also", document.Sentences[0].Words[9].Text);
        }

        [Test]
        public void GetDocument2()
        {
            SimpleWordsExtraction extraction = new SimpleWordsExtraction(Global.Factory.Create(false, false));
            Document document = extraction.GetDocument("Not bad Not bad and defintle again will do that. For you my king. I spent that road.");
            Assert.AreEqual(16, document.TotalWords);
            Assert.AreEqual(3, document.Sentences.Count);

            Assert.AreEqual("Not bad Not bad and defintle again will do that.", document.Sentences[0].Text);
            Assert.AreEqual(8, document.Sentences[0].Words.Count);
            Assert.AreEqual("not_bad", document.Sentences[0].Words[0].Text);
            Assert.AreEqual("not_bad", document.Sentences[0].Words[1].Text);
            Assert.AreEqual("and", document.Sentences[0].Words[2].Text);
            Assert.AreEqual("defintle", document.Sentences[0].Words[3].Text);
            Assert.AreEqual("again", document.Sentences[0].Words[4].Text);
            Assert.AreEqual("will", document.Sentences[0].Words[5].Text);
            Assert.AreEqual("do", document.Sentences[0].Words[6].Text);
            Assert.AreEqual("that", document.Sentences[0].Words[7].Text);

            Assert.AreEqual("For you my king.", document.Sentences[1].Text);
            Assert.AreEqual(4, document.Sentences[1].Words.Count);
            Assert.AreEqual("for", document.Sentences[1].Words[0].Text);
            Assert.AreEqual("you", document.Sentences[1].Words[1].Text);
            Assert.AreEqual("my", document.Sentences[1].Words[2].Text);
            Assert.AreEqual("king", document.Sentences[1].Words[3].Text);

            Assert.AreEqual("I spent that road.", document.Sentences[2].Text);
            Assert.AreEqual(4, document.Sentences[2].Words.Count);
            Assert.AreEqual("i", document.Sentences[2].Words[0].Text);
            Assert.AreEqual("spent", document.Sentences[2].Words[1].Text);
            Assert.AreEqual("that", document.Sentences[2].Words[2].Text);
            Assert.AreEqual("road", document.Sentences[2].Words[3].Text);
        }
    }
}
