using System;

namespace Dat
{
    public class LinkedList<PlaceHolderForDataType>
    {
        public int Count { get; private set; }
        private LinkedListElement<PlaceHolderForDataType> Head;
        private LinkedListElement<PlaceHolderForDataType> Tail;

        public void AddFirst(PlaceHolderForDataType Value)
        {
            LinkedListElement<PlaceHolderForDataType> newElement = new();
            newElement.Data = Value;
            if (Head == null)
            {
                // element ist das erste was in die liste kommt.
                Head = newElement;
                Tail = newElement;
                newElement.Prev = null;
                newElement.Next = null;
            }
            else
            {
                // nicht das erste element in der liste
                Head.Prev = newElement;
                newElement.Prev = null;
                newElement.Next = Head;
                Head = newElement;
            }
            Count++;
        }

        public void AddLast(PlaceHolderForDataType Value)
        {
            LinkedListElement<PlaceHolderForDataType> newElement = new();
            newElement.Data = Value;
            if (Head == null)
            {
                // element ist das erste was in die liste kommt.
                Head = newElement;
                Tail = newElement;
                newElement.Prev = null;
                newElement.Next = null;
            }
            else
            {
                // nicht das erste element in der liste
                Tail.Next = newElement;
                newElement.Next = null;
                newElement.Prev = Head;
                Tail = newElement;
            }
            Count++;
        }

        public void RemoveLast()
        {
            if (Tail != null)
            {
                // es gibt element(e)
                if (Tail.Prev != null)
                {
                    // es gibt mehr als eins
                    LinkedListElement<PlaceHolderForDataType> toDelete = Tail;
                    Tail = toDelete.Prev;
                    Tail.Next = null;
                    toDelete.Prev = null;
                    Count--;
                }
                else
                {
                    // es gibt nur eins
                    Head = null;
                    Tail = null;
                    Count = 0;
                }
            }
            else
            {
                // es gab kein element in der Liste
                throw new IndexOutOfRangeException("Remove from empty LinkedList is not allowed.");
            }
        }

        public ref PlaceHolderForDataType At(int Index)
        {
            if (Count <= Index)
                throw new IndexOutOfRangeException();

            LinkedListElement<PlaceHolderForDataType> buffer = Head;
            for (int counter = 0; counter < Index; counter++)
            {
                buffer = buffer.Next;
            }

            return ref buffer.Data;
        }

        public void RemoveFirst()
        {
            if (Head != null)
            {
                // es gibt element(e)
                if (Head.Next != null)
                {
                    // es gibt mehr als eins
                    LinkedListElement<PlaceHolderForDataType> toDelete = Head;
                    Head = toDelete.Next;
                    Head.Prev = null;
                    toDelete.Next = null;
                    Count--;
                }
                else
                {
                    // es gibt nur eins
                    Head = null;
                    Tail = null;
                    Count = 0;
                }
            }
            else
            {
                // es gab kein element in der Liste
                throw new IndexOutOfRangeException("Remove from empty LinkedList is not allowed.");
            }

        }

        public void Clear()
        {
            //TODO, das geht besser
            while (Count > 0)
            {
                RemoveFirst();
            }
        }
    }
}