using System;

namespace RedBlackBST
{
    class Program
    {
        static void Main(string[] args)
        {
            RedBlackTree<int> tree = new RedBlackTree<int>((x, y) => x - y);

            for (int i = 1; i <= 10; i++)
            {
                tree.Add(i);
            }

            for (int i = 1; i <= 8; i++)
            {
                tree.Remove(i);
            }

            tree.Remove(7);
            tree.Remove(10);
            Console.ReadKey();
        }
    }
}
