namespace System.Collections.Generic.RedBlack
{
    public class RedBlackEventArgs<TKey, TValue> : EventArgs
    {
        public TKey Key { get; set; }
        public TValue Item { get; set; }
    }
}