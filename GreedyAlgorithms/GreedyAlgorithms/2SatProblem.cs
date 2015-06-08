using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Monads;

namespace GreedyAlgorithms
{
    public class _2SatProblem
    {
        public static bool IsSatisfiable(StreamReader textreader)
        {
            var count = textreader.ReadLine();
            var graph = LoadGraph(textreader);
            var sccResult = CountStrongConnectedRegions.CountStrongConnectedRegionsOfDirectedGraph(graph);
            return !sccResult.Any(c=>c>1);
        }

        private static Stack<Vertice> LoadGraph(StreamReader textreader)
        {
            var graph = new DirectedGraphLoader();
            while (!textreader.EndOfStream)
            {
                var array = textreader.ReadLine().Split(' ', '\t').Select(int.Parse).ToArray(); ;
                array.Check(c => c.Length == 2, c => new InvalidOperationException("invalid array length"));
                var vertice1 = array[0];
                var vertice2 = -array[1];
                graph.AddEdge(vertice1, vertice2);
                var vertice3 = -array[0];
                var vertice4 = array[1];
                graph.AddEdge(vertice4, vertice3);
            }

            //foreach (var trimmedLine in File.ReadAllLines(fileName).Select(line => line.Trim()).Where(trimmedLine => !string.IsNullOrWhiteSpace(trimmedLine)))
            //{
            //    var array = trimmedLine.Split('\t', ' ').Select(int.Parse).ToArray();
            //    array.Check(c => c.Length == 2, c => new InvalidOperationException("invalid array length"));
            //    var vertice1 = array[0];
            //    var vertice2 = -array[1];
            //    graph.AddEdge(vertice1, vertice2);
            //    var vertice3 = -array[0];
            //    var vertice4 = array[1];
            //    graph.AddEdge(vertice3, vertice4);
            //}
            return graph.GetVertices();
        }
    }

    class CountStrongConnectedRegions
    {
        public static void CountStrongConnectedRegionsOfDirectedGraph()
        {
            var graph = GetFromFile("e:\\temp\\homework4.txt");
            var result = CountStrongConnectedRegionsOfDirectedGraph(graph);

            foreach (var count in result)
            {
                Console.WriteLine(count);
            }
            Console.ReadLine();
        }

        public static int[] CountStrongConnectedRegionsOfDirectedGraph(Stack<Vertice> graph)
        {
            var intermediate = DepthFirstLoop(graph, c => c.ThisAvailableFromVertices);
            var result =
                DepthFirstLoop(intermediate.Item1, c => c.AvailabelVertices).Item2.OrderByDescending(c => c).Take(5).ToArray();
            return result;
        }

        private static Tuple<Stack<Vertice>, List<int>> DepthFirstLoop(Stack<Vertice> graph, Func<Vertice, Dictionary<int, Vertice>> getListOfEdges)
        {
            foreach (var vertex in graph)
            {
                vertex.State = State.None;
            }
            var orderedVertices = new Stack<Vertice>();
            var scRegionCounts = new List<int>();
            while (graph.Count > 0)
            {
                var vertex = graph.Pop();
                if (vertex.State == State.None)
                {
                    var scc = DepthFirstSearch(vertex, orderedVertices, getListOfEdges, 0);
                    for (var i = 0; i < scc.Count; i++)
                    {
                        for (var j = i; j < scc.Count-i; j++)
                        {
                            if(scc[i] == -scc[j])
                                scRegionCounts.Add(scc.Count);
                        }
                    }
                    //scRegionCounts.Add(scc);
                }
            }
            return Tuple.Create(orderedVertices, scRegionCounts);
        }

        private static List<int> DepthFirstSearch(Vertice start, Stack<Vertice> orderedVertices, Func<Vertice, Dictionary<int, Vertice>> getListOfEdges, int count)
        {
            var innerStack = new Stack<Vertice>();
            var scc = new List<int>();
            innerStack.Push(start);
            while (innerStack.Count > 0)
            {
                var vertice = innerStack.Pop();
                switch (vertice.State)
                {
                    default:
                        continue;
                    case State.None:
                        vertice.State = State.InWork;
                        innerStack.Push(vertice);
                        foreach (var listOfEdge in getListOfEdges(vertice))
                        {
                            if (listOfEdge.Value.State == State.None)
                                innerStack.Push(listOfEdge.Value);
                        }
                        break;
                    case State.InWork:
                        vertice.State = State.Done;
                        orderedVertices.Push(vertice);
                        count++;
                        scc.Add(vertice.Label);
                        break;
                }
            }
            return scc;
            //return count;
        }

        private static Stack<Vertice> GetFromFile(string filename)
        {
            var graph = new DirectedGraphLoader();
            foreach (var trimmedLine in File.ReadAllLines(filename).Select(line => line.Trim()).Where(trimmedLine => !string.IsNullOrWhiteSpace(trimmedLine)))
            {
                var array = trimmedLine.Split('\t', ' ').Select(int.Parse).ToArray();
                array.Check(c => c.Length == 2, c => new InvalidOperationException("invalid array length"));
                var vertice1 = array[0];
                var vertice2 = array[1];
                graph.AddEdge(vertice1, vertice2);
            }
            return graph.GetVertices();
        }
    }
    public class DirectedGraphLoader
    {
        private Dictionary<int, Vertice> _vertices = new Dictionary<int, Vertice>();

        public Dictionary<int, Vertice> Vertices
        {
            get { return _vertices; }
            set { _vertices = value; }
        }

        public void AddEdge(int tail, int head)
        {
            Vertice vertice1;
            if (!Vertices.TryGetValue(tail, out vertice1))
            {
                vertice1 = new Vertice() { Label = tail };
                Vertices.Add(vertice1.Label, vertice1);
            }
            Vertice vertice2;
            if (!Vertices.TryGetValue(head, out vertice2))
            {
                vertice2 = new Vertice { Label = head };
                Vertices.Add(vertice2.Label, vertice2);
            }
            vertice1.AvailabelVertices.Add(vertice2.Label, vertice2);
            vertice2.ThisAvailableFromVertices.Add(vertice1.Label, vertice1);
        }

        public Stack<Vertice> GetVertices()
        {
            var stack = new Stack<Vertice>();
            foreach (var vertex in Vertices)
            {
                stack.Push(vertex.Value);
            }
            return stack;
        }
    }

    public class Vertice
    {
        private Dictionary<int, Vertice> _availabelVertices = new Dictionary<int, Vertice>();
        private Dictionary<int, Vertice> _thisAvailableFromVertices = new Dictionary<int, Vertice>();

        public int Label { get; set; }

        public State State { get; set; }

        public Dictionary<int, Vertice> AvailabelVertices
        {
            get { return _availabelVertices; }
            set { _availabelVertices = value; }
        }

        public Dictionary<int, Vertice> ThisAvailableFromVertices
        {
            get { return _thisAvailableFromVertices; }
            set { _thisAvailableFromVertices = value; }
        }
    }

    public enum State
    {
        None,
        InWork,
        Done
    }
}
