using System;
using System.Collections.Generic;
using System.Linq;

namespace GreedyAlgorithms
{
    public static class MaxSpacing
    {
        public static int Run(IEnumerable<string> lines, int clustersCount)
        {
            var counts = lines.First().Split(' ', '\t');
            var vertexcount = int.Parse(counts[0]);
            var edges = lines.Skip(1).Where(c => !String.IsNullOrWhiteSpace(c)).Select(c =>
            {
                var values = c.Split(' ', '\t');
                //Debug.Assert(values.Length == 3);
                return new
                {
                    From = LoadHelper.Parse(values[0]),
                    To = LoadHelper.Parse(values[1]),
                    Cost = LoadHelper.Parse(values[2])
                };
            }).ToArray();

            var unionFind = new UnionFind(vertexcount+1);
            for (int i = 1; i <= vertexcount; i++)
            {
                unionFind.Add(i);
            }

            var orderedEdges = edges.OrderBy(c => c.Cost);
            var enumerator = orderedEdges.GetEnumerator();
            enumerator.MoveNext();
            do
            {
                unionFind.Union(enumerator.Current.From, enumerator.Current.To);
            } 
            while (unionFind.RootCount >= clustersCount && enumerator.MoveNext());
            return enumerator.Current.Cost;
        }
    }
}