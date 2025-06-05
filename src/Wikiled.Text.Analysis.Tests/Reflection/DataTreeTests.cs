using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Wikiled.Text.Analysis.Reflection;
using Wikiled.Text.Analysis.Reflection.Data;
using Wikiled.Text.Analysis.Tests.Reflection.TestData;

namespace Wikiled.Text.Analysis.Tests.Reflection
{
    [TestFixture]
    public class DataTreeTests
    {
        [Test]
        public void Create()
        {
            var mapper = new CategoriesMapper();
            IMapCategory construction = mapper.Construct<MainItem>();
            MainItem main = new MainItem();
            DataTree tree = new DataTree(main, construction);
            ClassicAssert.AreEqual(main, tree.Instance);
            ClassicAssert.AreEqual(construction.Categories.Count(), tree.Branches.Count);
            ClassicAssert.AreEqual(main.SubCat, tree.Branches[0].Instance);
            ClassicAssert.AreEqual(construction.Fields.Count(), tree.Leafs.Count);
            ClassicAssert.AreEqual(construction.AllChildFields.Count(), tree.AllLeafs.Count());
        }

        [Test]
        public void TestCollection()
        {
            var mapper = new CategoriesMapper();
            IMapCategory construction = mapper.Construct<MainItem>();
            MainItem main = new MainItem();
            DataTree tree = new DataTree(main, construction);
            ClassicAssert.AreEqual(0, tree.Branches[1].Leafs.Count);
            main.Data["Test"] = 4;
            tree = new DataTree(main, construction);
            ClassicAssert.AreEqual(1, tree.Branches[1].Leafs.Count);

            ClassicAssert.AreEqual(4, tree.Branches[1].Leafs[0].Value);
            ClassicAssert.AreEqual("Test", tree.Branches[1].Leafs[0].Name);
            ClassicAssert.AreEqual("Test", tree.Branches[1].Leafs[0].Description);
        }

        [Test]
        public void GetSetValue()
        {
            var mapper = new CategoriesMapper();
            IMapCategory construction = mapper.Construct<MainItem>();
            MainItem main = new MainItem();
            main.IsGood = true;
            main.Total = 2;
            main.SubCat.Weight = 4;
            DataTree tree = new DataTree(main, construction);
            ClassicAssert.AreEqual(true, tree.Leafs[1].Value);
            ClassicAssert.AreEqual(2, tree.Leafs[0].Value);
            ClassicAssert.AreEqual(4, tree.Branches[0].Leafs[0].Value);

            tree.Leafs[1].Value = false;
            ClassicAssert.AreEqual(false, tree.Leafs[1].Value);
            ClassicAssert.AreEqual(false, main.IsGood);

            tree.Leafs[0].Value = 88;
            ClassicAssert.AreEqual(88, tree.Leafs[0].Value);
            ClassicAssert.AreEqual(88, main.Total);

            tree.Branches[0].Leafs[0].Value = 7;
            ClassicAssert.AreEqual(7, tree.Branches[0].Leafs[0].Value);
            ClassicAssert.AreEqual(7, main.SubCat.Weight);
        }

        [Test]
        public void DictionaryValue()
        {
            Dictionary<string, double> map = new Dictionary<string, double>();
            map["IsGood"] = 0.1;
            map["Weight"] = 3;
            var mapper = new CategoriesMapper();
            IMapCategory construction = mapper.Construct<MainItem>();
            MainItem main = new MainItem();
            main.IsGood = true;
            main.Total = 2;
            main.SubCat.Weight = 4;
            DataTree tree = new DataTree(main, construction, new DictionaryDataItemFactory(map));
            ClassicAssert.AreEqual(0.1, Math.Round((double)tree.Leafs[1].Value, 2));
            ClassicAssert.AreEqual(0, tree.Leafs[0].Value);
            ClassicAssert.AreEqual(3, tree.Branches[0].Leafs[0].Value);
        }
    }
}
