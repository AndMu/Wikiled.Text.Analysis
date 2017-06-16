﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Wikiled.Text.Analysis.Twitter;
using YamlDotNet.RepresentationModel;

namespace Wikiled.Text.Analysis.Tests.Twitter
{
    [TestFixture]
    public class ConformanceTests
    {
        private Autolink autolink;

        private Extractor extractor;

        private HitHighlighter highlighter = new HitHighlighter();

        private Validator validator;

        [SetUp]
        public void Setup()
        {
            highlighter = new HitHighlighter();
            validator = new Validator();
            extractor = new Extractor();
            autolink = new Autolink { NoFollow = false };
        }

        [Test]
        public void AutolinkAll()
        {
            var failures = new List<string>();
            foreach(var test in LoadTests<string>("autolink.yml", "all"))
            {
                try
                {
                    string actual = autolink.AutoLink(test.Text);
                    Assert.AreEqual(test.Expected, actual);
                }
                catch(Exception)
                {
                    failures.Add(test.Description + ": " + test.Text);
                }
            }
            if(failures.Any())
            {
                Assert.Fail(string.Join("\n", failures));
            }
        }

        [Test]
        public void AutolinkCashtags()
        {
            var failures = new List<string>();
            foreach(var test in LoadTests<string>("autolink.yml", "cashtags"))
            {
                try
                {
                    string actual = autolink.AutoLinkCashtags(test.Text);
                    Assert.AreEqual(test.Expected, actual);
                }
                catch(Exception)
                {
                    failures.Add(test.Description + ": " + test.Text);
                }
            }

            if(failures.Any())
            {
                Assert.Fail(string.Join("\n", failures));
            }
        }

        [Test]
        public void AutolinkHashtags()
        {
            var failures = new List<string>();
            foreach(var test in LoadTests<string>("autolink.yml", "hashtags"))
            {
                try
                {
                    string actual = autolink.AutoLinkHashtags(test.Text);
                    Assert.AreEqual(test.Expected, actual);
                }
                catch(Exception)
                {
                    failures.Add(test.Description + ": " + test.Text);
                }
            }

            if(failures.Any())
            {
                Assert.Fail(string.Join("\n", failures));
            }
        }

        [Test]
        public void AutolinkLists()
        {
            var failures = new List<string>();
            foreach(var test in LoadTests<string>("autolink.yml", "lists"))
            {
                try
                {
                    string actual = autolink.AutoLinkUsernamesAndLists(test.Text);
                    Assert.AreEqual(test.Expected, actual);
                }
                catch(Exception)
                {
                    failures.Add(test.Description + ": " + test.Text);
                }
            }

            if(failures.Any())
            {
                Assert.Fail(string.Join("\n", failures));
            }
        }

        [Test]
        public void AutolinkUrls()
        {
            var failures = new List<string>();
            foreach(var test in LoadTests<string>("autolink.yml", "urls"))
            {
                try
                {
                    string actual = autolink.AutoLinkUrls(test.Text);
                    Assert.AreEqual(test.Expected, actual);
                }
                catch(Exception)
                {
                    failures.Add(test.Description + ": " + test.Text);
                }
            }

            if(failures.Any())
            {
                Assert.Fail(string.Join("\n", failures));
            }
        }

        [Test]
        public void AutolinkUsernames()
        {
            var failures = new List<string>();
            foreach(var test in LoadTests<string>("autolink.yml", "usernames"))
            {
                try
                {
                    string actual = autolink.AutoLinkUsernamesAndLists(test.Text);
                    Assert.AreEqual(test.Expected, actual);
                }
                catch(Exception)
                {
                    failures.Add(test.Description + ": " + test.Text);
                }
            }

            if(failures.Any())
            {
                Assert.Fail(string.Join("\n", failures));
            }
        }

