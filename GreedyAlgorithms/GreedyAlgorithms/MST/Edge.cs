using System;

namespace GreedyAlgorithms.MST
{
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
}