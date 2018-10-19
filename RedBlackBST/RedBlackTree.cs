using System;
using System.Diagnostics;

namespace RedBlackBST
{
    [DebuggerDisplay("Root: {root}")]
    public class RedBlackTree<T>
    {
        public Node root;

        private readonly Comparison<T> comparison;

        public int Count { get; private set; }

        public RedBlackTree(Comparison<T> comparison)
        {
            Count = 0;
            root = null;
            this.comparison = comparison;
        }

        public void Add(T value)
        {
            root = Add(root, value);
            Count++;
            root.IsRed = false;
        }

        private Node Add(Node node, T value)
        {
            if (node == null) return new Node(value);

            if (IsRed(node.Right)) FlipColors(node);


            if (comparison(value, node.Value) >= 0)
            {
                node.Right = Add(node.Right, value);
            }
            else
            {
                node.Left = Add(node.Left, value);
            }

            if (IsRed(node.Right)) node = Rotate(node, Direction.LEFT);

            if (IsRed(node.Left) && IsRed(node.Left.Left)) node = Rotate(node, Direction.RIGHT);

            return node;
        }

        private Node Rotate(Node node, Direction d)
        {
            Node temp;

            switch (d)
            {
                case Direction.LEFT:
                    temp = node.Right;
                    node.Right = temp.Left;
                    temp.Left = node;
                    break;
                case Direction.RIGHT:
                    temp = node.Left;
                    node.Left = temp.Right;
                    temp.Right = node;
                    break;
                default:
                    throw new ArgumentException();
            }

            temp.IsRed = node.IsRed;
            node.IsRed = true;

            return temp;
        }

        public bool Contains(T value)
        {
            Node temp = root;
            while (temp != null && comparison(value, temp.Value) != 0)
            {
                if (comparison(value, temp.Value) < 0)
                {
                    temp = temp.Left;
                }
                else
                {
                    temp = temp.Right;
                }
            }

            return temp != null;
        }

        private void FlipColors(Node node)
        {
            node.IsRed = !node.IsRed;

            if(node.Left != null) node.Left.IsRed = !node.Left.IsRed;
            if(node.Right != null) node.Right.IsRed = !node.Right.IsRed;
        }

        private bool IsRed(Node node)
        {
            if (node == null)
            {
                return false;
            }

            return node.IsRed;
        }

        public bool Remove(T value)
        {
            if (!Contains(value))
            {
                return false;
            }

            int oldCount = Count;

            root = Remove(root, value);
            root.IsRed = false;

            return oldCount != Count;
        }

        private Node Remove(Node node, T value)
        {
            if(comparison(value, node.Value) < 0)
            {
                if(IsTwoNode(node.Left))
                {
                    node = MoveRed(node, Direction.LEFT);
                }

                node.Left = Remove(node.Left, value);
            }
            else
            {
                if (IsRed(node.Left))
                {
                    node = Rotate(node, Direction.RIGHT);
                }

                if(comparison(value, node.Value) == 0 && node.Left == null && node.Right == null)
                {
                    Count--;
                    return null;
                }

                if (node.Right != null)
                {
                    if (IsTwoNode(node.Right))
                    {
                        node = MoveRed(node, Direction.RIGHT);
                    }
                    if (comparison(value, node.Value) == 0)
                    {
                        Node temp = node.Right;
                        while (temp.Left != null)
                        {
                            temp = temp.Left;
                        }
                        node.Value = temp.Value;

                        node.Right = Remove(node.Right, temp.Value);
                    }
                    else
                    {
                        node.Right = Remove(node.Right, value);
                    }
                }
            }

            return Fixup(node);
        }

        private Node Fixup(Node node)
        {
            if (IsRed(node.Right))
            {
                node = Rotate(node, Direction.LEFT);
            }
            
            if (node.Left != null && IsRed(node.Left) && IsRed(node.Left.Left))
            {
                node = Rotate(node, Direction.RIGHT);
            }

            if(IsRed(node.Left) && IsRed(node.Right))
            {
                FlipColors(node);
            }

            if(node.Left != null && IsRed(node.Left.Right))
            {
                node.Left = Fixup(node.Left);
            }

            return node;
        }

        private bool IsTwoNode(Node node)
        {
            if (node == null)
            {
                return false;
            }
            return !IsRed(node.Left);
        }

        private Node MoveRed(Node node, Direction d)
        {
            FlipColors(node);

            switch (d)
            {
                case Direction.LEFT:
                    if (IsRed(node?.Right?.Left))
                    {
                        node.Right = Rotate(node.Right, Direction.RIGHT);
                        node = Rotate(node, Direction.LEFT);
                        FlipColors(node);
                    }
                    break;

                case Direction.RIGHT:
                    if (IsRed(node?.Left?.Left))
                    {
                        node = Rotate(node, Direction.RIGHT);
                        FlipColors(node);
                    }
                    break;

                default:
                    throw new ArgumentException();
            }

            return node;
        }

        [DebuggerDisplay("Value: {Value}, Color: {IsRed ? \"Red\" : \"Black\"}, Left: {Left}, Right: {Right}")]
        public class Node
        {
            public Node Left { get; set; }
            public Node Right { get; set; }
            public T Value { get; set; }
            public bool IsRed;
            public Node(T value)
            {
                Value = value;
                IsRed = true;
            }
        }

        public enum Direction { LEFT, RIGHT }
    }
}