        [Test]
        public void CountryTlds()
        {
            var failures = new List<string>();
            foreach(var test in LoadTests<List<string>>("tlds.yml", "country"))
            {
                try
                {
                    List<string> actual = new List<string>(extractor.ExtractUrls(test.Text));
                    CollectionAssert.AreEqual(test.Expected, actual);
                }
                catch(Exception)
                {
                    failures.Add(test.Description + ": " + test.Text);
                }
            }

            if(failures.Any())
            {
                Assert.Fail(string.Join("\n", failures));
            }
        }

        [Test]
        public void ExtractCashtags()
        {
            var failures = new List<string>();
            foreach(var test in LoadTests<dynamic>("extract.yml", "cashtags"))
            {
                try
                {
                    List<string> actual = new List<string>(extractor.ExtractCashtags(test.Text));
                    CollectionAssert.AreEquivalent(test.Expected, actual);
                }
                catch(Exception)
                {
                    failures.Add(test.Description + ": " + test.Text);
                }
            }

            if(failures.Any())
            {
                Assert.Fail(string.Join("\n", failures));
            }
        }

        [Test]
        public void ExtractCashtagsWithIndices()
        {
            var failures = new List<string>();
            foreach(var test in LoadTests<dynamic>("extract.yml", "cashtags_with_indices"))
            {
                try
                {
                    var actual = extractor.ExtractCashtagsWithIndices(test.Text);
                    for(int i = 0; i < actual.Length; i++)
                    {
                        var entity = actual[i];
                        Assert.AreEqual(test.Expected[i].cashtag, entity.Value);
                        Assert.AreEqual(test.Expected[i].indices[0], entity.Start);
                        Assert.AreEqual(test.Expected[i].indices[1], entity.End);
                    }
                }
                catch(Exception)
                {
                    failures.Add(test.Description + ": " + test.Text);
                }
            }

            if(failures.Any())
            {
                Assert.Fail(string.Join("\n", failures));
            }
        }

        [Test]
        public void ExtractHashtags()
        {
            var failures = new List<string>();
            foreach(var test in LoadTests<dynamic>("extract.yml", "hashtags"))
            {
                try
                {
                    List<string> actual = new List<string>(extractor.ExtractHashtags(test.Text));
                    CollectionAssert.AreEquivalent(test.Expected, actual);
                }
                catch(Exception)
                {
                    failures.Add(test.Description + ": " + test.Text);
                }
            }

            if(failures.Any())
            {
                Assert.Fail(string.Join("\n", failures));
            }
        }

        [Test]
        public void ExtractHashtagsWithIndices()
        {
            var failures = new List<string>();
            foreach(var test in LoadTests<dynamic>("extract.yml", "hashtags_with_indices"))
            {
                try
                {
                    var actual = extractor.ExtractHashtagsWithIndices(test.Text);
                    for(int i = 0; i < actual.Length; i++)
                    {
                        var entity = actual[i];
                        Assert.AreEqual(test.Expected[i].hashtag, entity.Value);
                        Assert.AreEqual(test.Expected[i].indices[0], entity.Start);
                        Assert.AreEqual(test.Expected[i].indices[1], entity.End);
                    }
                }
                catch(Exception)
                {
                    failures.Add(test.Description + ": " + test.Text);
                }
            }

            if(failures.Any())
            {
                Assert.Fail(string.Join("\n", failures));
            }
        }

        [Test]
        public void ExtractMentions()
        {
            var failures = new List<string>();
            foreach(var test in LoadTests<dynamic>("extract.yml", "mentions"))
            {
                try
                {
                    var actual = extractor.ExtractMentionedScreennames(test.Text);
                    CollectionAssert.AreEquivalent(test.Expected, actual);
                }
                catch(Exception)
                {
                    failures.Add(test.Description + ": " + test.Text);
                }
            }

            if(failures.Any())
            {
                Assert.Fail(string.Join("\n", failures));
            }
        }

