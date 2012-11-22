// The RedBlackException class distinguishes read black tree exceptions from .NET
// exceptions. 

using System.Runtime.Serialization;

namespace System.Collections.Generic.RedBlack
{
    [Serializable]
    public class RedBlackException : Exception
    {
        public RedBlackException()
        { }

        public RedBlackException(string message)
            : base(message)
        { }

        public RedBlackException(string message, Exception exception)
            : base(message, exception)
        { }

        protected RedBlackException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        { }
    }
}
