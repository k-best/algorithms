using System;
using System.Collections.Generic;

namespace GreedyAlgorithms
{
    public class UnionFind<T>
    {
        private readonly Dictionary<T, Element<T>> _elements;

        public UnionFind(Dictionary<T, Element<T>> elements)
        {
            _elements = elements;
        }

        public UnionFind(int capacity = 0, IEqualityComparer<T> comparer = null)
        {
            _elements = new Dictionary<T, Element<T>>(capacity, comparer);
        }

        public void Add(T value)
        {
            _elements.Add(value, new Element<T>(value));
        }

        public Element<T> Find(T elementValue)
        {
            var root = _elements[elementValue].Root;
            while (root.Root!=root)
            {
                root = root.Root;
            }
            return root;
        }

        public bool Union(T elementValue1, T elementValue2)
        {
            var root1 = Find(elementValue1);
            var root2 = Find(elementValue2);
            if (root1 == root2)
                return false;
            if (root1.Rank > root2.Rank)
            {
                root2.Root=root1;
                root1.Rank++;
            }
            else
            {
                root1.Root = root2;
                root2.Rank++;
            }
            return true;
        }

        private static void ChangeRoot(Element<T> @from, Element<T> to)
        {
            foreach (var child in from.Childs)
            {
                to.Childs.Add(child);
                child.Root = to;
            }
        }
    }

    public class Element<T>
    {
        public Element(T value)
        {
            Value = value;
            Root = this;
            Childs = new List<Element<T>>() {this}; 
            Rank = 0;
        }

        public Element<T> Root { get; set; }

        public List<Element<T>> Childs { get; set; }

        public T Value { get; set; }

        public int Rank { get; set; }

        public int ChildsCount { get { return Childs.Count; } }
    }

    public class Root<T> : IEquatable<Root<T>>
    {
        public int ChildsCount { get; set; }

        public List<Element<T>> Childs { get; set; }

        public T Value { get; set; }


        public bool Equals(Root<T> other)
        {
            return Value.Equals(other.Value);
        }
    }

    public class UnionFind
    {
        private readonly int[] _roots;
        private readonly int[] _rootRanks;

        public UnionFind(int[] elements)
        {
            _roots = new int[elements.Length];
            _rootRanks = new int[elements.Length];
            foreach (var element in elements)
            {
                _roots[element] = element;
                _rootRanks[element] = 1;
            }
        }

        public UnionFind(int capacity)
        {
            _roots = new int[capacity];
            _rootRanks = new int[capacity];
        }

        public void Add(int value)
        {
            _roots[value] = value;
            _rootRanks[value] = 1;
        }

        public Tuple<int, int> Find(int elementValue)
        {
            var root = GetParent(elementValue);                
            var rank = _rootRanks[root];
            return Tuple.Create(root, rank);
        }

        private int GetParent(int elementValue)
        {
            var root = _roots[elementValue];
            if (elementValue == root)
                return elementValue;
            var realRoot =  GetParent(root);
            _roots[elementValue] = realRoot;
            return realRoot;
        }

        public bool Union(int elementValue1, int elementValue2)
        {
            var root1 = Find(elementValue1);
            var root2 = Find(elementValue2);
            if (root1.Item1 == root2.Item1)
                return false;
            if (root1.Item2 > root2.Item2)
            {
                _roots[root2.Item1] = root1.Item1;
            }
            else if (root1.Item2 < root2.Item2)
            {
                _roots[root1.Item1] = root2.Item1;
            }
            else
            {
                _roots[root2.Item1] = root1.Item1;
                _rootRanks[root1.Item1] = root1.Item2 + 1;
            }
            return true;
        }
    }
}