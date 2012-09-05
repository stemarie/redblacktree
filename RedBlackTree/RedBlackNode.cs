namespace System.Collections.Generic.RedBlack
{
    /// <summary>
    /// The RedBlackNode class encapsulates a node in the tree
    /// </summary>
    internal class RedBlackNode<TKey, TValue>
        where TValue : class
        where TKey : IComparable
    {
        public TValue Data { get; set; }

        public TKey Key { get; set; }

        internal RedBlackTree<TKey, TValue>.RedBlackNodeType Color { get; set; }

        public RedBlackNode<TKey, TValue> Left { get; set; }

        public RedBlackNode<TKey, TValue> Right { get; set; }

        public RedBlackNode<TKey, TValue> Parent { get; set; }

        public RedBlackNode()
        {
            Color = RedBlackTree<TKey, TValue>.RedBlackNodeType.Red;

            Right = RedBlackTree<TKey, TValue>.SentinelNode;
            Left = RedBlackTree<TKey, TValue>.SentinelNode;
        }

        public RedBlackNode(TKey key, TValue data)
            : this()
        {
            Key = key;
            Data = data;
        }
    }
}
