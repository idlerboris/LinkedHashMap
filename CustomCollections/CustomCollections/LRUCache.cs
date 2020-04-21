using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CustomCollections
{
    public interface ILRUCache<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        bool Contains(TKey key);
        bool TryGet(TKey key, out TValue value);
        TValue Lookup(TKey key);
        void Add(TKey key, TValue value);
    }

    public class LRUCache<TKey, TValue> : ILRUCache<TKey, TValue>
    {
        private readonly int _capacity;
        private readonly LinkedHashMap<TKey, TValue> _linkedHashMap = new LinkedHashMap<TKey, TValue>();

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _linkedHashMap.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Contains(TKey key)
        {
            return _linkedHashMap.ContainsKey(key);
        }

        public bool TryGet(TKey key, out TValue value)
        {
            var result = _linkedHashMap.TryGetValue(key, out value);
            _linkedHashMap.UpdateKey(key);
            return result;
        }

        public TValue Lookup(TKey key)
        {
            return !TryGet(key, out var value) ? default : value;
        }

        public void Add(TKey key, TValue value)
        {
            if (_linkedHashMap.Count == _capacity) {
                _linkedHashMap.Remove(_linkedHashMap.First().Key);
            }
            _linkedHashMap.Add(key, value);
        }

        public LRUCache(int capacity)
        {
            _capacity = capacity;
        }
    }
}