using System;

namespace Test
{
    //
    // demonstrates using a key as a separate object
    //

    public class MyKey : IComparable
    {
        private int intMyKey;
        public int Key
        {
            get
            {
                return intMyKey;
            }

            set
            {
                intMyKey = value;
            }
        }

        public MyKey(int key)
        {
            intMyKey = key;
        }

        public int CompareTo(object key)
        {
            return Key > ((MyKey)key).Key
                       ? 1
                       : (Key < ((MyKey)key).Key
                              ? -1
                              : 0);
        }

        public override string ToString()
        {
            return intMyKey.ToString();
        }
    }
}