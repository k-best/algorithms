using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicProgramming
{
    class Program
    {
        static void Main(string[] args)
        {
            var textreader = new StreamReader(typeof(Program).Assembly.GetManifestResourceStream("DynamicProgramming.tsp.txt"));
            var result = new TSP().CalculateMinimalCost(textreader);
                //new APSP().CalculateMinimum(textreader);
            //var result = new KnapSack().CalculateKnapSackProblem(textreader);
            Console.WriteLine(result);
            Console.ReadLine();
        }
    }

    public class TSP
    {
        //private Dictionary<Tuple<int, int>, float> cache;
        private Dictionary<int, float[]> _graph;

        public int CalculateMinimalCost(StreamReader textreader)
        {
            var count = int.Parse(textreader.ReadLine());
            
            var sw = new Stopwatch();
            sw.Start();
            _graph = LoadGraph(textreader, count);
            sw.Stop();
            Console.WriteLine("Loaded for: {0}", sw.Elapsed);
            sw.Restart();
            
            return CalculateMinimalCost(count, _graph);
        }

        public int CalculateMinimalCost(int count, Dictionary<int, float[]> graph)
        {
            this._graph = graph;
            var cache = new Dictionary<int, float[]>();
            cache.Add(0, new float[] {0});

            for (var m = 1; m < count; m++)
            {
                cache = IterateSubsets(m, count, cache);
            }

            int c = (1 << count-1) - 1;
            float result = int.MaxValue;
            for (var i = 0; i <= count-1; i++)
            {
                var setWithLastHop = cache[c][i] + graph[i][0];
                result = result > setWithLastHop ? setWithLastHop : result;
            }

            return (int) Math.Floor(result);
        }

        private Dictionary<int, float[]> IterateSubsets(int m, int count, Dictionary<int, float[]> cache)
        {
            var sw = new Stopwatch();
            sw.Start();
            int c = (1 << m) - 1;
            var newCache = new Dictionary<int, float[]>();
            while (c < (1 << count-1))
            {
                doWork(c, count, cache, newCache, m);
                int a = c & -c;
                int b = c + a;
                c = (c ^ b)/4/a | b;
            }
            sw.Stop();
            Console.WriteLine("m={0}; t={1}", m, sw.Elapsed);
            return newCache;
        }

        private void doWork(int c, int count, Dictionary<int, float[]> cache, Dictionary<int, float[]> newCache, int m)
        {
            var ofS = GetElementsOfS(c, count).ToArray();
            float[] array = new float[count];
            for (int f = count - 1; f >= 0; f--)
            {
                array[f] = int.MaxValue;
            }
            newCache.Add(c, array);
            foreach (var j in ofS)
            {
                if(j==0)
                    continue;
                var cwithoutj = c - (1 << j-1);
                var subsets = GetSubsetLengths(cache, ofS, j, cwithoutj).ToArray();
                if (subsets.Any())
                    array[j] = subsets.Min();
            }
        }

        private IEnumerable<float> GetSubsetLengths(Dictionary<int, float[]> cache, int[] ofS, int j, int cwithoutj)
        {
            foreach (var k in ofS)
            {
                if (k == j) continue;
                var lbefore = cache[cwithoutj][k];//, out lbefore))
                    yield return lbefore+_graph[k][j];
            }
        }

        private static float GetValueFromCache(Dictionary<Tuple<int, int>, float> cache, int cwithoutj, int k)
        {
            float result;
            return cache.TryGetValue(Tuple.Create(cwithoutj, k), out result) ? result : int.MaxValue;
        }

        private static IEnumerable<int> GetElementsOfS(int c, int count)
        {
            //if (c == 0)
            yield return 0;
            for (var j = 1; j < count; j++)
            {
                if ((c & 1) != 0)
                {
                    yield return j;
                }
                c = c >> 1;
            }
        }

        private Dictionary<int, float[]> LoadGraph(StreamReader textreader, int count)
        {
            var cities = ReadArray(textreader).ToArray();
            var i = 0;
            var graph = new Dictionary<int, float[]>();
            foreach (var city in cities)
            {
                var array = new float[count];
                graph.Add(i,array);
                var edges = cities//.Where(oc => oc != city)
                        .Select((oc, j) => Tuple.Create(j, CalculateDistance(oc, city)))
                        .ToArray();
                foreach (var tuple in edges)
                {
                    array[tuple.Item1] = tuple.Item2;
                }
                i++;
            }
            return graph;
        }

        private float CalculateDistance(Point city, Point otherCity)
        {
            return (float)Math.Sqrt(Math.Pow(city.X - otherCity.X, 2) + Math.Pow(city.Y - otherCity.Y, 2));
        }

        public static IEnumerable<Point> ReadArray(StreamReader textreader)
        {
            if (textreader == null)
                yield break;
            while (!textreader.EndOfStream)
            {
                var values = textreader.ReadLine().Split(' ', '\t');
                yield return new Point(float.Parse(values[0]), float.Parse(values[1]));
            }
        }

        public class Point
        {
            public float X;
            public float Y;

            public Point(float x, float y)
            {
                X = x;
                Y = y;
            }
        }
    }
}
