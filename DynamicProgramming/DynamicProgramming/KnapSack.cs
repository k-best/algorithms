using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace DynamicProgramming
{
    class KnapSack
    {
        private class TupleComparer:IEqualityComparer<Tuple<int, int>>
        {
            public bool Equals(Tuple<int, int> x, Tuple<int, int> y)
            {
                return x.Item1 == y.Item1 && x.Item2 == y.Item2;
            }

            public int GetHashCode(Tuple<int, int> obj)
            {
                return obj.GetHashCode();
            }
        }

        private Dictionary<Tuple<int, int> ,int> _cache = new Dictionary<Tuple<int, int>, int>(new TupleComparer());
        private Tuple<int, int>[] _data;

        public int CalculateKnapSackProblem(StreamReader textreader)
        {
            var counts = textreader.ReadLine().Split(' ', '\t');
            var maxSize = LoadHelper.Parse(counts[0]);
            var numberOfItems = LoadHelper.Parse(counts[1]);
            _data = ReadArray(textreader).Take(numberOfItems).OrderByDescending(c=>c.Item2).ToArray();

            var sw = new Stopwatch();
            sw.Start();
            var result = Calculate(numberOfItems-1, maxSize);
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            return result;
        }

        private int Calculate(int i, int x)
        {
            var indices = Tuple.Create(i, x);
            int result;
            if (_cache.TryGetValue(indices, out result))
                return result;
            if (i < 1)
                return 0;
            var ith = _data[i];
            if (x - ith.Item2 < 0)
                return 0;
            var val1 = Calculate(i - 1, x);
            var val2 = Calculate(i - 1, x - ith.Item2);
            result = Math.Max(val1, ith.Item1 + val2);
            _cache.Add(indices, result);
            return result;
        }

        public static IEnumerable<Tuple<int, int>> ReadArray(StreamReader textreader)
        {
            if (textreader == null)
                yield break;
            while (!textreader.EndOfStream)
            {
                var values = textreader.ReadLine().Split(' ', '\t');
                yield return Tuple.Create(LoadHelper.Parse(values[0]), LoadHelper.Parse(values[1]));
            }
        }
    }
}