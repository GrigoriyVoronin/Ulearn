using System.Collections.Generic;

namespace rocket_bot
{
    public class Channel<T> where T : class
    {
        private readonly List<T> elements = new List<T>();

        /// <summary>
        ///     Возвращает элемент по индексу или null, если такого элемента нет.
        ///     При присвоении удаляет все элементы после.
        ///     Если индекс в точности равен размеру коллекции, работает как Append.
        /// </summary>
        public T this[int index]
        {
            get
            {
                lock (elements)
                {
                    return index < Count ? elements[index] : null;
                }
            }
            set
            {
                lock (elements)
                {
                    if (index == Count)
                    {
                        elements.Add(value);
                    }
                    else
                    {
                        elements[index] = value;
                        for (var i = Count - 1; i > index; i--)
                            elements.RemoveAt(i);
                    }
                }
            }
        }

        /// <summary>
        ///     Возвращает количество элементов в коллекции
        /// </summary>
        public int Count
        {
            get
            {
                lock (elements)
                {
                    return elements.Count;
                }
            }
        }

        /// <summary>
        ///     Возвращает последний элемент или null, если такого элемента нет
        /// </summary>
        public T LastItem()
        {
            lock (elements)
            {
                return Count > 0 ? elements[Count - 1] : null;
            }
        }

        /// <summary>
        ///     Добавляет item в конец только если lastItem является последним элементом
        /// </summary>
        public void AppendIfLastItemIsUnchanged(T item, T knownLastItem)
        {
            lock (elements)
            {
                if (Count == 0 && knownLastItem == null)
                    elements.Add(item);
                else if (elements[Count - 1] == knownLastItem)
                    elements.Add(item);
            }
        }
    }
}