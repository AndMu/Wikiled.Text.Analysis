using NUnit.Framework;
using Wikiled.Text.Analysis.Structure.Raw;

namespace Wikiled.Text.Analysis.Tests.Structure.Raw
{
    [TestFixture]
    public class RawDocumentTests
    {
        private RawDocument instance;

        [SetUp]
        public void SetUp()
        {
            instance = CreateRawDocument();
        }

        [Test]
        public void BuildEmpty()
        {
            var result = instance.Build();
            Assert.IsEmpty(result);
        }

        [Test]
        public void Build()
        {
            instance.Pages = new[] {new RawPage(), new RawPage(),};
            instance.Pages[0].Blocks = new[] {new TextBlockItem(), new TextBlockItem()};
            instance.Pages[1].Blocks = new[] { new TextBlockItem(), new TextBlockItem() };
            instance.Pages[0].Blocks[0].Text = "I";
            instance.Pages[0].Blocks[1].Text = "II";
            instance.Pages[1].Blocks[0].Text = "I-I";
            instance.Pages[1].Blocks[1].Text = "I-II";

            var result = instance.Build();
            Assert.AreEqual("I\r\nII\r\nI-I\r\nI-II", result);
        }

        private RawDocument CreateRawDocument()
        {
            return new RawDocument();
        }
    }
}
