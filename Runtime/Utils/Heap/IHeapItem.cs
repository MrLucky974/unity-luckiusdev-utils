using System;

namespace LuckiusDev.Utils.Heap
{
    public interface IHeapItem<T> : IComparable<T>
    {
        int HeapIndex { get; set; }
    }
}