using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GreedyAlgorithms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class MaxSpacingTests
    {
        [TestMethod]
        public void TestCase1ShouldPass()
        {
            var textreader = new StreamReader(typeof(MaxSpacing).Assembly.GetManifestResourceStream("GreedyAlgorithms.clustering_test1.txt"));
            var result = MaxSpacing.Run(GetLines(textreader),2);
            Assert.AreEqual(6, result);
        }

        [TestMethod]
        public void TestCase1_2ShouldPass()
        {
            var textreader = new StreamReader(typeof(MaxSpacing).Assembly.GetManifestResourceStream("GreedyAlgorithms.clustering_test2.txt"));
            var result = MaxSpacing.Run(GetLines(textreader), 2);
            Assert.AreEqual(4472, result);
        }

        [TestMethod]
        public void TestCase2ShouldPass()
        {
            var textreader = new StreamReader(typeof(MaxSpacing).Assembly.GetManifestResourceStream("GreedyAlgorithms.clustering1.txt"));
            var result = MaxSpacing.Run(GetLines(textreader).ToArray(), 4);
            Assert.AreEqual(106, result);
        }

        private IEnumerable<string> GetLines(StreamReader textreader)
        {
            while (!textreader.EndOfStream)
            {
                yield return textreader.ReadLine();
            }
        }
    }
}
