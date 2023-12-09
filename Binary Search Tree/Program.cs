using System.Diagnostics.Tracing;
using System.Drawing;

public class BinarySearchTree<T> where T : IComparable<T>
{
    // The number of elements currently inside the heap
    private int nodeCount = 0;

    // This is the root of our binary search tree
    private Node root = null;

    // Internal node containing node references
    // and the actual node data
    private class Node
    {
        public T data;
        public Node left, right;
        public Node(Node left, Node right, T elem)
        {
            this.data = elem;
            this.left = left;
            this.right = right;
        }
    }

    // Check if this binary tree is empty
    public bool IsEmpty()
    {
        return Size() == 0;
    }

    // Get the number of nodes in this binary tree
    public int Size()
    {
        return nodeCount;
    }

    // Add an element to this binary tree. Returns true
    // if we successfully perform an insertion
    public bool Add(T elem)
    {
        // Check if the value already exists in this
        // binary tree, if it does ignore adding it
        if (Contains(elem))
        {
            return false;
        }
        // Otherwise add this element to the binary tree
        else
        {
            root = Add(root, elem);
            nodeCount++;
            return true;
        }
    }

    // Private method to recursively add a value in the binary tree
    private Node Add(Node node, T elem)
    {
        // Base case: found a leaf node
        if (node == null)
        {
            node = new Node(null, null, elem);
        }
        else
        {
            // Place lower elements values in the left subtree
            if (elem.CompareTo(node.data) < 0)
            {
                node.left = Add(node.left, elem);
            }
            else
            {
                node.right = Add(node.right, elem);
            }
        }
        return node;
    }

    // Remove a value from this binary tree if it exists
    public bool Remove(T elem)
    {
        // Make sure the node we want to remove
        // actually exists before we remove it
        if (Contains(elem))
        {
            root = Remove(root, elem);
            nodeCount--;
            return true;
        }
        return false;
    }

    // Private recursive method to remove a value
    // from the binary tree
    private Node Remove(Node node, T elem)
    {
        if (node == null) return null;

        int cmp = elem.CompareTo(node.data);

        if (cmp < 0)
        {
            node.left = Remove(node.left, elem);
        }
        else if (cmp > 0)
        {
            node.right = Remove(node.right, elem);
        }
        // We found the node we want to remove
        else
        {
            // This is the case with only a right subtree or no subtree at all.
            // In this situation just swap the node we wish to remove
            // with its right child.
            if (node.left == null)
            {
                Node rightChild = node.right;

                node.data = default;
                node = null;
                return rightChild;
            }
            else if (node.right == null)
            {
                // This is the case with only a left subtree or
                // no subtree at all. In this situation just swap
                // the node we wish to remove with its left child.
                Node leftChild = node.left;

                node.data = default;
                node = null;
                return leftChild;
            }
            else
            {
                // Find the leftmost node in the right subtree
                Node tmp = DigLeft(node.right);
                // Swap the data
                node.data = tmp.data;
                // Go into the right subtree and remove the leftmost node we found
                // and swapped data with. This prevents us from having two nodes in
                // our tree with the same value.
                node.right = Remove(node.right, tmp.data);
            }
        }
        return node;
    }

    // Helper method to find the leftmost node
    private Node DigLeft(Node node)
    {
        Node cur = node;
        while (cur.left != null)
        {
            cur = cur.left;
        }
        return cur;
    }

    // Helper function to find the rightmost node
    private Node DigRight(Node node)
    {
        Node cur = node;
        while (cur.right != null)
        {
            cur = cur.right;
        }
        return cur;
    }

    // returns true is the element exists in the tree
    public bool Contains(T elem)
    {
        return Contains(root, elem);
    }

    // private recursive method to find an element in the tree

    private bool Contains(Node node, T elem)
    {
        // Base case: reached bottom, value not found
        if (node == null)
        {
            return false;
        }

        int cmp = elem.CompareTo(node.data);

        // Dig into the left subtree because the value we're
        // looking for is smaller than the current value
        if (cmp < 0)
        {
            return Contains(node.left, elem);
        }
        // Dig into the right subtree because the value we're
        // looking for is greater than the current value
        else if (cmp > 0)
        {
            return Contains(node.right, elem);
        }
        // We found the value we were looking for
        else
        {
            return true;
        }
    }

