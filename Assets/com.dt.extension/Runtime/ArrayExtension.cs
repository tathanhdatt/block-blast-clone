using System.Collections.Generic;

namespace Dt.Extension
{
    public static class ArrayExtension
    {
        public static void Clear<T>(this T[] array) where T : struct
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = default;
            }
        }

        public static void CleanUp<T>(this T[] array) where T : class
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = null;
            }
        }

        public static bool IsEmpty<T>(this T[] array)
        {
            return array.Length == 0;
        }

        public static T First<T>(this T[] array)
        {
            return array[0];
        }

        public static T Last<T>(this T[] array)
        {
            return array[array.Length - 1];
        }

        public static void Sort<T>(this T[] array, IComparer<T> comparer)
        {
            for (int i = 0; i < array.Length; i++)
            {
                for (int j = i + 1; j < array.Length - 1; j++)
                {
                    if (comparer.Compare(array[i], array[j]) > 1)
                    {
                        (array[i], array[j]) = (array[j], array[i]);
                    }
                }
            }
        }
    }
}