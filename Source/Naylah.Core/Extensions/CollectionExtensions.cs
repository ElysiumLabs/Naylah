using System;
using System.Collections.Generic;

namespace Naylah.Extensions
{
    public static class CollectionExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            if (items == null) return;
            foreach (var item in items) action(item);
        }

        //public static void ForEach<T>(this PersistableScalarCollection<T> items, Action<T> action)
        //{
        //    if (items == null) return;
        //    foreach (var item in items) action(item);
        //}
    }
}