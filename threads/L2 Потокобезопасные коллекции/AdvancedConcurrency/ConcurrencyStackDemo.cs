using System.Text.RegularExpressions;
using System.Threading;

namespace AdvancedConcurrency
{
    public class ConcurrencyStackDemo<T>
    {
        private class Node
        {
            public readonly T Value;
            public readonly Node Next;

            public Node(T value, Node next)
            {
                Value = value;
                Next = next;
            }
        }

        private Node head = null;

        public void Push(T value)
        {
            while (true)
            {
                var node = new Node(value, head);
                if (Interlocked.CompareExchange(ref head, node, node.Next) == node.Next)
                    return;
            }
        }

        public bool TryPop(out T value)
        {
            value = default;
            while (true)
            {
                var node = head;
                if (node == null)
                    return false;

                if (Interlocked.CompareExchange(ref head, node.Next, node) == node)
                {
                    value = node.Value;
                    return true;
                }
            }
        }
    }
}