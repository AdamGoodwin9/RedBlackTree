using System;
using System.Diagnostics;

namespace RedBlackBST
{
    [DebuggerDisplay("Root: {root}")]
    public class RedBlackTree<T>
    {
        public Node root;

        private readonly Comparison<T> comparison;
        public RedBlackTree(Comparison<T> comparison)
        {
            this.comparison = comparison;
            root = null;
        }

        public void Add(T value)
        {
            root = Add(root, value);
            root.IsRed = false;
        }

        private Node Add(Node node, T value)
        {
            if (node == null) return new Node(value);

            if(IsRed(node.Right)) FlipColors(node);


            if (comparison(value, node.Value) >= 0)
            {
                node.Right = Add(node.Right, value);
            }
            else
            {
                node.Left = Add(node.Left, value);
            }

            if(IsRed(node.Right)) node = Rotate(node, Direction.LEFT);

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

        private void FlipColors(Node node)
        {
            node.IsRed = !node.IsRed;
            node.Left.IsRed = !node.Left.IsRed;
            node.Right.IsRed = !node.Right.IsRed;
        }

        private bool IsRed(Node node)
        {
            if(node == null)
            {
                return false;
            }

            return node.IsRed;
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
