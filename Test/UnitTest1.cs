using Dat;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Test
{
    [TestClass]
    public class LinkedListTests
    {
        [TestMethod]
        public void Add()
        {
            // vorbereitung
            LinkedList<int> ll = new LinkedList<int>();

            // durchführung
            ll.AddFirst(22);

            // auswertung
            Assert.IsTrue(ll.At(0) == 22);

        }

        [TestMethod]
        public void Remove()
        {
            LinkedList<int> ll = new();

            ll.AddFirst(22);
            ll.AddFirst(33);

            ll.RemoveLast();
            ll.RemoveFirst();

            Assert.IsTrue(ll.Count == 0);
            Assert.ThrowsException<IndexOutOfRangeException>(ll.RemoveFirst);
            Assert.ThrowsException<IndexOutOfRangeException>(ll.RemoveLast);
        }

        public void RemoveOnEmpty()
        {

        }
        public void At()
        {

        }
    }

    [TestClass]
    public class QuadTreeTests
    {
        [TestMethod]
        public void Add()
        {
            Dat.QuadTree<GameObject> ot = new();

            ot.Add((1, 1), new GameObject());


        }
        [TestMethod]
        public void Remove()
        {

        }
        [TestMethod]
        public void Find()
        {

        }


    }
    class GameObject
    {

    }
}
