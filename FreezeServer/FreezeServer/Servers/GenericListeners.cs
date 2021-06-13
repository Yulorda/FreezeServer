using System;
using System.Collections.Generic;

public class GenericListeners
{
    private interface IGenericListener
    {
        void Invoke(int id, object obj);
    }

    private class GenericListener<T> : IGenericListener where T : class
    {
        public Action<int, T> listeners;

        public void AddListener(Action<int, T> listener)
        {
            listeners -= listener;
            listeners += listener;
        }

        public void RemoveListener(Action<int, T> listener)
        {
            listeners -= listener;
        }

        public void Invoke(int id, object obj)
        {
            listeners?.Invoke(id, obj as T);
        }
    }

    private Dictionary<Type, IGenericListener> listeners = new Dictionary<Type, IGenericListener>();

    public void AddListener<T>(Action<int, T> handler) where T : class
    {
        Type t = typeof(T);
        IGenericListener listener;

        if (listeners.ContainsKey(t))
        {
            listener = listeners[t];
        }
        else
        {
            listener = new GenericListener<T>();
            listeners.Add(t, listener);
        }

        (listener as GenericListener<T>).AddListener(handler);
    }

    public void RemoveListener<T>(Action<int, T> handler) where T : class
    {
        Type t = typeof(T);
        if (listeners.ContainsKey(t))
        {
            (listeners[t] as GenericListener<T>).RemoveListener(handler);
        }
    }

    public void RemoveAllListeners()
    {
        listeners.Clear();
    }

    public void Invoke<T>(int id, T obj)
    {
        Invoke(id, obj, typeof(T));
    }

    public void Invoke(int id, object obj, Type t)
    {
        if (listeners.TryGetValue(t, out IGenericListener listener))
        {
            listener.Invoke(id, obj);
        }
    }

    public void InvokeAll(int id, object obj)
    {
        foreach (var pair in listeners)
        {
            pair.Value?.Invoke(id, obj);
        }
    }
}
