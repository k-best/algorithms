using System;
using System.Collections.Generic;
using System.Linq;
using GreedyAlgorithms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class PMSTTests
    {
        [TestMethod]
        public void TestCase1ShouldPass()
        {
            var result = PMST.Run("c:\\temp\\homework2_1_2_test.txt");
            Assert.AreEqual(-16, result);
        }
        
        [TestMethod]
        public void TestCase2ShouldPass()
        {
            var results = new List<decimal>();
            for (int i = 0; i < 100; i++)
            {
                var result = PMST.Run("c:\\temp\\homework2_1_2_test2.txt");
                Console.WriteLine("итого: {0}", result);
                results.Add(result);
            }
            var minresult = results.Min();
            Assert.AreEqual(new Decimal(10.46351), minresult);
        }

        [TestMethod]
        public void TestCase3ShouldPass()
        {
            var results = new List<decimal>();
            for (int i = 0; i < 100; i++)
            {
                var result = PMST.Run("c:\\temp\\tinyEWG.txt");
                Console.WriteLine("итого: {0}", result);
                results.Add(result);
            }
            var minresult = results.Min();
            Assert.AreEqual(new decimal(1.81), minresult);
        }
    }
}