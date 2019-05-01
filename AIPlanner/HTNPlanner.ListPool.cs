using System;
using System.Collections.Generic;

namespace AIPlanner
{
    public static partial class HTNPlanner
    {
        public static class ListPool<T>
        {
            static Stack<List<T>> stack = new Stack<List<T>>();

            public static List<T> Take()
            {
                lock (stack)
                {
                    if (stack.Count > 0)
                        return stack.Pop();
                }
                return new List<T>();
            }

            public static List<T> Take(IEnumerable<T> init)
            {
                var list = Take();
                list.AddRange(init);
                return list;
            }

            public static void Return(List<T> list)
            {
                list.Clear();
                lock (stack)
                {
                    stack.Push(list);
                }
            }
        }

        public static class StackPool<T>
        {
            static Stack<Stack<T>> stack = new Stack<Stack<T>>();

            public static Stack<T> Take()
            {
                lock (stack)
                {
                    if (stack.Count > 0)
                        return stack.Pop();
                }
                return new Stack<T>();
            }

            public static void Return(Stack<T> list)
            {
                list.Clear();
                lock (stack)
                {
                    stack.Push(list);
                }
            }
        }
    }
}