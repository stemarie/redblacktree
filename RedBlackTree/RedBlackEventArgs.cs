namespace System.Collections.Generic.RedBlack
{
    public class RedBlackEventArgs<K, T> : EventArgs
    {
        public K Key { get; set; }
        public T Item { get; set; }
    }
}