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

        public RedBlackTree<K, T>.RedBlackNodeType Color { get; set; }

        public RedBlackNode<K, T> Left { get; set; }

        public RedBlackNode<K, T> Right { get; set; }

        public RedBlackNode<K, T> Parent { get; set; }

        public RedBlackNode()
        {
            Color = RedBlackTree<K, T>.RedBlackNodeType.Red;

            Right = RedBlackTree<K, T>.SentinelNode;
            Left = RedBlackTree<K, T>.SentinelNode;
        }

        public RedBlackNode<K, T> DirectionTrue(Direction direction)
        {
            switch (direction)
            {
                case Direction.Right:
                    return Right;
                case Direction.Left:
                    return Left;
                default:
                    return null;
            }
        }

        public RedBlackNode<K, T> DirectionOpposite(Direction direction)
        {
            switch (direction)
            {
                case Direction.Right:
                    return Left;
                case Direction.Left:
                    return Right;
                default:
                    return null;
            }
        }

        public RedBlackNode(K key, T data)
            : this()
        {
            Key = key;
            Data = data;
        }
    }
}
