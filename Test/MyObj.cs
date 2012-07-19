using System;

namespace Test
{
    public class MyObj : IComparable<MyObj>
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
            return _myData.CompareTo(other);
        }

        public override string ToString()
        {
            return _myData;
        }
    }
}
