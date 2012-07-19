namespace System.Collections.Generic.RedBlack
{
    /// <summary>
    /// The RedBlackNode class encapsulates a node in the tree
    /// </summary>
    internal class RedBlackNode<T> 
        where T : class, IComparable
    {
        public T Data { get; set; }

        public RedBlackTree<T>.RedBlackNodeType Color { get; set; }

        public RedBlackNode<T> Left { get; set; }

        public RedBlackNode<T> Right { get; set; }

        public RedBlackNode<T> Parent { get; set; }

        public RedBlackNode()
        {
            Color = RedBlackTree<T>.RedBlackNodeType.Red;

            Right = RedBlackTree<T>.SentinelNode;
            Left = RedBlackTree<T>.SentinelNode;
        }

        public RedBlackNode(T data)
            : this()
        {
            Data = data;
        }
    }
}
