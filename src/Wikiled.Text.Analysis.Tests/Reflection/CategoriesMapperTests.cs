using System;
using System.Linq;
using NUnit.Framework;
using Wikiled.Text.Analysis.Reflection;
using Wikiled.Text.Analysis.Tests.Reflection.TestData;

namespace Wikiled.Text.Analysis.Tests.Reflection
{
    [TestFixture]
    public class CategoriesMapperTests
    {
        [Test]
        public void RegularConstruct()
        {
            var mapper = new CategoriesMapper();
            IMapCategory construction = mapper.Construct<MainItem>();
            Assert.AreEqual(3, construction.AllChildFields.Count());
            Assert.AreEqual(2, construction.Fields.Count());
            Assert.AreEqual(2, construction.Categories.Count());
            Assert.AreEqual(1, construction.Categories.First().Fields.Count());
        }

        [Test]
        public void ResolveInstance()
        {
            var mapper = new CategoriesMapper();
            IMapCategory construction = mapper.Construct<MainItem>();
            MainItem main = new MainItem();
            Assert.AreEqual(main, construction.ResolveInstance(main));
            Assert.AreEqual(main.SubCat, construction.Categories.First().ResolveInstance(main));
        }
      
        [Test]
        public void ConstructNotAllowed()
        {
            var mapper = new CategoriesMapper();
            Assert.Throws<ArgumentOutOfRangeException>(() => mapper.Construct<AnotherMainItem>());
        }

        [Test]
        public void GetValue()
        {
            var mapper = new CategoriesMapper();
            IMapCategory construction = mapper.Construct<MainItem>();
            MainItem main = new MainItem();
            main.IsGood = true;
            main.Total = 2;
            main.SubCat.Weight = 4;
            Assert.AreEqual(true, construction["IsGood"].First().GetValue<bool>(main));
            Assert.AreEqual(4, construction["Weight"].First().GetValue<int>(main.SubCat));
            Assert.AreEqual(2, construction["Total"].First().GetValue<int>(main));
        }

        [Test]
        public void SetValue()
        {
            var mapper = new CategoriesMapper();
            IMapCategory construction = mapper.Construct<MainItem>();
            MainItem main = new MainItem();
            construction["IsGood"].First().SetValue(main, true);
            construction["Weight"].First().SetValue(main.SubCat, 10);
            construction["Total"].First().SetValue(main, 20);
            Assert.AreEqual(true, main.IsGood);
            Assert.AreEqual(20, main.Total);
            Assert.AreEqual(10, main.SubCat.Weight);
        }
    }
}
