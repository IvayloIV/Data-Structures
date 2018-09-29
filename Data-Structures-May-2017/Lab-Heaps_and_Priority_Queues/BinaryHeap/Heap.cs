using System;

public static class Heap<T> where T : IComparable<T>
{
    public static void Sort(T[] arr)
    {
        for (int i = arr.Length / 2 - 1; i >= 0; i--)
        {
            HeapifyDown(arr, i, arr.Length);
        }

        for (int i = arr.Length - 1; i >= 1; i--)
        {
            SwapIndexes(arr, 0, i);
            HeapifyDown(arr, 0, i);
        }
    }

    private static void HeapifyDown(T[] arr, int index, int lenght)
    {
        while (index < lenght / 2)
        {
            var childIndex = 2 * index + 1;
            if (childIndex + 1 < lenght && arr[childIndex].CompareTo(arr[childIndex + 1]) < 0)
            {
                childIndex++;
            }

            if (arr[index].CompareTo(arr[childIndex]) < 0)
            {
                SwapIndexes(arr, index, childIndex);
            }
            else
            {
                break;
            }

            index = childIndex;
        }
    }

    private static void SwapIndexes(T[] arr, int a, int b)
    {
        var temp = arr[a];
        arr[a] = arr[b];
        arr[b] = temp;
    }
}
