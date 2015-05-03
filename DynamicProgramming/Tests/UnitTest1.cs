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
            var graph = new Dictionary<Tuple<int, int>, float>
            {
                {Tuple.Create(1, 1), 0},
                {Tuple.Create(1, 2), 1},
                {Tuple.Create(1, 3), 4},
                {Tuple.Create(1, 4), 6},
                {Tuple.Create(2, 1), 1},
                {Tuple.Create(2, 2), 0},
                {Tuple.Create(2, 3), 2},
                {Tuple.Create(2, 4), 3},
                {Tuple.Create(3, 1), 4},
                {Tuple.Create(3, 2), 2},
                {Tuple.Create(3, 3), 0},
                {Tuple.Create(3, 4), 5},
                {Tuple.Create(4, 1), 6},
                {Tuple.Create(4, 2), 3},
                {Tuple.Create(4, 3), 5},
                {Tuple.Create(4, 4), 0}
            };



            var result = new TSP().CalculateMinimalCost(4, graph);
            Assert.AreEqual(13, result);
        }
    }
}
