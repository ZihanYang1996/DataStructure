using System;
using System.Collections.Generic;
using System.Diagnostics;



public class UnionFind
{
    //The bumber of elements in this union find
    private int size;

    // Used to track the size of each of the components
    private int[] sz;

    // id[i] points to the parent of i, if id[i] = i then i is a root node
    private int[] id;

    // Tracks the number of components in the union find
    private int numComponents;

    public UnionFind(int size)
    {
        if (size <= 0) throw new ArgumentException("Size <= 0 is not allowed");

        this.size = numComponents = size;
        sz = new int[size];
        id = new int[size];

        for (int i = 0; i < size; i++)
        {
            id[i] = i; // Linkd to itself (self root)
            sz[i] = 1; // Each component is originally of size one
        }
    }

    // Find which component/set 'p' belongs to, takes amortized constant time.
    public int Find(int p)
    {
        int root = p;
        while (root != id[root])
        {
            root = id[root];
        }

        // Compress the path leading back to the root.
        // Doing this operation is called "path compression"
        // and is what gives us amortized time complexity.
        while (p != root)
        {
            int next = id[p];
            id[p] = root;
            p = next;
        }

        return root;
    }

    // Return whether or not the elements 'p' and
    // 'q' are in the same components/set.
    public bool Connected(int p, int q)
    {
        return Find(p) == Find(q);
    }

    // Return the size of the components/set 'p' belongs to
    public int ComponentSize(int p)
    {
        return sz[Find(p)];
    }

    // Return the number of elements in this UnionFind/Disjoint set
    public int Size()
    {
        return size;
    }

    // Returns the number of remaining components/sets
    public int Components()
    {
        return numComponents;
    }

    // Unify the components/sets containing elements 'p' and 'q'
    public void Unify(int p, int q)
    {
        int root1 = Find(p);
        int root2 = Find(q);

        // These elements are already in the same group!
        if (root1 == root2) return;

        if (sz[root1] < sz[root2])
        {
            sz[root2] += sz[root1];
            id[root1] = root2;
        }
        else
        {
            sz[root1] += sz[root2];
            id[root2] = root1;
        }

        // Since the roots found are different we know that the
        // number of components/sets has decreased by one
        numComponents--;
    }

}

public class Program
{
    public static void Main()
    {
        UnionFind uf = new UnionFind(10);

        // Perform some unions
        uf.Unify(1, 2);
        uf.Unify(3, 4);
        uf.Unify(1, 3);

        // Check if elements are in the same group
        Console.WriteLine(uf.Connected(1, 4)); // Should print "True"
        Console.WriteLine(uf.Connected(1, 5)); // Should print "False"

        // Print the number of components
        Console.WriteLine(uf.Components()); // Should print "7" because there are 7 groups left: {0}, {1, 2, 3, 4}, {5}, {6}, {7}, {8}, {9}
    }
}