        [Test]
        public void ExtractMentionsOrListsWithIndices()
        {
            var failures = new List<string>();
            foreach(var test in LoadTests<dynamic>("extract.yml", "mentions_or_lists_with_indices"))
            {
                try
                {
                    List<TweetEntity> actual = new List<TweetEntity>(extractor.ExtractMentionsOrListsWithIndices(test.Text));
                    for(int i = 0; i < actual.Count; i++)
                    {
                        var entity = actual[i];
                        Assert.AreEqual(test.Expected[i].screen_name, entity.Value);
                        Assert.AreEqual(test.Expected[i].list_slug, entity.ListSlug);
                        Assert.AreEqual(test.Expected[i].indices[0], entity.Start);
                        Assert.AreEqual(test.Expected[i].indices[1], entity.End);
                    }
                }
                catch(Exception)
                {
                    failures.Add(test.Description + ": " + test.Text);
                }
            }

            if(failures.Any())
            {
                Assert.Fail(string.Join("\n", failures));
            }
        }

        [Test]
        public void ExtractMentionsWithIndices()
        {
            var failures = new List<string>();
            foreach(var test in LoadTests<dynamic>("extract.yml", "mentions_with_indices"))
            {
                try
                {
                    var actual = extractor.ExtractMentionedScreennamesWithIndices(test.Text);
                    for(int i = 0; i < actual.Length; i++)
                    {
                        var entity = actual[i];
                        Assert.AreEqual(test.Expected[i].screen_name, entity.Value);
                        Assert.AreEqual(test.Expected[i].indices[0], entity.Start);
                        Assert.AreEqual(test.Expected[i].indices[1], entity.End);
                    }
                }
                catch(Exception)
                {
                    failures.Add(test.Description + ": " + test.Text);
                }
            }

            if(failures.Any())
            {
                Assert.Fail(string.Join("\n", failures));
            }
        }

        [Test]
        public void ExtractReplies()
        {
            var failures = new List<string>();
            foreach(var test in LoadTests<dynamic>("extract.yml", "replies"))
            {
                try
                {
                    string actual = extractor.ExtractReplyScreenname(test.Text);
                    Assert.AreEqual(test.Expected, actual);
                }
                catch(Exception)
                {
                    failures.Add(test.Description + ": " + test.Text);
                }
            }

            if(failures.Any())
            {
                Assert.Fail(string.Join("\n", failures));
            }
        }

        [Test]
        public void ExtractUrls()
        {
            var failures = new List<string>();
            foreach(var test in LoadTests<dynamic>("extract.yml", "urls"))
            {
                try
                {
                    List<string> actual = new List<string>(extractor.ExtractUrls(test.Text));
                    CollectionAssert.AreEquivalent(test.Expected, actual);
                }
                catch(Exception)
                {
                    failures.Add(test.Description + ": " + test.Text);
                }
            }

            if(failures.Any())
            {
                Assert.Fail(string.Join("\n", failures));
            }
        }

        [Test]
        public void ExtractUrlsWithIndices()
        {
            var failures = new List<string>();
            foreach(var test in LoadTests<dynamic>("extract.yml", "urls_with_indices"))
            {
                try
                {
                    var actual = extractor.ExtractUrlsWithIndices(test.Text);
                    for(int i = 0; i < actual.Length; i++)
                    {
                        var entity = actual[i];
                        Assert.AreEqual(test.Expected[i].url, entity.Value);
                        Assert.AreEqual(test.Expected[i].indices[0], entity.Start);
                        Assert.AreEqual(test.Expected[i].indices[1], entity.End);
                    }
                }
                catch(Exception)
                {
                    failures.Add(test.Description + ": " + test.Text);
                }
            }

            if(failures.Any())
            {
                Assert.Fail(string.Join("\n", failures));
            }
        }

        [Test]
        public void GenericTlds()
        {
            var failures = new List<string>();
            foreach(var test in LoadTests<List<string>>("tlds.yml", "generic"))
            {
                try
                {
                    List<string> actual = new List<string>(extractor.ExtractUrls(test.Text));
                    CollectionAssert.AreEqual(test.Expected, actual);
                }
                catch(Exception)
                {
                    failures.Add(test.Description + ": " + test.Text);
                }
            }

            if(failures.Any())
            {
                Assert.Fail(string.Join("\n", failures));
            }
        }

