namespace System.Collections.Generic.RedBlack
{
    /// <summary>
    /// The RedBlackNode class encapsulates a node in the tree
    /// </summary>
    public class RedBlackNode<T> where T : class, IComparable<T>
    {
        public T Data { get; set; }

        public RedBlack<T>.RedBlackNodeType Color { get; set; }

        public RedBlackNode<T> Left { get; set; }

        public RedBlackNode<T> Right { get; set; }

        public RedBlackNode<T> Parent { get; set; }

        public RedBlackNode()
        {
            Color = RedBlack<T>.RedBlackNodeType.Red;

            Right = RedBlack<T>.SentinelNode;
            Left = RedBlack<T>.SentinelNode;
        }

        public RedBlackNode(T data)
            : this()
        {
            Data = data;
        }
    }
}
