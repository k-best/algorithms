using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreedyAlgorithms
{
    class Program
    {
        static void Main(string[] args)
        {
            //SchedulingAlgorithm.Run("c:\\temp\\homework2_1.txt", (w,l)=>w-l);
            var results = new List<decimal>();
            for (int i = 0; i < 100; i++)
            {
                var result = PMST.Run("c:\\temp\\homework2_1_2.txt");
                Console.WriteLine("итого: {0}", result);
                results.Add(result);
            }
            var minresult = results.Min();
            Console.WriteLine("итого: {0}", minresult);
            Console.ReadLine();
        }
    }

    public class PMST
    {
        public static decimal Run(string file)
        {
            var graph = LoadGraph(file);
            decimal mstLength = 0;
            var begin = graph.ExtractMinValue().Value;
            Console.WriteLine(begin.Label);
            RecalculateKeys(begin, graph);
            while (graph.Count>0)
            {
                var element = graph.ExtractMinValue();
                mstLength += element.Key;
                RecalculateKeys(element.Value, graph);
            }
            return mstLength;
        }

        private static void RecalculateKeys(Vertex begin, Heap<decimal, Vertex> graph)
        {
            foreach (var edge in begin.Edges)
            {
                KeyValuePair<decimal, Vertex> element;
                if(graph.TryDeleteValue(edge.SecondVertex, out element))
                {
                    var newKey = edge.Cost.CompareTo(element.Key) < 0 ? edge.Cost : element.Key;
                    graph.Insert(new KeyValuePair<decimal, Vertex>(newKey, element.Value));
                }
            }
        }

        private static Heap<decimal, Vertex> LoadGraph(string file)
        {
            var graph = new Heap<decimal, Vertex>();
            var edges = File.ReadAllLines(file).Skip(1).Select(c =>
            {
                var values = c.Split(' ', '\t');
                Debug.Assert(values.Length == 3);
                return new Edge
                (
                    int.Parse(values[0]),
                    int.Parse(values[1]),
                    decimal.Parse(values[2], NumberStyles.AllowDecimalPoint, new CultureInfo("en-US"))
                );
            }).ToArray();
            foreach (var edge in edges)
            {
                const int key = int.MaxValue; //new Edge(edge.FirstVertex.Label, edge.SecondVertex.Label, int.MaxValue);
                var firstVertex = graph.GetOrInsert(new KeyValuePair<decimal, Vertex>(key, edge.FirstVertex));
                if(firstVertex!=default(Vertex))
                    firstVertex.Edges.Add(new Edge(edge.FirstVertex.Label, edge.SecondVertex.Label, edge.Cost));
                var secondVertex = graph.GetOrInsert(new KeyValuePair<decimal, Vertex>(key, edge.SecondVertex));
                if (secondVertex != default(Vertex))
                    secondVertex.Edges.Add(new Edge(edge.SecondVertex.Label, edge.FirstVertex.Label, edge.Cost));
            }
            graph.RandomizeHead();
            return graph;
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
        private Edge() { }

        public Edge(int first, int second, decimal cost)
        {
            Cost = cost;
            FirstVertex = new Vertex{Label = first, Edges = new List<Edge>{this}};
            var invertedEdge = new Edge{Cost = cost, SecondVertex = FirstVertex};
            SecondVertex = new Vertex { Label = second, Edges = new List<Edge>{ invertedEdge } };
            invertedEdge.FirstVertex = SecondVertex;
        }

        public Vertex FirstVertex { get; set; }
        public Vertex SecondVertex { get; set; }
        public decimal Cost { get; set; }
        public int CompareTo(Edge other)
        {
            return Cost.CompareTo(other.Cost);
        }
    }

    public class Graph
    {
        public List<Vertex> Vertices { get; set; }
        public List<Edge> Edges { get; set; } 
    }
}