        [Test]
        public void HighlightPlainText()
        {
            var failures = new List<string>();
            foreach(var test in LoadTests<string>("hit_highlighting.yml", "plain_text"))
            {
                try
                {
                    string actual = highlighter.Highlight(test.Text, test.Hits);
                    Assert.AreEqual(test.Expected, actual);
                }
                catch(Exception)
                {
                    failures.Add(test.Description + ": " + test.Text);
                }
            }

            if(failures.Any())
            {
                Assert.Fail(string.Join("\n", failures));
            }
        }

        [Test]
        public void HighlightWithLinks()
        {
            var failures = new List<string>();
            foreach(var test in LoadTests<string>("hit_highlighting.yml", "with_links"))
            {
                try
                {
                    string actual = highlighter.Highlight(test.Text, test.Hits);
                    Assert.AreEqual(test.Expected, actual);
                }
                catch(Exception)
                {
                    failures.Add(test.Description + ": " + test.Text);
                }
            }

            if(failures.Any())
            {
                Assert.Fail(string.Join("\n", failures));
            }
        }

        [Test]
        public void ValidateHashTags()
        {
            var failures = new List<string>();
            foreach(var test in LoadTests<bool>("validate.yml", "hashtags"))
            {
                try
                {
                    bool actual = validator.IsValidHashTag(test.Text);
                    Assert.AreEqual(test.Expected, actual);
                }
                catch(Exception)
                {
                    failures.Add(test.Description + ": " + test.Text);
                }
            }

            if(failures.Any())
            {
                Assert.Fail(string.Join("\n", failures));
            }
        }

        [Test]
        public void ValidateLengths()
        {
            var failures = new List<string>();
            int actual = default(int);
            foreach(var test in LoadTests<int>("validate.yml", "lengths"))
            {
                try
                {
                    actual = validator.GetTweetLength(test.Text);
                    Assert.AreEqual(test.Expected, actual);
                }
                catch(Exception)
                {
                    failures.Add(test.Description + " – " + test.Text + " : expected " + test.Expected + ", actual " + actual + "");
                }
            }

            if(failures.Any())
            {
                Assert.Fail(string.Join("\n", failures));
            }
        }

        [Test]
        public void ValidateLists()
        {
            var failures = new List<string>();
            foreach(var test in LoadTests<bool>("validate.yml", "lists"))
            {
                try
                {
                    bool actual = validator.IsValidList(test.Text);
                    Assert.AreEqual(test.Expected, actual);
                }
                catch(Exception)
                {
                    failures.Add(test.Description + ": " + test.Text);
                }
            }

            if(failures.Any())
            {
                Assert.Fail(string.Join("\n", failures));
            }
        }

        [Test]
        public void ValidateTweets()
        {
            var failures = new List<string>();
            foreach(var test in LoadTests<bool>("validate.yml", "tweets"))
            {
                try
                {
                    bool actual = validator.IsValidTweet(test.Text);
                    Assert.AreEqual(test.Expected, actual);
                }
                catch(Exception)
                {
                    failures.Add(test.Description + ": " + test.Text);
                }
            }

            if(failures.Any())
            {
                Assert.Fail(string.Join("\n", failures));
            }
        }

        [Test]
        public void ValidateUrls()
        {
            var failures = new List<string>();
            foreach(var test in LoadTests<bool>("validate.yml", "urls"))
            {
                try
                {
                    bool actual = validator.IsValidUrl(test.Text);
                    Assert.AreEqual(test.Expected, actual);
                }
                catch(Exception)
                {
                    failures.Add(test.Description + ": " + test.Text);
                }
            }

            if(failures.Any())
            {
                Assert.Fail(string.Join("\n", failures));
            }
        }

        [Test]
        public void ValidateUrlsWithoutProtocol()
        {
            var failures = new List<string>();
            foreach(var test in LoadTests<bool>("validate.yml", "urls_without_protocol"))
            {
                try
                {
                    bool actual = validator.IsValidUrl(test.Text);
                    Assert.AreEqual(test.Expected, actual);
                }
                catch(Exception)
                {
                    failures.Add(test.Description + ": " + test.Text);
                }
            }

            if(failures.Any())
            {
                Assert.Fail(string.Join("\n", failures));
            }
        }

