using System;
using System.Collections;
using System.Collections.Generic;

public static class IListExtension
{
    private static readonly Random Random = new Random();

    public static T RandomItem<T>(this IList<T> list)
    {
        if (list.IsEmpty())
        {
            throw new IndexOutOfRangeException();
        }

        return list[Random.Next(0, list.Count)];
    }

    public static T First<T>(this IList<T> list)
    {
        return list[0];
    }

    public static T Last<T>(this IList<T> list)
    {
        return list[list.Count - 1];
    }

    public static void RemoveFirst(this IList list)
    {
        list.RemoveAt(0);
    }

    public static void RemoveLast(this IList list)
    {
        list.RemoveAt(list.Count - 1);
    }

    public static void Shuffle(this IList list)
    {
        int numberItem = list.Count;
        for (int i = 0; i < numberItem; i++)
        {
            int randomIndex = Random.Next(0, numberItem);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }

    public static IList Concat(this IList list, IList other)
    {
        foreach (var item in other)
        {
            list.Add(item);
        }

        return list;
    }

    public static bool IsEmpty<T>(this IList<T> list)
    {
        return list.Count <= 0;
    }

    public static T RemoveRandomItem<T>(this IList<T> list)
    {
        if (list.IsEmpty())
        {
            throw new IndexOutOfRangeException();
        }

        int itemIndex = Random.Next(0, list.Count);
        T item = list[itemIndex];
        list.RemoveAt(itemIndex);
        return item;
    }
}