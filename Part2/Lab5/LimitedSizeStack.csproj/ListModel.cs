using System;
using System.Collections.Generic;
using System.Linq;

namespace TodoApplication
{
    abstract public class Command<T>
    {
        public Command(ListModel<T> model)
        {
            items = model.Items;
        }

        protected List<T> items;

        abstract public void Do();

        abstract public void Undo();
    }

    public class CommandAdd<T> : Command<T>
    {
        private readonly T item;

        private readonly int index;

        public CommandAdd(ListModel<T> model, T itemToAdd) : base(model)
        {
            item = itemToAdd;
            index = items.Count;
        }

        override public void Do()
        {
            items.Add(item);
        }

        override public void Undo()
        {
            items.RemoveAt(index);
        }
    }

    public class CommandRemove<T> : Command<T>
    {
        private readonly T item;

        private readonly int index;

        public CommandRemove(ListModel<T> model, int indexToRemove) : base(model)
        {
            items = model.Items;
            index = indexToRemove;
            item = items[index];
        }

        override public void Do()
        {
            items.RemoveAt(index);
        }

        override public void Undo()
        {
            items.Insert(index,item);
        }
    }

    public class ListModel<TItem>
    {
        public List<TItem> Items { get; private set; }

        public int Limit { get; private set; }

        private LimitedSizeStack<Command<TItem>> commandStack;

        public ListModel(int limit)
        {
            Items =  new List<TItem>();
            Limit = limit;
            commandStack = new LimitedSizeStack<Command<TItem>>(limit);
        }

        public void AddItem(TItem item)
        {
            var commandAdd = new CommandAdd<TItem>(this, item);
            commandAdd.Do();
            commandStack.Push(commandAdd);
        }

        public void RemoveItem(int index)
        {
            var commandRemove = new CommandRemove<TItem>(this, index);
            commandRemove.Do();
            commandStack.Push(commandRemove);
        }

        public bool CanUndo()
        {
            return commandStack.Count > 0;
        }

        public void Undo()
        {
            var commandToUndo = commandStack.Pop();
            commandToUndo.Undo();
        }
    }
}
