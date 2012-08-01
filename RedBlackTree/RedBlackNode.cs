namespace System.Collections.Generic.RedBlack
{
    /// <summary>
    /// The RedBlackNode class encapsulates a node in the tree
    /// </summary>
    internal class RedBlackNode<K, T>
        where T : class
        where K : IComparable
    {
        public T Data { get; set; }

        public K Key { get; set; }

        internal RedBlackTree<K, T>.RedBlackNodeType Color { get; set; }

        public RedBlackNode<K, T> Left { get; set; }

        public RedBlackNode<K, T> Right { get; set; }

        public RedBlackNode<K, T> Parent { get; set; }

        public RedBlackNode()
        {
            Color = RedBlackTree<K, T>.RedBlackNodeType.Red;

            Right = RedBlackTree<K, T>.SentinelNode;
            Left = RedBlackTree<K, T>.SentinelNode;
        }

        public RedBlackNode(K key, T data)
            : this()
        {
            Key = key;
            Data = data;
        }
    }
}
