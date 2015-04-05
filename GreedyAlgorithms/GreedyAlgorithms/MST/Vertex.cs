using System;
using System.Collections.Generic;

namespace GreedyAlgorithms.MST
{
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
}