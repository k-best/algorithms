using System.Collections.Generic;
using System.IO;
using System.Linq;
using GreedyAlgorithms;
using GreedyAlgorithms.Clustering;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class MaxClusteringTests
    {
        [TestMethod]
        public void TestCase1ShouldPass()
        {
            var textreader = new StreamReader(typeof(MaxSpacing).Assembly.GetManifestResourceStream("GreedyAlgorithms.clustering_big.txt"));
            var result = MaxClustering.Run(GetLines(textreader).Take(1000).ToArray(), 2);
            Assert.AreEqual(989, result);
        }

        [TestMethod]
        public void TestCase2ShouldPass()
        {
            var textreader = new StreamReader(typeof(MaxSpacing).Assembly.GetManifestResourceStream("GreedyAlgorithms.clustering_big.txt"));
            var result = MaxClustering.Run(GetLines(textreader).Take(10000).ToArray(), 2);
            Assert.AreEqual(9116, result);
        }

        [TestMethod]
        public void TestCase3ShouldPass()
        {
            var textreader = new StreamReader(typeof(MaxSpacing).Assembly.GetManifestResourceStream("GreedyAlgorithms.clustering_big.txt"));
            var result = MaxClustering.Run(GetLines(textreader).Take(100000).ToArray(), 2);
            Assert.AreEqual(22456, result);
        }

        [TestMethod]
        public void TestCase4ShouldPass()
        {
            var textreader = new StreamReader(typeof(MaxSpacing).Assembly.GetManifestResourceStream("GreedyAlgorithms.clustering_big.txt"));
            var result = MaxClustering.Run(GetLines(textreader).ToArray(), 2);
            Assert.AreEqual(6118, result);
        }

        private IEnumerable<string> GetLines(StreamReader textreader)
        {
            textreader.ReadLine();
            while (!textreader.EndOfStream)
            {
                yield return textreader.ReadLine();
            }
        }
    }
}