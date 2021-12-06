using System.Collections.Generic;
using ToDoTask;
using System;

namespace LeftToDo
{
    static class LogicalExtensions
    {
        public static bool TryCountBasedIndex<T>(this List<T> list, string input, out T item)
        {
            try
            {
                int index = Convert.ToInt32(input);
                item = list[--index];
                return true;
            }
            catch
            {
                item = default(T);
                return false;
            }
        }
    }
}