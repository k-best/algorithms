using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using GreedyAlgorithms.MST;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Tests
{
    [TestClass]
    public class KruskalTests
    {
        [TestMethod]
        public void TestCase1ShouldPass()
        {
            var result = Kruskal.Run(File.ReadLines("c:\\temp\\homework2_1_2_test.txt"));
            Assert.AreEqual(-16, result);
        }
        
        [TestMethod]
        public void TestCase2ShouldPass()
        {
            var results = new List<decimal>();
            for (int i = 0; i < 100; i++)
            {
                var result = Kruskal.Run(File.ReadLines("c:\\temp\\homework2_1_2_test2.txt"));
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
                var result = Kruskal.Run(File.ReadLines("c:\\temp\\tinyEWG.txt"));
                Console.WriteLine("итого: {0}", result);
                results.Add(result);
            }
            var minresult = results.Min();
            Assert.AreEqual(new decimal(1.81), minresult);
        }

        [TestMethod]
        public void TestCase4ShouldPass()
        {
            //var textreader = new StreamReader(typeof(PrimTests).Assembly.GetManifestResourceStream("Tests.largeEWG.txt"));
            var result = Kruskal.Run(ReadLines("c:\\work\\largeEWG.txt"));//GetLines(textreader));
            Assert.AreEqual(new Decimal(647.66306955), result);
        }

        private IEnumerable<string> ReadLines(string file)
        {
            using (StreamReader sr = File.OpenText(file))
            {
                string s;
                while ((s = sr.ReadLine()) != null)
                {
                    yield return s;
                }
            }
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