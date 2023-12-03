using System;
using System.Collections.Generic;
using System.Diagnostics;

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

    // Test if an element is in heap, O(1)
    public bool Contains(T elem)
    {
        if (elem == null) return false;
        return lookup.ContainsKey(elem);

        // Linear scan to check containment, O(n)
        // for (int i = 0; i < heapSize; i++)
        // {
        //     if (heap[i].Equals(elem)) return true;
        // }
        // return false;
    }

    // Adds an element to the priority queue, the
    // element must not be null, O(log(n))
    public void Add(T elem)
    {
        if (elem == null) throw new ArgumentNullException();

        if (heapSize < heapCapacity)
        {
            heap[heapSize] = elem;
        }
        else
        {
            heap.Add(elem);
            heapCapacity++;
        }
        LookupAdd(elem, heapSize);
        Swim(heapSize++);

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

    // Removes a particular element in the heap, O(log(n))
    public bool Remove(T elem)
    {
        if (elem == null) return false;

        // Logarithmic removal with lookup, O(log(n))
        int index = LookupGet(elem);
        if (index != -1) RemoveAt(index);
        return index != -1;
    }

    // Removes a node at particular index, O(log(n))
    public T RemoveAt(int i)
    {
        if (IsEmpty()) return default;

        heapSize--;
        T removed_data = heap[i];
        Swap(i, heapSize);

        // Obliterate the value
        heap[heapSize] = default;
        LookupRemove(removed_data, heapSize);

        // Remove last element
        if (i == heapSize) return removed_data;

        T elem = heap[i];

        // Try sinking element
        Sink(i);

        // If sinking did not work try swimming
        if (heap[i].Equals(elem)) Swim(i);

        return removed_data;
    }

    // Makesure the heap invariant is maintained
    // Call this method with k=-0 to start at the root
    public bool IsMinHeap(int k)
    {
        // If we are outside the bounds of the heap return true
        if (k >= heapSize) return true;

        int left = (2 * k) + 1;
        int right = (2 * k) + 2;

        if (left < heapSize && Less(k, left) != k) return false;
        if (right < heapSize && Less(k, right) != k) return false;

        return IsMinHeap(left) && IsMinHeap(right);
    }

    // Removes the index at a given value, O(log(n))
    private void LookupRemove(T elem, int index)
    {
        SortedSet<int> set;
        if (lookup.TryGetValue(elem, out set))
        {
            set.Remove(index);
        }
        if (set.Count == 0) lookup.Remove(elem);
    }


    // Extracts an index position for the given value
    // NOTE: If a value exists multiple times in the heap the highest
    // index is returned (this has arbitrarily been chosen)
    private int LookupGet(T elem)
    {
        SortedSet<int> set;
        if (lookup.TryGetValue(elem, out set))
        {
            return set.Max;
        }
        return -1;
    }

    // Perform bottom up node swim, O(log(n))
    private void Swim(int k)
    {
        // Grab the index of the next parent node WRT to k
        int parent = (k - 1) / 2;

        // Keep swimming while we have not reached the
        // root and while we're less than our parent.

        while (k > 0 && Less(k, parent) == k)
        {
            // Exchange k with the parent
            Swap(parent, k);
            k = parent;

            // Grab the index of the next parent node WRT to k
            parent = (k - 1) / 2;
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
}
