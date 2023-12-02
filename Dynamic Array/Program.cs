using System;
using System.Collections.Generic;

public class DynamicArray<T>
{
    private T[] array;
    private int length;  // length user thinks array is
    private int capacity;  // Actual array size

    public DynamicArray(int initialCapacity = 4)
    {
        array = new T[initialCapacity];
        length = 0;
        capacity = initialCapacity;
    }

    public DynamicArray(T[] array)
    {
        this.array = array;
        length = array.Length;
        capacity = array.Length;
    }

    public int Size()
    {
        return length;
    }

    public bool IsEmpty()
    {
        return length == 0;
    }

    public T Get(int index)
    {
        return array[index];
    }

    public void Set(int index, T element)
    {
        array[index] = element;
    }

    public void Clear()
    {
        for (int i = 0; i < capacity; i++)
        {
            array[i] = default;
        }
        length = 0;
    }

    public void Add(T element)
    {
        if (length + 1 >= capacity)  // Time to resize
        {
            if (capacity == 0)
            {
                capacity = 1;
            }
            else
            {
                capacity *= 2;  // double the size
                T[] newArray = new T[capacity];
                Array.Copy(array, newArray, length);
                array = newArray;
            }
        }
        array[length++] = element;
    }

    public void RemoveAt(int index)
    {
        T[] newArray = new T[length - 1];
        for (int i = 0, j = 0; i < length; i++, j++)
        {
            if (i == index)
            {
                j -= 1;
            }
            else
            {
                newArray[j] = array[i];
            }
        }
        array = newArray;
        length--;
    }

    public bool Remove(T element)
    {
        for (int i = 0; i < length; i++)
        {
            if (array[i].Equals(element))
            {
                RemoveAt(i);
                return true;
            }
        }
        return false;
    }

    public void PrintArray()
    {
        for (int i = 0; i < length; i++)
        {
            Console.Write(array[i] + " ");
        }
        Console.WriteLine();
    }
}

public class Program
{
    public static void Main()
    {
        DynamicArray<int> arr = new DynamicArray<int>();
        arr.Add(1);
        arr.Add(2);
        arr.Add(3);
        arr.Add(4);
        arr.Add(5);
        arr.PrintArray();
        arr.RemoveAt(2);
        arr.PrintArray();
        arr.Remove(4);
        arr.PrintArray();
        arr.Set(2, 100);
        arr.PrintArray();
    }
}

