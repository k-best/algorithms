using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GreedyAlgorithms.Clustering
{
    public static class MaxClustering
    {
        private static Dictionary<int, List<int>> _distances;

        static MaxClustering()
        {
            _distances = new Dictionary<int, List<int>>();
            GenerateHummingDistanceMasks();
        }

        private static void GenerateHummingDistanceMasks()
        {
            _distances.Add(0, new List<int> {0});
            var listOf1 = new List<int>();
            _distances.Add(1, listOf1);
            var listOf2 = new List<int>();
            _distances.Add(2, listOf2);
            for (var i = 0; i < 24; i++)
            {
                var value = (int)Math.Pow(2,i);
                listOf1.Add(value);
                for (var j = i; j < 24; j++)
                {
                    if (i != j)
                    {
                        listOf2.Add(value+(int)Math.Pow(2,j));
                    }
                }
            }
        }

        public static int Run(IEnumerable<string> lines, int minSpacingDistance)
        {
            var sw = new Stopwatch();
            var nodes = lines.Where(c => !String.IsNullOrWhiteSpace(c))
                .Select(c => LoadHelper.ParseBits(c)).Distinct().Select((c, i) => new Node() { Label = i, Value = c }).ToDictionary(c => c.Value, c => c);//, });

            
            sw.Start();
            var links = nodes.Values.SelectMany(node => _distances[1],
                (node, distance) =>
                {
                    Node node1;
                    return nodes.TryGetValue(node.Value ^ distance, out node1) ? new Link {From = node.Label, Distance = 1, To = node1.Label} : null;
                })
                .Concat(nodes.Values.SelectMany(node => _distances[2],
                    (node, distance) =>
                    {
                        Node node1;
                        return nodes.TryGetValue(node.Value ^ distance, out node1)
                            ? new Link {From = node.Label, Distance = 2, To = node1.Label}
                            : null;
                    }))
                .Where(c=>c!=null)
                .OrderBy(c => c.Distance);
            var unionFind = new UnionFind(nodes.Count);
            foreach (var node in nodes.Values)
            {
                unionFind.Add(node.Label);
            }
            foreach (var link in links)
            {
                unionFind.Union(link.From, link.To);
            }
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            
            return unionFind.RootCount;
        }

        public class Link
        {
            public int From { get; set; }
            public int To { get; set; }
            public int Distance { get; set; }
        }
        
        public class Node
        {
            public int Label { get; set; }
            public int Value { get; set; }
            public int DistanceFromOne { get; set; }
            public override int GetHashCode()
            {
                return Label;
            }

            public override bool Equals(object obj)
            {
                var otherNode = obj as Node;
                return otherNode != null && Label.Equals(otherNode.Label);
            }
        }
    }
}