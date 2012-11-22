using System;
using System.Collections.Generic;
using System.Collections.Generic.RedBlack;

namespace Performance.RedBlack
{
    class Program
    {
        static void Main(string[] args)
        {
            var stack = new Stack<string>();
            var tree = new RedBlackTree<string, string>();

            for (int i = 0; i < 10000; i++)
            {
                stack.Push(Guid.NewGuid().ToString());
            }

            foreach (string item in stack)
            {
                tree.Add(item, item);
            }
        }
    }
}
