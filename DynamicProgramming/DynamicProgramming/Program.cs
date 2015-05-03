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
            var textreader = new StreamReader(typeof(Program).Assembly.GetManifestResourceStream("DynamicProgramming.tspTest.txt"));
            var result = new TSP().CalculateMinimalCost(textreader);
                //new APSP().CalculateMinimum(textreader);
            //var result = new KnapSack().CalculateKnapSackProblem(textreader);
            Console.WriteLine(result);
            Console.ReadLine();
        }
    }

    internal class TSP
    {
        public int CalculateMinimalCost(StreamReader textreader)
        {
            var count = int.Parse(textreader.ReadLine());
            var pathcount = (int)Math.Pow(2, count);
            var sw = new Stopwatch();
            sw.Start();
            var graph = LoadGraph(textreader);
            sw.Stop();
            Console.WriteLine("Loaded for: {0}", sw.Elapsed);
            sw.Restart();
            //var paths = new IEnumerable<int>[pathcount];
            var array = new float[pathcount, count+1];
            for (var i = 0; i < pathcount; i++)
            {
                for (var j = 0; j < count+1; j++)
                {
                    array[i, j] = int.MaxValue;
                }
            }
            array[1, 1] = 0;

            for (var m = 2; m < count+1; m++)
            {
                IterateSubsets(m, count, array, graph);
            }

            int c = (1 << count) - 1;
            float result=int.MaxValue;
            for (var i = 0; i < count; i++)
            {
                var setWithLastHop = array[c, i]+graph[Tuple.Create(i+1,1)];
                result = result > setWithLastHop ? setWithLastHop : result;
            }

            return (int)Math.Floor(result);
        }

        private void IterateSubsets(int m, int count, float[,] array, Dictionary<Tuple<int, int>, float> graph)
        {
            int c = (1 << m) - 1;
            while (c < (1 << count))
            {
                doWork(c, array, count, graph);
                int a = c & -c;
                int b = c + a;
                c = (c ^ b)/4/a | b;
            }
        }

        private void doWork(int c, float[,] array, int count, Dictionary<Tuple<int, int>, float> graph)
        {
            var ofS = GetElementsOfS(c, count).ToArray();
            foreach (var j in ofS)
            {
                if(j==0)
                    continue;
                var cwithoutj = c - (1 << j-1);
                var subsets = ofS.Where(k=>k!=j)
                    .Select(k => array[cwithoutj, k] + graph[Tuple.Create(k,j)]).ToArray();
                array[c,j] = subsets.Any()?subsets.Min() : int.MaxValue;
            }
        }

        private static IEnumerable<int> GetElementsOfS(int c, int count)
        {
            //if (c == 0)
            //yield return 0;
            for (var j = 1; j <= count; j++)
            {
                if ((c & 1) != 0)
                {
                    yield return j;
                }
                c = c >> 1;
            }
        }

        private Dictionary<Tuple<int, int>, float> LoadGraph(StreamReader textreader)
        {
            var cities = ReadArray(textreader).ToArray();
            var i = 1;
            var graph = new Dictionary<Tuple<int, int>, float>();
            foreach (var city in cities)
            {
                var edges = cities//.Where(oc => oc != city)
                        .Select((oc, j) => Tuple.Create(j+1, CalculateDistance(oc, city)))
                        .ToArray();
                foreach (var tuple in edges)
                {
                    graph.Add(Tuple.Create(i, tuple.Item1), tuple.Item2);
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
