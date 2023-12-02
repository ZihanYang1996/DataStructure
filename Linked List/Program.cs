using System.Reflection.Metadata;
using System.Collections.Generic;
using System.Collections;

public class Node<T>
{
    public T Value { get; set; }
    public Node<T>? Next { get; set; }
    public Node<T>? Prev { get; set; }

    public Node(T value/* , Node<T> next, Node<T> prev */)
    {
        this.Value = value;
        // this.next = next;
        // this.prev = prev;
        // Can use the object initailizer instead !!!!!
    }
}

public class DoublyLinkedList<T> : IEnumerable<T>
{

    public Node<T>? Head = null;
    public Node<T>? Tail = null;
    private int Size = 0;

    public int GetSize()
    {
        return Size;
    }

    public bool IsEmpty()
    {
        return Size == 0;
    }

    public void AddFirst(T value)
    {
        if (IsEmpty())
        {
            Head = Tail = new Node<T>(value) { Next = null, Prev = null };  // Object initializer syntax
        }
        else
        {
            Head.Prev = new Node<T>(value) { Next = Head, Prev = null };
            Head = Head.Prev;
        }
        Size++;
    }

    public void AddLast(T value)
    {
        if (IsEmpty())
        {
            Head = Tail = new Node<T>(value) { Next = null, Prev = null };
        }
        else
        {
            Tail.Next = new Node<T>(value) { Next = null, Prev = Tail };
            Tail = Tail.Next;
        }
        Size++;
    }

    public void RemoveFirst()
    {
        if (IsEmpty())
        {
            return;
        }

        Head = Head.Next;
        Size--;
        if (IsEmpty())
        {
            Tail = null;
        }
        else
        {
            Head.Prev = null;
        }
    }

    public void RemoveLast()
    {
        if (IsEmpty())
        {
            return;
        }
        Tail = Tail.Prev;
        Size--;
        if (IsEmpty())
        {
            Head = null;
        }
        else
        {
            Tail.Next = null;
        }
    }

    public void Remove(Node<T> node)
    {
        if (node.Prev == null)
        {
            RemoveFirst();
            // Console.WriteLine("RemoveFirst");
            return;
        }
        if (node.Next == null)
        {
            RemoveLast();
            return;
        }
        node.Prev.Next = node.Next;
        node.Next.Prev = node.Prev;
        Size--;
        node.Value = default;
        node = node.Next = node.Prev = null;
    }

    public void RemoveValue(T value)
    {
        Node<T>? current = Head;
        while (current != null)
        {
            if (current.Value.Equals(value))
            {
                Remove(current);
                return;
            }
            current = current.Next;
        }
    }

    public void Print()
    {
        Node<T>? current = Head;
        while (current != null)
        {
            Console.Write(current.Value + " ");
            current = current.Next;
        }
        Console.WriteLine();
    }

    public IEnumerator<T> GetEnumerator()
    {
        Node<T>? current = Head;
        while (current != null)
        {
            yield return current.Value;
            current = current.Next;
        }
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }    

}

public class Program
{
    public static void Main()
    {
        DoublyLinkedList<int> list = new DoublyLinkedList<int>();
        list.AddLast(1);
        list.Print();
        Console.WriteLine(list.GetSize());
        list.AddLast(2);
        list.Print();
        Console.WriteLine(list.GetSize());
        list.AddLast(3);
        list.Print();
        Console.WriteLine(list.GetSize());
        list.AddLast(4);
        list.Print();
        Console.WriteLine(list.GetSize());
        list.AddLast(5);
        list.Print();
        Console.WriteLine(list.GetSize());
        list.RemoveValue(3);
        list.Print();
        Console.WriteLine(list.GetSize());
        list.RemoveValue(1);
        list.Print();
        Console.WriteLine(list.GetSize());
        list.RemoveValue(5);
        list.Print();
        Console.WriteLine(list.GetSize());
        list.AddFirst(100);
        list.Print();
        list.AddLast(200);
        list.Print();
        Console.WriteLine(list.GetSize());
        list.RemoveValue(2);
        list.Print();
        Console.WriteLine(list.GetSize());
        foreach (var item in list)
        {
            Console.Write(item + "; ");
        }
    }
}