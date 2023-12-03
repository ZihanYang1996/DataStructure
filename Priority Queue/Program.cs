using System;
using System.Collections.Generic;

public class PriorityQueue<T> where T : IComparable<T>
{
    // A dynamic list to track the elements inside the heap
    private List<T> heap;
    // This dictionary keeps track of the possible indices a particular 
    //node value is found in the heap. 
    //Having this mapping lets us have O(log(n)) removals 
    //and O(1) element containment check
    private Dictionary<T, SortedSet<int>> lookup = new Dictionary<T, SortedSet<int>>();

    private int heapSize = 0;  // Number of elements present in heap
    private int heapCapacity = 0;  // The internal capacity of the heap

    // A constructor that initializes the heap with a default capacity of 1
    public PriorityQueue() : this(1) { }  // Learn about this syntax

    public PriorityQueue(int size)
    {
        heap = new List<T>(size);
    }

    public PriorityQueue(T[] elems)
    {
        heapSize = heapCapacity = elems.Length;
        heap = new List<T>(heapCapacity);

        // Place all element in heap
        for (int i = 0; i < heapSize; i++)
        {
            LookupAdd(elems[i], i);
            heap.Add(elems[i]);
        }

        // Heapify process, O(n), dont't quite understand, need to review
        for (int i = Math.Max(0, (heapSize / 2) - 1); i >= 0; i--)
        {
            Sink(i);
        }
    }
    // Returns true/false depending on if the priority queue is empty
    public bool IsEmpty()
    {
        return heapSize == 0;
    }
    public int Size()
    {
        return heapSize;
    }

    // Clears everything inside the heap, O(n)
    public void Clear()
    {
        for (int i = 0; i < heapCapacity; i++)
        {
            heap[i] = default;
        }
        heapSize = 0;
        lookup.Clear();
    }

    // Returns the value of the element with the lowest
    // priority in this priority queue. If the priority
    // queue is empty null is returned.
    public T Peek()
    {
        if (IsEmpty()) return default;
        return heap[0];

    }

    // Removes the root of the heap, O(log(n))
    public T poll()
    {
        return RemoveAt(0);
    }

    private void LookupAdd(T elem, int index)
    {
        SortedSet<int> set;
        if (lookup.TryGetValue(elem, out set))
        {
            set.Add(index);
        }
        else
        {
            set = new SortedSet<int>();
            set.Add(index);
            lookup.Add(elem, set);
        }

    }
    // Top down node sink, O(log(n))
    private void Sink(int k)
    {
        int heapSize = Size();

        while (true)
        {
            int left = (2 * k) + 1;  // Left  node
            int right = (2 * k) + 2;  // Right node+
            if (right >= heapSize) break;  // If outside of bounds of heap, stop
            int smallest = Less(left, right);  // Get the smallest of the two nodes
            if (Less(k, smallest) == k) break;  // Stop if heap invariant is satisfied
            Swap(smallest, k);  // Swap with the smallest of the two nodes
            k = smallest;
        }
    }

    private void Swap(int i, int j)
    {
        T elem_i = heap[i];
        T elem_j = heap[j];
        heap[i] = elem_j;
        heap[j] = elem_i;

        // Update the lookup table
        LookupSwap(elem_i, elem_j, i, j);
    }

    private void LookupSwap(T elem_i, T elem_j, int i, int j)
    {
        SortedSet<int> set_i = lookup[elem_i];
        SortedSet<int> set_j = lookup[elem_j];

        set_i.Remove(i);
        set_j.Remove(j);

        set_i.Add(j);
        set_j.Add(i);
    }


    private int Less(int i, int j)
    {
        T node1 = heap[i];
        T node2 = heap[j];
        return node1.CompareTo(node2) <= 0 ? i : j;
    }

    // Removes a particular element in the heap, O(log(n))
    public T RemoveAt(int i)
    {
        throw new NotImplementedException();  // To be implemented tomorrow
    }
}