        [Test]
        public void ValidateUsernames()
        {
            var failures = new List<string>();
            foreach(var test in LoadTests<bool>("validate.yml", "usernames"))
            {
                try
                {
                    bool actual = validator.IsValidUsername(test.Text);
                    Assert.AreEqual(test.Expected, actual);
                }
                catch(Exception)
                {
                    failures.Add(test.Description + ": " + test.Text);
                }
            }

            if(failures.Any())
            {
                Assert.Fail(string.Join("\n", failures));
            }
        }

        private dynamic ConvertNode<T>(YamlNode node)
        {
            dynamic dynnode = node;

            if(node is YamlScalarNode)
            {
                if(string.IsNullOrEmpty(dynnode.Value))
                {
                    return null;
                }

                if(typeof(T) == typeof(int))
                {
                    return int.Parse(dynnode.Value);
                }

                if(typeof(T) == typeof(bool))
                {
                    return dynnode.Value == "true";
                }

                return dynnode.Value;
            }

            if(node is YamlSequenceNode)
            {
                dynamic list;
                if(typeof(T) == typeof(List<List<int>>))
                {
                    list = new List<List<int>>();
                    foreach(var item in dynnode.Children)
                    {
                        list.Add(ConvertNode<List<int>>(item));
                    }
                }
                else if(typeof(T) == typeof(List<int>))
                {
                    list = new List<int>();
                    foreach(var item in dynnode.Children)
                    {
                        list.Add(ConvertNode<int>(item));
                    }
                }
                else if(typeof(T) == typeof(List<string>))
                {
                    list = new List<string>();
                    foreach(var item in dynnode.Children)
                    {
                        list.Add(ConvertNode<string>(item));
                    }
                }
                else
                {
                    list = new List<dynamic>();
                    foreach(var item in dynnode.Children)
                    {
                        list.Add(ConvertNode<T>(item));
                    }
                }

                return list;
            }

            if(node is YamlMappingNode)
            {
                dynamic mapnode = new ExpandoObject();
                foreach(var item in ((YamlMappingNode)node).Children)
                {
                    var key = item.Key.ToString();
                    if(key == "indices")
                    {
                        ((IDictionary<string, object>)mapnode).Add(key, ConvertNode<int>(item.Value));
                    }
                    else
                    {
                        ((IDictionary<string, object>)mapnode).Add(key, ConvertNode<T>(item.Value));
                    }
                }

                return mapnode;
            }

            return null;
        }

        private IEnumerable<Test<TExpected>> LoadTests<TExpected>(string file, string section)
        {
            // load yaml file

            using(var stream = new StreamReader(Path.Combine(TestContext.CurrentContext.TestDirectory, "Twitter", "Conformance", file)))
            {
                var yaml = new YamlStream();
                yaml.Load(stream);
                // load specified test section
                var root = yaml.Documents[0].RootNode as YamlMappingNode;
                var tests = root.Children[new YamlScalarNode("tests")] as YamlMappingNode;
                var sect = tests.Children.Single(x => x.Key.ToString() == section);

                var items = sect.Value as YamlSequenceNode;
                foreach(YamlMappingNode item in items)
                {
                    // parse test
                    var test = new Test<TExpected>();
                    test.Description = ConvertNode<string>(item.Children.Single(x => x.Key.ToString() == "description").Value);
                    test.Text = ConvertNode<string>(item.Children.Single(x => x.Key.ToString() == "text").Value);
                    test.Expected = ConvertNode<TExpected>(item.Children.Single(x => x.Key.ToString() == "expected").Value);
                    test.Hits = ConvertNode<List<List<int>>>(item.Children.SingleOrDefault(x => x.Key.ToString() == "hits").Value);

                    // return test
                    yield return test;
                }
            }
        }
    }
}
