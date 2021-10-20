using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApplication
{
    class StackItem<T>
    {
        public StackItem(T item)
        {
            Value = item;
        }

        public StackItem<T> PreviosItem;
        public StackItem<T> NextItem;
        public T Value;
    }

    public class LimitedSizeStack<T>
    {
        private readonly int limit;

        StackItem<T> LastItem { get; set; }

        public int Count { get; set; }

        StackItem<T> FirstItem { get; set; }

        public LimitedSizeStack(int limit)
        {
            this.limit = limit;
        }

        public void Push(T item)
        {
            var newItem = new StackItem<T>(item);
            if (Count == 0)
            {
                LastItem = FirstItem = newItem;
            }
            else if (Count == limit)
            {
                FirstItem = FirstItem.NextItem;
                FirstItem.PreviosItem = null;
                Count--;
            }
            newItem.PreviosItem = LastItem;
            LastItem.NextItem = newItem;
            LastItem = newItem;
            Count++;
        }

        public T Pop()
        {
            if (Count == 0)
                throw new Exception();
            var value = LastItem.Value;
            if (Count == 1)
            {
                LastItem = null;
                FirstItem = null;
            }
            else
            {
                LastItem = LastItem.PreviosItem;
                LastItem.NextItem = null;
            }
            Count--;
            return value;
        }
    }
}
