using System;
using System.Collections.Generic;
using System.IO;
using DynamicProgramming;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class TSPTest
    {
        [TestMethod]
        public void TestCase1ShouldPass()
        {
            var textreader = new StreamReader(typeof(TSP).Assembly.GetManifestResourceStream("DynamicProgramming.tspTest.txt"));
            var result = new TSP().CalculateMinimalCost(textreader);

            Assert.AreEqual(4, result);
        }

        [TestMethod]
        public void TestCase2ShouldPass()
        {
            var textreader = new StreamReader(typeof(TSP).Assembly.GetManifestResourceStream("DynamicProgramming.tspTest2.txt"));
            var result = new TSP().CalculateMinimalCost(textreader);

            Assert.AreEqual(12, result);
        }

        [TestMethod]
        public void TestCase3ShouldPass()
        {
            var textreader = new StreamReader(typeof(TSP).Assembly.GetManifestResourceStream("DynamicProgramming.tspTest3.txt"));
            var result = new TSP().CalculateMinimalCost(textreader);

            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public void TestWithNotGeometricGraphShouldPass()
        {
            var graph = new Dictionary<int, float[]>
            {
                {0, new float[] {0, 1, 4, 6}},
                {1, new float[] {1,0,2,3} },
                {2, new float[] {4,2,0,5} },
                {3, new float[] {6,4,5,0} }
            };


            var result = new TSP().CalculateMinimalCost(4, graph);
            Assert.AreEqual(13, result);
        }
    }
}
