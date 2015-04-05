using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;

namespace GreedyAlgorithms.MST
{
    public class Prim
    {
        private static Dictionary<int, Vertex> _vertices;

        public static decimal Run(IEnumerable<string> lines)
        {
            var swAll = new Stopwatch();
            swAll.Start();
            var graphAndVertices = LoadGraph(lines);
            decimal mstLength = 0;
            var graph = graphAndVertices.Item1;
            _vertices = graphAndVertices.Item2;
            var sw = new Stopwatch();
            sw.Start();
            var begin = graph.ExtractMinValue().Value;
            Console.WriteLine(begin);
            RecalculateKeys(begin, graph);
            while (graph.Count>0)
            {
                var element = graph.ExtractMinValue();
                mstLength += element.Key;
                RecalculateKeys(element.Value, graph);
            }
            sw.Stop();
            swAll.Stop();
            Console.WriteLine("мст рассчитано за {0}", sw.Elapsed);
            Console.WriteLine("итого: {0}", mstLength);
            Console.WriteLine("за врем€: {0}", swAll.Elapsed);
            return mstLength;
        }

        private static void RecalculateKeys(int beginLabel, Heap<decimal, int> graph)
        {
            var begin = _vertices[beginLabel];
            foreach (var edge in begin.Edges)
            {
                KeyValuePair<decimal, int> element;
                if (graph.TryDeleteValue(edge.To, out element))
                {
                    var newKey = edge.Cost < element.Key ? edge.Cost : element.Key;
                    graph.Insert(new KeyValuePair<decimal, int>(newKey, element.Value));
                }
            }
        }

        private static Tuple<Heap<decimal, int>, Dictionary<int, Vertex>> LoadGraph(IEnumerable<string> lines)
        {
            var graph = new Heap<decimal, int>();
            //var counts = lines.First().Split(' ', '\t');
            //var vertexcount = int.Parse(counts[0]);
            //var edgesCount = int.Parse(counts[1]);
            var sw2 = new Stopwatch();
            sw2.Start();
            var edges = lines.Skip(1).Where(c=>!String.IsNullOrWhiteSpace(c)).Select(c =>
            {
                var values = c.Split(' ', '\t');
                //Debug.Assert(values.Length == 3);
                return new {
                    From = Parse(values[0]),
                    To = Parse(values[1]),
                    Cost = DecimalParse(values[2])
                    };
            }).ToArray();
            sw2.Stop();
            Console.WriteLine("«агрузили строки в массив ребер за {0}", sw2.Elapsed);
            var sw = new Stopwatch();
            sw.Start();
            var vertices = new Dictionary<int, Vertex>();
            foreach (var edge in edges)
            {
                TryAddVertex(vertices, edge.From, edge.To, edge.Cost);
                TryAddVertex(vertices, edge.To, edge.From, edge.Cost);
            }
            //Debug.Assert(vertices.Count==vertexcount);

            sw.Stop();
            Console.WriteLine("сформировали список вершин с исход€щими из них ребрами за {0}", sw.Elapsed);
            sw.Restart();
            foreach (var vertex in vertices)
            {
                graph.Insert(new KeyValuePair<decimal, int>(decimal.MaxValue-100, vertex.Key));
            }
            graph.RandomizeHead();
            sw.Stop();
            Console.WriteLine("загрузили граф в кучу за {0}", sw.Elapsed);
            return Tuple.Create(graph, vertices);
        }

        private static IEnumerable<string> ReadFromFile(string file)
        {
            return File.ReadLines(file);
        }

        private static IEnumerable<string> ReadFromFile(StreamReader file)
        {
            while (!file.EndOfStream)
            {
                yield return file.ReadLine();
            }
        }

        private static void TryAddVertex(Dictionary<int, Vertex> vertices, int from, int to, decimal cost)
        {
            var edge = new Edge(to, cost);
            Vertex vertex1;

            if (vertices.TryGetValue(from, out vertex1))
                vertex1.Edges.Add(edge);
            else
                vertices.Add(from, new Vertex { Edges = new[] { edge }.ToList() });
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
            return value / 100000000;
        }
    }
}