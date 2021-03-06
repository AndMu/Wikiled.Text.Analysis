﻿using System.Linq;
using NUnit.Framework;
using Wikiled.Text.Analysis.Tokenizer.Pipelined;

namespace Wikiled.Text.Analysis.Tests.Tokenizer.Pipelined
{
    [TestFixture]
    public class PunctuationPipelineTests
    {
        [Test]
        public void Process()
        {
            var result = new PunctuationPipeline().Process(new[] { "father's", "day", "(really),", "$day", "#xxx" }).ToArray();
            Assert.AreEqual(8, result.Length);
            Assert.AreEqual("father's", result[0]);
            Assert.AreEqual("day", result[1]);
            Assert.AreEqual("(", result[2]);
            Assert.AreEqual("really", result[3]);
            Assert.AreEqual(")", result[4]);
            Assert.AreEqual(",", result[5]);
            Assert.AreEqual("$day", result[6]);
            Assert.AreEqual("#xxx", result[7]);
        }
    }
}
