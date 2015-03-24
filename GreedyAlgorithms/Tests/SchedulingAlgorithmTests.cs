using System;
using GreedyAlgorithms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class SchedulingAlgorithmTests
    {
        [TestMethod]
        public void TestCasesShouldPassForRatio()
        {
            var result = SchedulingAlgorithm.Run("c:\\temp\\homework2_1_test1.txt", (w,l)=>w/l);
            Assert.AreEqual(674634, result);
        }

        [TestMethod]
        public void TestCasesShouldPassForDifference()
        {
            var result = SchedulingAlgorithm.Run("c:\\temp\\homework2_1_test1.txt", (w, l) => w - l);
            Assert.AreEqual(688647, result);
        }
    }
}
