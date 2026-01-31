using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

public class EventfulQueue<T>
{
    public event Action<T> OnItemQueued;
    public event Action<T> OnItemDequeued;
    
    private Queue<T> _queue = new Queue<T>();

    public void Enqueue(T obj)
    {
        _queue.Enqueue(obj);
        OnItemQueued?.Invoke(obj);
    }
    
    public T Dequeue()
    {
        var obj = _queue.Dequeue();
        OnItemDequeued?.Invoke(obj);
        return obj;
    }

    public List<T> ItemsAsList()
    {
        return _queue.ToArray().ToList();
    }

    public T Peek() => _queue.Peek();
    public int Count => _queue.Count;
}
