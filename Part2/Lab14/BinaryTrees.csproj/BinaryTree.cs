using System;
using System.Collections;
using System.Collections.Generic;

namespace BinaryTrees
{
    public class BinaryTree<T> : IEnumerable<T>
        where T : IComparable
    {
        private int count;

        private Node currentNode;

        private Node root;

        public T this[int i]
        {
            get
            {
                currentNode = root;
                if (currentNode == null || currentNode.Size <= i)
                    throw new IndexOutOfRangeException();

                var indexCounter = 0;
                var currentI = currentNode.Left?.Size ?? 0;
                while (true)
                {
                    if (currentI == i)
                        return currentNode.Value;

                    if (currentI > i)
                    {
                        currentNode = currentNode.Left;
                        currentI = (currentNode.Left?.Size ?? 0) + indexCounter;
                    }
                    else
                    {
                        currentNode = currentNode.Right;
                        indexCounter = currentI + 1;
                        currentI = indexCounter + (currentNode.Left?.Size ?? 0);
                    }
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (var i = 0; i < count; i++)
                yield return this[i];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(T value)
        {
            count++;
            currentNode = root;
            if (root == null)
                root = new Node(value);
            else
                while (true)
                {
                    currentNode.Size++;
                    if (currentNode.Value.CompareTo(value) > 0)
                    {
                        if (TryAddNextNode(value, Direction.Left))
                            break;
                    }
                    else
                    {
                        if (TryAddNextNode(value, Direction.Right))
                            break;
                    }
                }
        }

        private bool TryAddNextNode(T value, Direction direction)
        {
            if (Direction.Left == direction)
                return TryGoLeft(value);

            if (Direction.Right == direction)
                return TryGoRight(value);

            return false;
        }

        private bool TryGoRight(T value)
        {
            if (currentNode.Right == null)
            {
                currentNode.Right = new Node(value);
                return true;
            }

            currentNode = currentNode.Right;
            return false;
        }

        private bool TryGoLeft(T value)
        {
            if (currentNode.Left == null)
            {
                currentNode.Left = new Node(value);
                return true;
            }

            currentNode = currentNode.Left;
            return false;
        }

        public bool Contains(T value)
        {
            if (root == null)
                return false;

            currentNode = root;
            while (true)
            {
                var compareValue = currentNode.Value.CompareTo(value);
                if (compareValue == 0)
                    return true;
                if (!TryTakeNextNode(compareValue))
                    return false;
            }
        }

        private bool TryTakeNextNode(int compareValue)
        {
            if (compareValue > 0)
            {
                if (currentNode.Left == null)
                    return false;

                currentNode = currentNode.Left;
            }
            else
            {
                if (currentNode.Right == null)
                    return false;

                currentNode = currentNode.Right;
            }

            return true;
        }

        private enum Direction
        {
            Left,
            Right
        }

        private sealed class Node
        {
            public Node(T value)
            {
                Value = value;
                Size = 1;
            }

            public T Value { get; }

            public Node Left { get; set; }

            public Node Right { get; set; }

            public int Size { get; set; }
        }
    }
}