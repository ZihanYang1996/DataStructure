public class Queue<T>
{
    private LinkedList<T> _list = new LinkedList<T>();

    public void Enqueue(T item)
    {
        _list.AddLast(item);
    }

    public T Dequeue()
    {
        if (_list.Count == 0)
        {
            throw new InvalidOperationException("Queue is empty");
        }
        T value = _list.First.Value;
        _list.RemoveFirst();
        return value;
    }

    public T Peek()
    {
        if (_list.Count == 0)
        {
            throw new InvalidOperationException("Queue is empty");
        }
        return _list.First.Value;
    }
    
    public bool IsEmpty()
    {
        return _list.Count == 0;
    }
}