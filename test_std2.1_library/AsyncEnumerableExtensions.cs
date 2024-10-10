using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

public static class AsyncEnumerableExtensions
{
    private static async Task<List<T>> GetElement<T>(IAsyncEnumerable<T> asyncEnumerable)
    {
        var resultList = new List<T>();
        await foreach (T item in asyncEnumerable)
        {
            resultList.Add(item);
        }
        return resultList;
    }

    public static IEnumerable<T> ToEnumerable<T>(this IAsyncEnumerable<T> asyncEnumerable)
    {
        if (asyncEnumerable == null)
        {
            throw new ArgumentNullException(nameof(asyncEnumerable));
        }

        var task = Task.Run(() => GetElement(asyncEnumerable));

        return task.GetAwaiter().GetResult();
    }
}