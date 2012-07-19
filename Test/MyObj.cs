using System;

namespace Test
{
    public class MyObj : IComparable<MyObj>, IComparable
    {
        private string _myData;

        public string Data
        {
            get
            {
                return _myData;
            }

            set
            {
                _myData = value;
            }
        }

        public MyObj(string data)
        {
            Data = data;
        }

        public int CompareTo(MyObj other)
        {
            return String.CompareOrdinal(_myData, other.Data);
        }

        public override string ToString()
        {
            return _myData;
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance is less than <paramref name="obj"/>. Zero This instance is equal to <paramref name="obj"/>. Greater than zero This instance is greater than <paramref name="obj"/>. 
        /// </returns>
        /// <param name="obj">An object to compare with this instance. </param><exception cref="T:System.ArgumentException"><paramref name="obj"/> is not the same type as this instance. </exception><filterpriority>2</filterpriority>
        public int CompareTo(object obj)
        {
            return String.CompareOrdinal(_myData, (obj as MyObj).Data);
        }
    }
}
