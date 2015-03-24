﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace GreedyAlgorithms
{
    public class Heap<TKey, TValue>
        where TKey : IComparable<TKey>
        where TValue : IEquatable<TValue>
    {
        private readonly List<KeyValuePair<TKey, TValue>> _innerStorage = new List<KeyValuePair<TKey, TValue>>();
        private readonly Dictionary<TValue, int> _index = new Dictionary<TValue, int>();
        public int Count { get { return _innerStorage.Count; } }

        public TValue GetOrInsert(KeyValuePair<TKey, TValue> element)
        {
            int index;
            if (_index.TryGetValue(element.Value, out index))
                return _innerStorage[index].Value;
            Insert(element);
            return default(TValue);
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
            _innerStorage.Add(element);
            _index.Add(element.Value, Count - 1);
            if (Count > 1)
                RebalanceUp(_innerStorage.Count - 1);
        }

        public bool TryDeleteValue(TValue element, out KeyValuePair<TKey, TValue> value)
        {
            int index;
            value = default(KeyValuePair<TKey,TValue>);
            var result = _index.TryGetValue(element, out index);
            if (!result) 
                return false;
            value = _innerStorage[index];
            SwapElements(index, Count - 1);
            _innerStorage.RemoveAt(Count - 1);
            _index.Remove(value.Value);
            if (Count > 0)
                RebalaceDown(index);
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
            _innerStorage[first] = _innerStorage[second];
            _index[_innerStorage[second].Value] = first;
            _innerStorage[second] = firstelement;
            _index[firstelement.Value] = second;
        }
    }
}