// The RedBlackException class distinguishes read black tree exceptions from .NET
// exceptions. 

namespace System.Collections.Generic.RedBlack
{
    public class RedBlackException : Exception
    {
        public RedBlackException()
        { }

        public RedBlackException(string msg)
            : base(msg)
        { }
    }
}
