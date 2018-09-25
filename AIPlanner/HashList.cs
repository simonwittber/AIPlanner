using System.Collections.Generic;
using System.Linq;


namespace AIPlanner
{
    class HashList<T>
    {
        List<T> list = new List<T>();
        HashSet<T> hashSet = new HashSet<T>();

        public int Count { get => list.Count; }

        public T this[int key]
        {
            get => list[key];
            set
            {
                list[key] = value;
                hashSet.Add(value);
            }
        }

        public List<T> ToList()
        {
            return list.ToList();
        }

        public T[] ToArray()
        {
            return list.ToArray();
        }

        public Dictionary<K, T> ToDictionary<K>(System.Func<T, K> keySelector)
        {
            return list.ToDictionary(keySelector);
        }

        public void Add(T item)
        {
            if (hashSet.Add(item))
                list.Add(item);
        }

        public void Remove(T item)
        {
            list.Remove(item);
            hashSet.Remove(item);
        }

        public bool Contains(T item)
        {
            return hashSet.Contains(item);
        }

        public void Clear()
        {
            list.Clear();
            hashSet.Clear();
        }
    }
}


