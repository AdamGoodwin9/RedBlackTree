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

            Console.ReadKey();
        }
    }
}
