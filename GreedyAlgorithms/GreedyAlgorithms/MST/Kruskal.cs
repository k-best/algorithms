using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace GreedyAlgorithms.MST
{
    public class Kruskal
    {
        public static Decimal Run(IEnumerable<string> lines)
        {
            var swAll = new Stopwatch();
            var sw = new Stopwatch();
            swAll.Start();
            sw.Start();
            var counts = lines.First().Split(' ', '\t');
            var vertexcount = int.Parse(counts[0]);
            var edgesCount = int.Parse(counts[1]);
            var edges = lines.Skip(1).Where(c => !String.IsNullOrWhiteSpace(c)).Select(c =>
            {
                var values = c.Split(' ', '\t');
                //Debug.Assert(values.Length == 3);
                return new
                {
                    From = Parse(values[0]),
                    To = Parse(values[1]),
                    Cost = DecimalParse(values[2])
                };
            }).ToArray();
            sw.Stop();
            Console.WriteLine("Загрузили строки в массив ребер за {0}", sw.Elapsed);
            sw.Restart();
            //var vertices = edges.Select(c => c.From).Concat(edges.Select(c => c.To)).Distinct();
            var unionFind = new UnionFind(vertexcount);
            for(int i=0; i<vertexcount; i++)
            {
                unionFind.Add(i);
            }
            sw.Stop();
            Console.WriteLine("Загрузили вершины в юнионфайнд за {0}", sw.Elapsed);
            //sw.Restart();
            //Array.Sort(edges, (x,y)=>x.Cost.CompareTo(y.Cost));
            //sw.Stop();
            //Console.WriteLine("сортировка массива ребер за {0}", sw.Elapsed);
            sw.Restart();
            var result = edges.OrderBy(c=>c.Cost).Where(edge => unionFind.Union(edge.From, edge.To)).Sum(edge => edge.Cost);
            sw.Stop();
            Console.WriteLine("вычисление mst за {0}", sw.Elapsed);
            swAll.Stop();
            Console.WriteLine("итого: {0}", result);
            Console.WriteLine("за время: {0}", swAll.Elapsed);
            return result;
        }

        private static int Parse(string s)
        {
            int value = 0;
            for (var i = 0; i < s.Length; i++)
            {
                value = value * 10 + (s[i] - '0');
            }
            return value;
        }

        private static decimal DecimalParse(string s)
        {
            decimal value = 0;
            for (var i = 2; i < s.Length; i++)
            {
                value = value * 10 + (s[i] - '0');
            }
            return value/100000000;
        }
    }
}