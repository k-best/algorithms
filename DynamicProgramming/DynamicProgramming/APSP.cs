using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DynamicProgramming
{
    internal class APSP
    {
        public int CalculateMinimum(StreamReader textreader)
        {
            var counts = textreader.ReadLine().Split(' ', '\t').Select(c=> LoadHelper.Parse(c)).ToArray();
            var edges = ReadArray(textreader);
            var vertices = edges.GroupBy(c => c.Item1, c => Tuple.Create(c.Item2, c.Item3))
                .ToDictionary(c => c.Key, c => c.ToArray());
            var aprevious = new int[counts[0], counts[0]];
            var anext = new int[counts[0], counts[0]];
            FillAWithIntMax(aprevious, counts[0]);
            FillBaseCase(counts, vertices, aprevious);
            for (int k = 0; k < counts[0]; k++)
            {
                for (int i = 0; i < counts[0]; i++)
                {
                    for (int j = 0; j < counts[0]; j++)
                    {
                        var i1 = aprevious[i, k];
                        var i2 = aprevious[k, j];
                        long val2 = (i1 + i2);
                        if (val2 > int.MaxValue)
                            val2 = int.MaxValue;
                        var val1 = aprevious[i, j];
                        if (val1 > int.MaxValue)
                            val1 = int.MaxValue;
                        anext[i, j] = Math.Min(val1, (int)val2);
                    }
                }
                var atemp = aprevious;
                aprevious = anext;
                anext = atemp;
            }

            CheckNegativeCycle(aprevious, counts[0]);

            var result = GetPairs(aprevious, counts[0]).ToDictionary(c => c.Key, c => c.Value).Min(c => c.Value);

            return result;
        }

        private IEnumerable<KeyValuePair<Tuple<int, int>, int>> GetPairs(int[,] a, int count)
        {
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    yield return new KeyValuePair<Tuple<int, int>, int>(Tuple.Create(i,j), a[i, j]);
                }
            }
        }

        private void CheckNegativeCycle(int[,] a, int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (a[i, i] < 0)
                    throw new InvalidOperationException("negative cycle");
            }
        }

        private static void FillBaseCase(int[] counts, Dictionary<int, Tuple<int, int>[]> vertices, int[,] a)
        {
            foreach (var vertice in vertices)
            {
                var edgecount = vertice.Value.Length;
                for (var j = 0; j < edgecount; j++)
                {
                    var targetEdge = vertice.Value[j];
                    a[vertice.Key, targetEdge.Item1] = targetEdge.Item2;
                }
                a[vertice.Key, vertice.Key] = 0;
            }
        }

        private void FillAWithIntMax(int[,] a, int count)
        {
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    a[i, j] = 1000000;
                }
            }
        }


        public static IEnumerable<Tuple<int, int, int>> ReadArray(StreamReader textreader)
        {
            if (textreader == null)
                yield break;
            while (!textreader.EndOfStream)
            {
                var values = textreader.ReadLine().Split(' ', '\t');
                yield return Tuple.Create(
                    int.Parse(values[0])-1,
                    int.Parse(values[1])-1,
                    int.Parse(values[2])
                    );
            }
        }
    }
}