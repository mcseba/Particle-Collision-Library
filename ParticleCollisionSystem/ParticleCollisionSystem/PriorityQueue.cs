using System;
using System.Collections;
using System.Collections.Generic;

namespace ParticleCollisionSystem
{
    /// <summary>
    /// Implementation of 'minimum priority queue' data structure which is ordering items by priority in ascending order.
    /// </summary>
    /// <typeparam name="T">Object stored in PQ of type PQItem</typeparam>
    public class PriorityQueue<T> : IEnumerable<T> where T : IComparable<T>
    {
        #region PQItem struct
        public struct PQItem : IComparable<PQItem>
        {
            public T Obj;

            public double Priority;

            public int CompareTo(PQItem other)
            {
                var x = Priority.CompareTo(other.Priority);

                if (x == 0)
                {
                    x = Obj.CompareTo(other.Obj);
                }
                return x;
            }
        }
        #endregion

        #region props

        private PQItem[] _pq;
        /// <summary>
        /// Array of type PQItem
        /// </summary>
        public PQItem[] PQ
        {
            get => _pq;
            set => _pq = value;
        }

        /// <summary>
        /// Returns number of elements in the priority queue
        /// </summary>
        public int Count { get { return PQ.Length; } }

        /// <summary>
        /// Provides a specific IComparer Comparer to compare values in priority queue
        /// </summary>
        public IComparer<T> Comparer { get; set; }
        #endregion

        #region Ctors
        /// <summary>
        /// Default constructor, creates priority queue with 1 capacity.
        /// </summary>
        public PriorityQueue() : this(1)
        { }

        /// <summary>
        /// Create priority queue with specific capacity.
        /// </summary>
        /// <param name="capacity"></param>
        public PriorityQueue(int capacity)
        {
            PQ = new PQItem[capacity];
        }

        /// <summary>
        /// Creates priority queue with specific capacity and comparer provided.
        /// </summary>
        /// <param name="capacity"></param>
        /// <param name="comparer"></param>
        public PriorityQueue(int capacity, IComparer<T> comparer)
        {
            PQ = new PQItem[capacity];
            Comparer = comparer;
        }

        /// <summary>
        /// Creates new minimum priority queue and fills with data from KeyValuePair
        /// </summary>
        /// <param name="PriorityObject">Param, where Key is priority of type int and value is object of type T</param>
        public PriorityQueue(KeyValuePair<int, T>[] PriorityObject)
        {
            PQ = new PQItem[PriorityObject.Length];

            for (int i = 0; i < Count; i++)
            {
                PQ[i].Obj = PriorityObject[i].Value;
                PQ[i].Priority = PriorityObject[i].Key;
            }
            MinHeapify(0);
        }
        #endregion

        /// <summary>
        /// Add new PQ item to the array and sorts
        /// </summary>
        /// <param name="priority">Priority of this object</param>
        /// <param name="obj">T type object</param>
        public void Enqueue(double priority, T obj)
        {
            Resize(Count + 1);
            var item = new PQItem() { Priority = priority, Obj = obj };
            PQ[Count - 1] = item;
            BuildMinHeap(Count - 1);
        }

        /// <summary>
        /// Returns and removes first priority T type object
        /// </summary>
        /// <returns></returns>
        public T Dequeue()
        {
            if (Count > 0)
            {
                var returnItem = PQ[0];
                PQ[0] = PQ[Count - 1];
                Array.Resize(ref _pq, Count - 1);
                MinHeapify(0);
                return returnItem.Obj;
            }
            else
                throw new Exception("Empty queue.");
        }


        /// <summary>
        /// Recurensive method allows to build Min Priority Queue - lowest priority is at the top of the heap (Ascending order).
        /// </summary>
        /// <param name="index"></param>
        public void MinHeapify(int index)
        {
            var leftChild = ChildL(index);
            var rightChild = ChildR(index);
            var parent = index;

            if (leftChild < Count && PQ[leftChild].Priority < PQ[parent].Priority)
            {
                parent = leftChild;
            }
            if (rightChild < Count && PQ[rightChild].Priority < PQ[parent].Priority)
            {
                parent = rightChild;
            }

            if (parent < Count && parent != index)
            {
                Swap(parent, index);
                MinHeapify(parent);
            }
        }

        /// <summary>
        /// Keeps min order in min priority queue.
        /// </summary>
        /// <param name="index"></param>
        public void BuildMinHeap(int index)
        {
            while (index >= 0 && PQ[index].Priority < PQ[(index - 1) / 2].Priority)
            {
                Swap(index, (index - 1) / 2);
                index = (index - 1) / 2;
            }
        }

        /// <summary>
        /// Helper function to swap two nodes of the priority queue
        /// </summary>
        /// <param name="i">Inex of first item/node</param>
        /// <param name="j">Index of second item/node</param>
        private void Swap(int i, int j)
        {
            if (i < Count && i >= 0 && j >= 0 && j < Count && i != j)
            {
                var temp = PQ[i];
                PQ[i] = PQ[j];
                PQ[j] = temp;
            }
        }

        /// <summary>
        /// Gets left child of the parent node.
        /// </summary>
        /// <param name="index">Parent's node index</param>
        /// <returns></returns>
        private int ChildL(int index)
        {
            return index * 2 + 1;
        }

        /// <summary>
        /// Gets right child of the parent node.
        /// </summary>
        /// <param name="index">Parent's node index</param>
        /// <returns></returns>
        private int ChildR(int index)
        {
            return index * 2 + 2;
        }

        /// <summary>
        /// Checks if priority queue is empty and returns bool.
        /// True - empty
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return Count == 0;
        }

        /// <summary>
        /// Returns smallest priority node which is first node of priority queue
        /// </summary>
        /// <returns></returns>
        public T Min()
        {
            if (!IsEmpty())
            {
                MinHeapify(0);
                return PQ[0].Obj;
            }
            else
                throw new Exception("Priority Queue is empty");
        }

        /// <summary>
        /// Returns highest priority node which is last node of priority queue
        /// </summary>
        /// <returns></returns>
        public T Max()
        {
            if (!IsEmpty())
            {
                MinHeapify(0);
                return PQ[Count - 1].Obj;
            }
            else
                throw new Exception("Priority Queue is empty");
        }

        /// <summary>
        /// Resizes priority queue to the new capacity.
        /// </summary>
        /// <param name="newCapacity">Must be higher than current priority queue length</param>
        public void Resize(int newCapacity)
        {
            if (newCapacity > Count)
            {
                PQItem[] temp = new PQItem[newCapacity];
                for (int i = 0; i < Count; i++)
                {
                    temp[i] = PQ[i];
                }
                PQ = temp;
            }
        }

        #region IEnumerators
        public IEnumerator<T> GetEnumerator()
        {
            foreach (var el in PQ)
            {
                Console.WriteLine($"Priority: {el.Priority}");
                yield return el.Obj;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}