    // Computes the height of the tree, O(n)
    public int Height()
    {
        return Height(root);
    }

    // Recursive helper method to compute the height of the tree
    private int Height(Node node)
    {
        if (node == null) return 0;
        return Math.Max(Height(node.left), Height(node.right)) + 1;
    }

    // This method returns an iterator for a given TreeTraversalOrder.
    // The ways in which you can traverse the tree are in four different ways:
    // preorder, inorder, postorder and levelorder.
    public IEnumerable<T> PreOrder()
    {
        return DoPreorder(root);
    }

    public IEnumerable<T> InOrder()
    {
        return DoInorder(root);
    }

    public IEnumerable<T> PostOrder()
    {
        return DoPostorder(root);
    }


    private IEnumerable<T> DoPreorder(Node node)
    {
        if (node != null)
        {
            yield return node.data;
            foreach (var left in DoPreorder(node.left))
            {
                yield return left;
            }
            foreach (var right in DoPreorder(node.right))
            {
                yield return right;
            }
        }

    }

    private IEnumerable<T> DoInorder(Node node)
    {
        if (node != null)
        {
            foreach (var left in DoInorder(node.left))
            {
                yield return left;
            }

            yield return node.data;

            foreach (var right in DoInorder(node.right))
            {
                yield return right;
            }
        }
    }

    private IEnumerable<T> DoPostorder(Node node)
    {
        if (node != null)
        {
            foreach (var left in DoPostorder(node.left))
            {
                yield return left;
            }

            foreach (var right in DoPostorder(node.right))
            {
                yield return right;
            }

            yield return node.data;
        }
    }
}

public class Program
{
    public static void Main()
    {
        BinarySearchTree<int> bst = new BinarySearchTree<int>();
        Console.WriteLine(bst.Add(5));
        Console.WriteLine(bst.Add(3));
        Console.WriteLine(bst.Add(7));
        Console.WriteLine(bst.Add(2));
        Console.WriteLine(bst.Add(4));
        Console.WriteLine(bst.Add(6));
        Console.WriteLine(bst.Add(8));

        Console.WriteLine("PreOrder Traversal:");
        foreach (var item in bst.PreOrder())
        {
            Console.Write(item + " ");
        }
        Console.WriteLine("\n");

        Console.WriteLine("InOrder Traversal:");
        foreach (var item in bst.InOrder())
        {
            Console.Write(item + " ");
        }
        Console.WriteLine("\n");

        Console.WriteLine("PostOrder Traversal:");
        foreach (var item in bst.PostOrder())
        {
            Console.Write(item + " ");
        }
        Console.WriteLine("\n");

        Console.WriteLine(bst.Add(5));
        Console.WriteLine(bst.Add(3));
        Console.WriteLine(bst.Add(7));
        Console.WriteLine(bst.Add(2));
        Console.WriteLine(bst.Add(4));
        Console.WriteLine(bst.Add(6));
        Console.WriteLine(bst.Add(8));
        Console.WriteLine("InOrder Traversal:");
        foreach (var item in bst.InOrder())
        {
            Console.Write(item + " ");
        }
        Console.WriteLine("\n");

        Console.WriteLine(bst.Remove(5));
        Console.WriteLine("InOrder Traversal:");
        foreach (var item in bst.InOrder())
        {
            Console.Write(item + " ");
        }
        Console.WriteLine("\n");

        Console.WriteLine(bst.Remove(7));
        Console.WriteLine("InOrder Traversal:");
        foreach (var item in bst.InOrder())
        {
            Console.Write(item + " ");
        }
        Console.WriteLine("\n");

        Console.WriteLine(bst.Remove(6));
        Console.WriteLine("InOrder Traversal:");
        foreach (var item in bst.InOrder())
        {
            Console.Write(item + " ");
        }
        Console.WriteLine("\n");
    }
}