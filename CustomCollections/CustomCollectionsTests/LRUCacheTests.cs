using System.Collections.Generic;
using System.Linq;
using CustomCollections;
using NUnit.Framework;

namespace CustomCollectionsTests
{
    public class LRUCacheTests
    {
        [Test]
        public void CacheShouldPreserveTheOrder()
        {
            var items = new[] {
                KeyValuePair.Create(1, "One"),
                KeyValuePair.Create(3, "Three"),
                KeyValuePair.Create(2, "Two"),
            };

            ILRUCache<int, string> lruCache = new LRUCache<int, string>(100);
            foreach (var (key, value) in items) {
                lruCache.Add(key, value);
            }

            var num = 0;
            foreach (var item in lruCache) {
                Assert.AreEqual(items[num++], item);
            }

            Assert.AreEqual("One", lruCache.Lookup(1));
            num = 0;
            foreach (var item in lruCache) {
                if (num == 2) Assert.AreEqual(1, item.Key);
                else Assert.AreEqual(items[num + 1], item);
                num++;
            }

            Assert.AreEqual(true, lruCache.TryGet(3, out var threeVal));
            Assert.AreEqual("Three", threeVal);
        }

        [Test]
        public void CacheShouldRemoveLeasRecentlyUsedItems()
        {
            var items = new[] {
                KeyValuePair.Create(1, "One"),
                KeyValuePair.Create(3, "Three"),
                KeyValuePair.Create(2, "Two"),
                KeyValuePair.Create(5, "Five"),
                KeyValuePair.Create(4, "Four"),
            };

            ILRUCache<int, string> lruCache = new LRUCache<int, string>(2);
            foreach (var (key, value) in items) {
                lruCache.Add(key, value);
            }

            var cacheElements = lruCache.ToArray();
            Assert.AreEqual(2, cacheElements.Length);
            Assert.AreEqual(5, cacheElements[0].Key);
            Assert.AreEqual(4, cacheElements[1].Key);

            lruCache.Lookup(5);
            cacheElements = lruCache.ToArray();
            Assert.AreEqual(2, cacheElements.Length);
            Assert.AreEqual(4, cacheElements[0].Key);
            Assert.AreEqual(5, cacheElements[1].Key);

            for (var i = 1; i <= 3; i++) {
                Assert.False(lruCache.Contains(i));
            }
            Assert.True(lruCache.Contains(4));
            Assert.True(lruCache.Contains(5));
        }
    }
}