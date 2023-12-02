using System;
using System.Collections;

public class Stack<T>
{
    private List<T>? StackList;

    public Stack()
    {
        StackList = new List<T>();
    }

    public void Push (T item)
    {
        StackList.Add(item);
    }

    public T Pop()
    {
        if (StackList.Count == 0)
        {
            throw new InvalidOperationException("Stack is empty");
        }
        else
        {
            T item = StackList[StackList.Count - 1];
            StackList.RemoveAt(StackList.Count - 1);
            return item;
        }
    }
    public T Peek()
    {
        if (StackList.Count == 0)
        {
            throw new InvalidOperationException("Stack is empty");
        }

        return StackList[StackList.Count - 1];
    }

    public bool IsEmpty()
    {
        return StackList.Count == 0;
    }
}