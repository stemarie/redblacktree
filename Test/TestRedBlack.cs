using System;
using System.Collections.Generic;
using System.Collections.Generic.RedBlack;

namespace Test
{
    sealed class TestRedBlack
    {
        static readonly RedBlack<MyObj> redBlack = new RedBlack<MyObj>();

        static public void Main()
        {
            // create MyObjs containing key and string data
            MyObj obj1 = new MyObj("MyObj 1");
            MyObj obj2 = new MyObj("MyObj 2");
            MyObj obj3 = new MyObj("MyObj 3");
            MyObj obj4 = new MyObj("MyObj 4");
            MyObj obj5 = new MyObj("MyObj 5");
            MyObj obj6 = new MyObj("MyObj 6");
            MyObj obj7 = new MyObj("MyObj 7");
            MyObj obj8 = new MyObj("MyObj 8");
            MyObj obj9 = new MyObj("MyObj 9");
            MyObj obj10 = new MyObj("MyObj 10");
            MyObj obj11 = new MyObj("MyObj 11");
            MyObj obj12 = new MyObj("MyObj 12");
            MyObj obj13 = new MyObj("MyObj 13");

            try
            {
                // format: Add(key, value)
                redBlack.Add(obj1);
                redBlack.Add(obj2);
                redBlack.Add(obj3);
                redBlack.Add(obj4);
                redBlack.Add(obj5);
                redBlack.Add(obj6);
                redBlack.Add(obj7);
                redBlack.Add(obj8);
                redBlack.Add(obj9);
                redBlack.Add(obj10);
                redBlack.Add(obj11);
                redBlack.Add(obj12);
                redBlack.Add(obj13);

                Console.WriteLine(Environment.NewLine);

                IEnumerator<MyObj> items = redBlack.GetEnumerator();

                Enumerate(items);

                Console.WriteLine(Environment.NewLine);

                DumpMinMaxValue();
                Console.WriteLine(Environment.NewLine);

                MyKey tObjKey = (MyKey)redBlack.GetMinKey();
                MyObj tObj = redBlack.GetData(tObjKey);
                Console.WriteLine(@"Remove Min Key: " + tObj.Data);
                Console.WriteLine(Environment.NewLine);
                redBlack.Remove(tObjKey);

                Console.WriteLine(Environment.NewLine);

                Console.WriteLine(@"Remove Max Value:" + redBlack.GetMaxValue());
                redBlack.RemoveMax();
                Console.WriteLine(@"Remove Min Value:" + redBlack.GetMinValue());
                redBlack.RemoveMin();
                Console.WriteLine(Environment.NewLine);

                Console.WriteLine(Environment.NewLine);

                Console.WriteLine(@"Remove Min Key:" + redBlack.GetMinKey());
                redBlack.RemoveMin();
                Console.WriteLine(@"Remove Max Key:" + redBlack.GetMaxKey());
                redBlack.RemoveMax();

                Console.WriteLine(Environment.NewLine);

                Console.WriteLine(@"** Clearing Tree **");
                redBlack.Clear();
                Console.WriteLine(Environment.NewLine);

                Console.WriteLine(@"Press enter to terminate");
                Console.ReadLine();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(@"Press enter to terminate");
                Console.ReadLine();
            }
        }

        private static void Enumerate(IEnumerator<MyObj> items)
        {
            Console.WriteLine(@"Enumerating");
            try
            {
                items.MoveNext();
                while (items.Current != null)
                {
                    Console.WriteLine(@"{0}", items.Current.Data);
                    items.MoveNext();
                }
            }
            catch (Exception)
            { }
        }

        public static void DumpMinMaxValue()
        {
            Console.WriteLine(@"** Dumping Min/Max Values  **");
            Console.WriteLine(@"Min MyObj value: " + redBlack.GetMinValue().Data);
            Console.WriteLine(@"Max MyObj value: " + redBlack.GetMaxValue().Data);
            Console.WriteLine(@"Min MyObj key: " + redBlack.GetMinKey());
            Console.WriteLine(@"Max MyObj key: " + redBlack.GetMaxKey());
        }
    }
}
