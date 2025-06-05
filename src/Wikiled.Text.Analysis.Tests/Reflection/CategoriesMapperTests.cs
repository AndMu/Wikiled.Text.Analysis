using System;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
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
            ClassicAssert.AreEqual(3, construction.AllChildFields.Count());
            ClassicAssert.AreEqual(2, construction.Fields.Count());
            ClassicAssert.AreEqual(2, construction.Categories.Count());
            ClassicAssert.AreEqual(1, construction.Categories.First().Fields.Count());
        }

        [Test]
        public void ResolveInstance()
        {
            var mapper = new CategoriesMapper();
            IMapCategory construction = mapper.Construct<MainItem>();
            MainItem main = new MainItem();
            ClassicAssert.AreEqual(main, construction.ResolveInstance(main));
            ClassicAssert.AreEqual(main.SubCat, construction.Categories.First().ResolveInstance(main));
        }
      
        [Test]
        public void ConstructNotAllowed()
        {
            var mapper = new CategoriesMapper();
            ClassicAssert.Throws<ArgumentOutOfRangeException>(() => mapper.Construct<AnotherMainItem>());
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
            ClassicAssert.AreEqual(true, construction["IsGood"].First().GetValue<bool>(main));
            ClassicAssert.AreEqual(4, construction["Weight"].First().GetValue<int>(main.SubCat));
            ClassicAssert.AreEqual(2, construction["Total"].First().GetValue<int>(main));
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
            ClassicAssert.AreEqual(true, main.IsGood);
            ClassicAssert.AreEqual(20, main.Total);
            ClassicAssert.AreEqual(10, main.SubCat.Weight);
        }
    }
}
