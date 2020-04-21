using System;
using System.Collections.Generic;
using CustomCollections;
using NUnit.Framework;

namespace CustomCollectionsTests
{
    public class Tests
    {
        [Test]
        public void EmptyCollectionShouldWork()
        {
            var linkedHashMap = new LinkedHashMap<int, string>();

            Assert.False(linkedHashMap.Contains(new KeyValuePair<int, string>()));
            Assert.False(linkedHashMap.ContainsKey(0));
            Assert.AreEqual(0, linkedHashMap.Count);

            foreach (var _ in linkedHashMap) {
                throw new Exception("There should be no elements.");
            }

            Assert.AreEqual(0, linkedHashMap.Keys.Count);
            Assert.AreEqual(0, linkedHashMap.Values.Count);
        }

        [Test]
        public void SimpleOperationsShouldBehaveAsInDictionary()
        {
            var linkedHashMap = new LinkedHashMap<int, string> {
                {1, "One"},
                {3, "Three"},
                {2, "Two"}
            };

            Assert.AreEqual(3, linkedHashMap.Count);
            Assert.AreEqual(3, linkedHashMap.Keys.Count);
            Assert.AreEqual(3, linkedHashMap.Values.Count);
            Assert.True(linkedHashMap.ContainsKey(1));
            Assert.True(linkedHashMap.ContainsKey(2));
            Assert.True(linkedHashMap.ContainsKey(3));
            Assert.True(!linkedHashMap.ContainsKey(4));

            Assert.False(linkedHashMap.Remove(4));
            Assert.True(linkedHashMap.Remove(2));
            Assert.AreEqual(2, linkedHashMap.Count);
            Assert.True(linkedHashMap.Remove(1));
            Assert.AreEqual(1, linkedHashMap.Count);

            Assert.AreEqual("Three", linkedHashMap[3]);

            linkedHashMap[1] = "OneAgain";
            Assert.AreEqual(2, linkedHashMap.Count);
            Assert.AreEqual("OneAgain", linkedHashMap[1]);
            Assert.AreEqual("Three", linkedHashMap[3]);
        }

        [Test]
        public void OrderShouldBePreserved()
        {
            var items = new[] {
                KeyValuePair.Create(1, "One"),
                KeyValuePair.Create(3, "Three"),
                KeyValuePair.Create(2, "Two"),
            };

            var linkedHashMap = new LinkedHashMap<int, string>();
            foreach (var item in items) {
                linkedHashMap.Add(item);
            }

            using var enumerator = linkedHashMap.GetEnumerator();
            enumerator.MoveNext();
            Assert.AreEqual(1, enumerator.Current.Key);
            enumerator.MoveNext();
            Assert.AreEqual(3, enumerator.Current.Key);
            enumerator.MoveNext();
            Assert.AreEqual(2, enumerator.Current.Key);
            Assert.True(!enumerator.MoveNext());

            var num = 0;
            foreach (var item in linkedHashMap) {
                Assert.AreEqual(items[num++], item);
            }

            linkedHashMap[1] = "OneAgain";
            num = 0;
            foreach (var item in linkedHashMap) {
                if (num == 2)
                    Assert.AreEqual(1, item.Key);
                else
                    Assert.AreEqual(items[num + 1], item);
                num++;
            }
        }
    }
}