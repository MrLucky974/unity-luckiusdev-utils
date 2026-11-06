namespace LuckiusDev.Utils
{
    public class Heap<T> where T : IHeapItem<T>
    {
        private readonly T[] m_items;
        private int m_currentItemCount;

        public Heap(int maxHeapSize)
        {
            m_items = new T[maxHeapSize];
        }

        public void Add(T item)
        {
            item.HeapIndex = m_currentItemCount;
            m_items[m_currentItemCount] = item;
            SortUp(item);
            m_currentItemCount++;
        }

        public T RemoveFirst()
        {
            T firstItem = m_items[0];
            m_currentItemCount--;
            m_items[0] = m_items[m_currentItemCount];
            m_items[0].HeapIndex = 0;
            SortDown(m_items[0]);
            return firstItem;
        }

        public void UpdateItem(T item)
        {
            SortUp(item);
        }

        public int Count
        {
            get
            {
                return m_currentItemCount;
            }
        }

        public bool Contains(T item)
        {
            return Equals(m_items[item.HeapIndex], item);
        }

        private void SortDown(T item)
        {
            while (true)
            {
                int childIndexLeft = item.HeapIndex * 2 + 1;
                int childIndexRight = item.HeapIndex * 2 + 2;

                if (childIndexLeft < m_currentItemCount)
                {
                    var swapIndex = childIndexLeft;

                    if (childIndexRight < m_currentItemCount)
                    {
                        if (m_items[childIndexLeft].CompareTo(m_items[childIndexRight]) < 0)
                        {
                            swapIndex = childIndexRight;
                        }
                    }

                    if (item.CompareTo(m_items[swapIndex]) < 0)
                    {
                        Swap(item, m_items[swapIndex]);
                    }
                    else
                    {
                        return;
                    }

                }
                else
                {
                    return;
                }

            }
        }

        private void SortUp(T item)
        {
            int parentIndex = (item.HeapIndex - 1) / 2;

            while (true)
            {
                T parentItem = m_items[parentIndex];
                if (item.CompareTo(parentItem) > 0)
                {
                    Swap(item, parentItem);
                }
                else
                {
                    break;
                }

                parentIndex = (item.HeapIndex - 1) / 2;
            }
        }

        private void Swap(T itemA, T itemB)
        {
            m_items[itemA.HeapIndex] = itemB;
            m_items[itemB.HeapIndex] = itemA;
            (itemA.HeapIndex, itemB.HeapIndex) = (itemB.HeapIndex, itemA.HeapIndex);
        }
    }
}