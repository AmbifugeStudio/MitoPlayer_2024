using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace MitoPlayer_2024.Helpers
{
    public class ObservableConcurrentBag<T>
    {
        private ConcurrentBag<T> _bag = new ConcurrentBag<T>();

        public event EventHandler<ItemAddedEventArgs<T>> ItemAdded;

        public void Add(T item)
        {
            _bag.Add(item);
            OnItemAdded(item);
        }

        protected virtual void OnItemAdded(T item)
        {
            ItemAdded?.Invoke(this, new ItemAddedEventArgs<T>(item));
        }

        public IEnumerable<T> GetItems()
        {
            return _bag;
        }

        // Expose Count property
        public int Count => _bag.Count;

        // Expose IsEmpty property
        public bool IsEmpty => !_bag.Any();

        // Expose TryTake method
        public bool TryTake(out T item)
        {
            return _bag.TryTake(out item);
        }

        // Method to clear the bag
        public void Clear()
        {
            while (TryTake(out _)) { }
        }
    }


    public class ItemAddedEventArgs<T> : EventArgs
    {
        public T Item { get; }

        public ItemAddedEventArgs(T item)
        {
            Item = item;
        }
    }
}


