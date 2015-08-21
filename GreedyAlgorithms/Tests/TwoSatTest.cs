using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GreedyAlgorithms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class TwoSatTest
    {
        [TestMethod]
        public void TestCase1ShouldPass()
        {
            var textreader = new StreamReader(typeof(_2SatProblem).Assembly.GetManifestResourceStream("GreedyAlgorithms.2SetTest2.txt"));
            var result = _2SatProblem.IsSatisfiable(textreader);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestCase2ShouldPass()
        {
            var textreader = new StreamReader(typeof(_2SatProblem).Assembly.GetManifestResourceStream("GreedyAlgorithms.2SetTest.txt"));
            var result = _2SatProblem.IsSatisfiable(textreader);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestCase3ShouldPass()
        {
            var textreader = new StreamReader(typeof(_2SatProblem).Assembly.GetManifestResourceStream("GreedyAlgorithms.2SetTest1.txt"));
            var result = _2SatProblem.IsSatisfiable(textreader);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestCase4ShouldPass()
        {
            var textreader = new StreamReader(typeof(_2SatProblem).Assembly.GetManifestResourceStream("GreedyAlgorithms.2SetTest3.txt"));
            var result = _2SatProblem.IsSatisfiable(textreader);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestCase5ShouldPass()
        {
            var textreader = new StreamReader(typeof(_2SatProblem).Assembly.GetManifestResourceStream("GreedyAlgorithms.2SetTest4.txt"));
            var result = _2SatProblem.IsSatisfiable(textreader);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestCase6ShouldPass()
        {
            var textreader = new StreamReader(typeof(_2SatProblem).Assembly.GetManifestResourceStream("GreedyAlgorithms.2SetTest5.txt"));
            var result = _2SatProblem.IsSatisfiable(textreader);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestCase7ShouldPass()
        {
            var textreader = new StreamReader(typeof(_2SatProblem).Assembly.GetManifestResourceStream("GreedyAlgorithms.2SetTest6.txt"));
            var result = _2SatProblem.IsSatisfiable(textreader);
            Assert.IsTrue(result);
        }
    }
}
