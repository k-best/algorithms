using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;

namespace GreedyAlgorithms
{
    class Program
    {
        private static void Main(string[] args)
        {
            //SchedulingAlgorithm.Run("c:\\temp\\homework2_1.txt", (w,l)=>w-l);
            var result = PMST.Run("c:\\temp\\largeEWG.txt");
            Console.WriteLine("итого: {0}", result);
            Console.ReadLine();
        }
    }

    public class PMST
    {
        private static Dictionary<int, Vertex> _vertices;

        public static decimal Run(string file)
        {
            var sw = new Stopwatch();
            sw.Start();
            var graphAndVertices = LoadGraph(file);
            decimal mstLength = 0;
            var graph = graphAndVertices.Item1;
            _vertices = graphAndVertices.Item2;
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
            Console.WriteLine("итого: {0}", mstLength);
            Console.WriteLine("за время: {0}", sw.Elapsed);
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

        private static Tuple<Heap<decimal, int>, Dictionary<int, Vertex>> LoadGraph(string file)
        {
            var graph = new Heap<decimal, int>();
            var sw = new Stopwatch();
            sw.Start();
            var lines = File.ReadAllLines(file);
            var counts = lines[0].Split(' ', '\t');
            //var vertexcount = int.Parse(counts[0]);
            //var edgesCount = int.Parse(counts[1]);
            var edges = lines.Skip(1).Where(c=>!String.IsNullOrWhiteSpace(c)).Select(c =>
            {
                var values = c.Split(' ', '\t');
                Debug.Assert(values.Length == 3);
                return new LoadEdge
                (
                    int.Parse(values[0]),
                    int.Parse(values[1]),
                    decimal.Parse(values[2], NumberStyles.AllowDecimalPoint, new CultureInfo("en-US"))
                );
            }).ToArray();
            sw.Stop();
            Console.WriteLine("загрузка:{0}",sw.Elapsed);
            var vertices = new Dictionary<int, Vertex>();
            foreach (var edge in edges)
            {
                TryAddVertex(vertices, edge.From, edge.To, edge.Cost);
                TryAddVertex(vertices, edge.To, edge.From, edge.Cost);
            }
            //Debug.Assert(vertices.Count==vertexcount);
            
            foreach (var vertex in vertices)
            {
                graph.Insert(new KeyValuePair<decimal, int>(decimal.MaxValue-100, vertex.Key));
            }
            graph.RandomizeHead();
            return Tuple.Create(graph, vertices);
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
    }

    public class Vertex : IEquatable<Vertex>
    {
        public int Label { get; set; }
        public List<Edge> Edges { get; set; }

        public bool Equals(Vertex other)
        {
            return Label == other.Label;
        }

        public override int GetHashCode()
        {
            return Label;
        }
    }

    public class Edge : IComparable<Edge>
    {
        public Edge(int to, decimal cost)
        {
            Cost = cost;
            To = to;
        }

        public int To { get; set; }
        public decimal Cost { get; set; }
        public int CompareTo(Edge other)
        {
            return Cost.CompareTo(other.Cost);
        }
    }

    public class LoadEdge
    {
        public LoadEdge(int first, int second, decimal cost)
        {
            Cost = cost;
            From = first;
            To = second;
        }

        public int From { get; set; }
        public int To { get; set; }
        public decimal Cost { get; set; }
    }
}
