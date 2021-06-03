using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace TowerDefense
{
    struct ObjectPoolEqualityComparer<T> : IEqualityComparer<T>
        where T : class
    {
        public bool Equals(T x, T y)
        {
            return Object.ReferenceEquals(x, y);
        }

        public int GetHashCode(T obj)
        {
            return RuntimeHelpers.GetHashCode(obj);
        }
    }

    public sealed class ObjectPool<T>
        where T : class
    {
        #region Singleton
        public static ObjectPool<T> Instance { get; }

        static ObjectPool()
        {
            Instance = new ObjectPool<T>();
        }

        private ObjectPool() { }
        #endregion Singleton

        #region Storage

        private readonly Queue<T> items = new Queue<T>();
        Func<T> factory;

        HashSet<T> borrowedItems = new HashSet<T>(new ObjectPoolEqualityComparer<T>());

        #endregion

        #region Populate and Grow


        public void Populate(Func<T> objectFactory, int initialCount)
        {
            if (items.Count > 0) throw new InvalidOperationException("Populate can only be called once");

            for (int i = 0; i < initialCount; i++)
            {
                items.Enqueue(objectFactory());
            }
        }

        // TODONE: Write a borrow method
        // TODONE: Write a return method

        public T Borrow()
        {
            //TODONE: Make sure we have enough items to borrow one
            EnsureBorrow();

            var item = items.Dequeue();//storage[typeof(TSubType)].items.Dequeue() as TSubType;

            // Keep track of borrowed item
            borrowedItems.Add(item);

            return item;
        }

        public void Return(T item)
        {
            if (!borrowedItems.Contains(item)) throw new InvalidOperationException("Not my item!");

            Type itemType = item.GetType();

            borrowedItems.Remove(item);
            //storage[itemType].items.Enqueue(item);
            items.Enqueue(item);
        }

        private void EnsureBorrow(int increment = 10)
        {
            //Type type = typeof(TSubType);
            if (items.Count > 0) return;

            for (int i = 0; i < increment; i++)
            {
                items.Enqueue(factory());
            }

        }

        #endregion Populate and Grow
        

    }
}
