using System.Collections;
using System.Linq;
using System.Threading;
using NetGore.Collections;
using NUnit.Framework;

namespace NetGore.Tests.Collections
{
    [TestFixture]
    public class TSListTests
    {
        const int _testRange = 30;

        /// <summary>
        /// Dummy method that enumerators over the parameter object.
        /// </summary>
        static void DummyEnumerator(object obj)
        {
            IEnumerable e = (IEnumerable)obj;

            string s = string.Empty;
            foreach (object item in e)
            {
                s += item.ToString();
                Thread.Sleep(1);
            }

            if (s == null)
            {
            }
        }

        [Test]
        public void ThreadSafeAddTest()
        {
            var l = new TSList<int>(Enumerable.Range(1, _testRange));

            for (int i = 0; i < 10; i++)
            {
                Thread t = new Thread(DummyEnumerator);
                t.Start(l);
            }

            for (int i = 0; i < 10; i++)
            {
                l.Add(i);
            }
        }

        [Test]
        public void ThreadSafeFindTest()
        {
            var l = new TSList<int>(Enumerable.Range(1, _testRange));

            for (int i = 0; i < 10; i++)
            {
                Thread t = new Thread(DummyEnumerator);
                t.Start(l);
            }

            for (int i = 0; i < 10; i++)
            {
                l.Find(y => y == 10);
            }
        }

        [Test]
        public void ThreadSafeIndexerTest()
        {
            var l = new TSList<int>(Enumerable.Range(1, _testRange));

            for (int i = 0; i < 10; i++)
            {
                Thread t = new Thread(DummyEnumerator);
                t.Start(l);
            }

            for (int i = 0; i < 10; i++)
            {
                int x = l[i];
                l[i] = x + 1;
                Assert.AreEqual(x + 1, l[i]);
            }
        }

        [Test]
        public void ThreadSafeIndexOfTest()
        {
            var l = new TSList<int>(Enumerable.Range(1, _testRange));

            for (int i = 0; i < 10; i++)
            {
                Thread t = new Thread(DummyEnumerator);
                t.Start(l);
            }

            for (int i = 1; i < 10; i++)
            {
                int x = l.IndexOf(i);
                Assert.AreEqual(i, l[x]);
            }
        }

        [Test]
        public void ThreadSafeInsertTest()
        {
            var l = new TSList<int>(Enumerable.Range(1, _testRange));

            for (int i = 0; i < 10; i++)
            {
                Thread t = new Thread(DummyEnumerator);
                t.Start(l);
            }

            for (int i = 0; i < 10; i++)
            {
                l.Insert(0, i);
            }
        }

        [Test]
        public void ThreadSafeRemoveAtTest()
        {
            var l = new TSList<int>(Enumerable.Range(1, _testRange));

            for (int i = 0; i < 10; i++)
            {
                Thread t = new Thread(DummyEnumerator);
                t.Start(l);
            }

            for (int i = 0; i < 10; i++)
            {
                l.RemoveAt(0);
            }
        }

        [Test]
        public void ThreadSafeRemoveTest()
        {
            var l = new TSList<int>(Enumerable.Range(1, _testRange));

            for (int i = 0; i < 10; i++)
            {
                Thread t = new Thread(DummyEnumerator);
                t.Start(l);
            }

            for (int i = 0; i < 10; i++)
            {
                l.Remove(i);
            }
        }

        [Test]
        public void ThreadSafeToArrayTest()
        {
            var l = new TSList<int>(Enumerable.Range(1, _testRange));

            for (int i = 0; i < 10; i++)
            {
                Thread t = new Thread(DummyEnumerator);
                t.Start(l);
            }

            for (int i = 0; i < 10; i++)
            {
                l.ToArray();
            }
        }

        [Test]
        public void ThreadSafeTrimExcessTest()
        {
            var l = new TSList<int>(Enumerable.Range(1, _testRange));

            for (int i = 0; i < 10; i++)
            {
                Thread t = new Thread(DummyEnumerator);
                t.Start(l);
            }

            for (int i = 0; i < 10; i++)
            {
                l.TrimExcess();
                l.AddRange(Enumerable.Range(1, _testRange / 2));
            }
        }
    }
}