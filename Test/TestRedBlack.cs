using System;
using System.Collections.Generic;
using System.Collections.Generic.RedBlack;

namespace Test
{
    sealed class TestRedBlack
    {
        static readonly RedBlackTree<string, MyObj> RedBlackTree = new RedBlackTree<string, MyObj>();

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
                RedBlackTree.Add(obj1.Data, obj1);
                RedBlackTree.Add(obj2.Data, obj2);
                RedBlackTree.Add(obj3.Data, obj3);
                RedBlackTree.Add(obj4.Data, obj4);
                RedBlackTree.Add(obj5.Data, obj5);
                RedBlackTree.Add(obj6.Data, obj6);
                RedBlackTree.Add(obj7.Data, obj7);
                RedBlackTree.Add(obj8.Data, obj8);
                RedBlackTree.Add(obj9.Data, obj9);
                RedBlackTree.Add(obj10.Data, obj10);
                RedBlackTree.Add(obj11.Data, obj11);
                RedBlackTree.Add(obj12.Data, obj12);
                RedBlackTree.Add(obj13.Data, obj13);

                Console.WriteLine(Environment.NewLine);

                IEnumerator<MyObj> items = RedBlackTree.GetEnumerator();

                Enumerate(items);

                Console.WriteLine(Environment.NewLine);

                DumpMinMaxValue();
                Console.WriteLine(Environment.NewLine);

                string tObjKey = RedBlackTree.GetMinKey();
                MyObj tObj = RedBlackTree.GetData(tObjKey);
                Console.WriteLine(@"Remove Min Key: " + tObj.Data);
                Console.WriteLine(Environment.NewLine);
                RedBlackTree.Remove(tObjKey);

                Console.WriteLine(Environment.NewLine);

                Console.WriteLine(@"Remove Max Value:" + RedBlackTree.GetMaxValue());
                RedBlackTree.RemoveMax();
                Console.WriteLine(@"Remove Min Value:" + RedBlackTree.GetMinValue());
                RedBlackTree.RemoveMin();
                Console.WriteLine(Environment.NewLine);

                Console.WriteLine(Environment.NewLine);

                Console.WriteLine(@"Remove Min Key:" + RedBlackTree.GetMinKey());
                RedBlackTree.RemoveMin();
                Console.WriteLine(@"Remove Max Key:" + RedBlackTree.GetMaxKey());
                RedBlackTree.RemoveMax();
                
                Console.WriteLine(Environment.NewLine);

                Console.WriteLine(@"** Clearing Tree **");
                RedBlackTree.Clear();
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
            Console.WriteLine(@"Min MyObj value: " + RedBlackTree.GetMinValue().Data);
            Console.WriteLine(@"Max MyObj value: " + RedBlackTree.GetMaxValue().Data);
            Console.WriteLine(@"Min MyObj key: " + RedBlackTree.GetMinKey());
            Console.WriteLine(@"Max MyObj key: " + RedBlackTree.GetMaxKey());
        }
    }
}
