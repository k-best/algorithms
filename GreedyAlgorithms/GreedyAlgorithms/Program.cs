using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GreedyAlgorithms
{
    class Program
    {
        private static void Main(string[] args)
        {
            //SchedulingAlgorithm.Run("c:\\temp\\homework2_1.txt", (w, l) => w - l);
            //var result = MST.Kruskal.Run(File.ReadLines("c:\\work\\largeEWG.txt"));
            //var textreader = new StreamReader(typeof(Program).Assembly.GetManifestResourceStream("GreedyAlgorithms.2sat1.txt"));
            //var result = _2SatProblem.IsSatisfiable(textreader);
            //textreader.Dispose();
            //Console.WriteLine("result: {0}", result ? "Has solution" : "No Solution");

            //textreader = new StreamReader(typeof(Program).Assembly.GetManifestResourceStream("GreedyAlgorithms.2sat2.txt"));
            //result = _2SatProblem.IsSatisfiable(textreader);
            //textreader.Dispose();
            //Console.WriteLine("result: {0}", result ? "Has solution" : "No Solution");

            //textreader = new StreamReader(typeof(Program).Assembly.GetManifestResourceStream("GreedyAlgorithms.2sat3.txt"));
            //result = _2SatProblem.IsSatisfiable(textreader);
            //textreader.Dispose();
            //Console.WriteLine("result: {0}", result ? "Has solution" : "No Solution");

            //textreader = new StreamReader(typeof(Program).Assembly.GetManifestResourceStream("GreedyAlgorithms.2sat4.txt"));
            //result = _2SatProblem.IsSatisfiable(textreader);
            //textreader.Dispose();
            //Console.WriteLine("result: {0}", result ? "Has solution" : "No Solution");

            //textreader = new StreamReader(typeof(Program).Assembly.GetManifestResourceStream("GreedyAlgorithms.2sat5.txt"));
            //result = _2SatProblem.IsSatisfiable(textreader);
            //textreader.Dispose();
            //Console.WriteLine("result: {0}", result ? "Has solution" : "No Solution");

            //textreader = new StreamReader(typeof(Program).Assembly.GetManifestResourceStream("GreedyAlgorithms.2sat6.txt"));
            //result = _2SatProblem.IsSatisfiable(textreader);
            //textreader.Dispose();
            //Console.WriteLine("result: {0}", result ? "Has solution" : "No Solution");
            var huffmanCodes = new HuffmanCodes().CalculateCodes();
            foreach (var huffmanCode in huffmanCodes)
            {
                Console.WriteLine("{0} : {1}", huffmanCode.Key, huffmanCode.Value);
            }
            Console.ReadLine();
        }
    }

    public class HuffmanCodes
    {
        private Dictionary<decimal, char> _alphabeth = new Dictionary<decimal, char> { { 0.28m, 'a' }, { 0.27m, 'b' }, { 0.2m, 'c' }, { 0.15m, 'd' }, { 0.1m, 'e' }, };

        public Dictionary<char, string> CalculateCodes()
        {
            var heap = new Heap<decimal, BinaryTreeNode>();
            foreach (var letter in _alphabeth)
            {
                heap.Insert(new KeyValuePair<decimal, BinaryTreeNode>(letter.Key, new BinaryTreeNode {Frequency = letter.Key, Symbol = letter.Value}));
            }
            while (heap.Count>1)
            {
                var right = heap.ExtractMinValue();
                var left = heap.ExtractMinValue();
                var parent = new BinaryTreeNode {Frequency = left.Key+right.Key, Left = left.Value, Right = right.Value};
                left.Value.Parent = parent;
                right.Value.Parent = parent;
                heap.Insert(new KeyValuePair<decimal, BinaryTreeNode>(parent.Frequency, parent));
            }
            var resultTree = heap.ExtractMinValue().Value;
            var result = new Dictionary<char, string>();
            GetChildsCode(resultTree, result, string.Empty);
            return result;
        }

        public void GetChildsCode(BinaryTreeNode node, Dictionary<char, string> resultDictionary, string prefix)
        {
            if (node.Symbol.HasValue)
            {
                resultDictionary.Add(node.Symbol.Value, prefix);
                return;
            }
            GetChildsCode(node.Left, resultDictionary, prefix+"0");
            GetChildsCode(node.Right, resultDictionary, prefix+"1");
        }

    }


    public class BinaryTreeNode 
    {
        public BinaryTreeNode Left { get; set; }
        public BinaryTreeNode Right { get; set; }
        public BinaryTreeNode Parent { get; set; }
        public decimal Frequency { get; set; }
        public char? Symbol { get; set; }
    }
}
