using System;
using System.Collections.Generic;

namespace GreedyAlgorithms
{
    public class Heap<TKey, TValue>
        where TKey : IComparable<TKey>
        //where TValue : IEquatable<TValue>
    {
        private readonly List<KeyValuePair<TKey, TValue>> _innerStorage;
        private readonly Dictionary<TValue, int> _index;

        public Heap()
        {
            _innerStorage = new List<KeyValuePair<TKey, TValue>>();
            _index=new Dictionary<TValue, int>();
        }

        public Heap(int count)
        {
            _innerStorage = new List<KeyValuePair<TKey, TValue>>(count);
            _index = new Dictionary<TValue, int>(count);
        }

        public int Count { get { return _innerStorage.Count; } }

        //public KeyValuePair<TKey,TValue>? GetOrInsert(KeyValuePair<TKey, TValue> element)
        //{
        //    int index;
        //    if (_index.TryGetValue(element.Value, out index))
        //        return _innerStorage[index];
        //    Insert(element);
        //    return null;
        //}

        private void Check()
        {
            for (int i = 0; i < Count/2+1; i++)
            {
                var parent = _innerStorage[i].Key;
                var childIndex1 = (i + 1)*2 - 1;
                if (childIndex1>Count-1)
                {
                    continue;
                }
                var child1 = _innerStorage[childIndex1].Key;
                if(child1.CompareTo(parent)<0)
                    throw new InvalidOperationException();
                var childIndex2 = childIndex1 + 1;
                if (childIndex2 > Count - 1)
                {
                    continue;
                }
                var child2 = _innerStorage[childIndex2].Key;
                if (child2.CompareTo(parent) < 0)
                    throw new InvalidOperationException();
                var result = Tuple.Create(child2, parent);
            }
        }

        public void RandomizeHead()
        {
            if(Count==0)
                return;
            var random = new Random();
            var rndindex = random.Next(Count - 1);
            SwapElements(0, rndindex);
            RebalaceDown(0);
        }

        public void Insert(KeyValuePair<TKey, TValue> element)
        {
            var index = Count;
            _innerStorage.Add(element);
            _index.Add(element.Value, index);
            if (Count > 1)
                RebalanceUp(index);
        }

        public bool TryDeleteValue(TValue element, out KeyValuePair<TKey, TValue> value)
        {
            int index;
            value = default(KeyValuePair<TKey,TValue>);
            if (!_index.TryGetValue(element, out index)) 
                return false;
            SwapElements(index, Count - 1);
            value = _innerStorage[Count-1];
            _innerStorage.RemoveAt(Count - 1);
            _index.Remove(element);
            if (Count > 0&&index<Count)
            {
                RebalaceDown(index);
                RebalanceUp(index);
            }
            return true;
        }
        
        public KeyValuePair<TKey, TValue> ExtractMinValue()
        {
            var result = _innerStorage[0];
            SwapElements(0, Count - 1);
            _innerStorage.RemoveAt(Count - 1);
            _index.Remove(result.Value);
            if (Count > 0)
                RebalaceDown(0);
            return result;
        }

        private void RebalanceUp(int childIndex)
        {
            if(childIndex==0)
                return;
            var parentIndex = (childIndex + 1)/2 - 1;
            var childElement = _innerStorage[childIndex];
            var parentElement = _innerStorage[parentIndex];
            if (parentElement.Key.CompareTo(childElement.Key)<=0)
            {
                return;
            }
            SwapElements(childIndex, parentIndex);
            RebalanceUp(parentIndex);
        }

        private void RebalaceDown(int parentIndex)
        {
            var childIndex1 = (parentIndex + 1)*2 - 1;
            if (childIndex1>Count-1)
            {
                return;
            }
            var childIndex2 = childIndex1 + 1;
            int childIndex;
            if (childIndex2 > Count - 1)
                childIndex = childIndex1;
            else
            {
                childIndex = _innerStorage[childIndex1].Key.CompareTo(_innerStorage[childIndex2].Key) < 0
                    ? childIndex1
                    : childIndex2;
            }
            if(_innerStorage[childIndex].Key.CompareTo(_innerStorage[parentIndex].Key)>=0)
                return;
            SwapElements(childIndex, parentIndex);
            RebalaceDown(childIndex);
        }

        private void SwapElements(int first, int second)
        {
            var firstelement = _innerStorage[first];
            var secondElement = _innerStorage[second];
            _innerStorage[first] = secondElement;
            _index[secondElement.Value] = first;
            _innerStorage[second] = firstelement;
            _index[firstelement.Value] = second;
        }
    }
}