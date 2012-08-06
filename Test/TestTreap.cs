using System;
using TreapCS;

namespace Test
{
	sealed class TestTreap
	{
		static Treap treap = new Treap();
		
		static public void Main()
		{
			
			// create MyObjs containing key and string data
			MyObj obj1 = new MyObj(0003, "MyObj C");
			MyObj obj2 = new MyObj(0001, "MyObj A");
			MyObj obj3 = new MyObj(0002, "MyObj B");
			MyObj obj4 = new MyObj(0004, "MyObj D");
			MyObj obj5 = new MyObj(0005, "MyObj E");
			
			try
			{
                // format: Add(key, value)
				treap.Add(new MyKey(obj1.Key), obj1);
				Console.WriteLine("Added to Treap, key: " + obj1.ToString());
				treap.Add(new MyKey(obj2.Key), obj2);
				Console.WriteLine("Added to Treap, key: " + obj2.ToString());
				treap.Add(new MyKey(obj3.Key), obj3);
				Console.WriteLine("Added to Treap, key: " + obj3.ToString());
				treap.Add(new MyKey(obj4.Key), obj4);
				Console.WriteLine("Added to Treap, key: " + obj4.ToString());
				treap.Add(new MyKey(obj5.Key), obj5);
				Console.WriteLine("Added to Treap, key: " + obj5.ToString());
				Console.WriteLine(Environment.NewLine);
				
				DumpTreap(true);
				Console.WriteLine(Environment.NewLine);
				
				Console.WriteLine("- Treap Values -");
				TreapEnumerator t = treap.Values();
				while (t.MoveNext())
					Console.WriteLine(((MyObj)(t.Value)).Data);
				Console.WriteLine(Environment.NewLine);
				
				Console.WriteLine("- Treap Keys -");
				TreapEnumerator k = treap.Keys();
				while (k.MoveNext())
					Console.WriteLine(k.Key);
				Console.WriteLine(Environment.NewLine);
				    
				DumpMinMaxValue();
				Console.WriteLine(Environment.NewLine);
				
				// test Remove
				MyKey tObjKey   = (MyKey) treap.GetMinKey();
				MyObj tObj      = (MyObj) treap.GetData(tObjKey);
				Console.WriteLine("Remove Min Key: " + tObj.ToString());
				Console.WriteLine(Environment.NewLine);
				treap.Remove(tObjKey);
				DumpTreap(false);
				Console.WriteLine(Environment.NewLine);
				
				Console.WriteLine("Remove Max Value:" + treap.GetMaxValue().ToString());
				treap.RemoveMax();
				Console.WriteLine("Remove Min Value:" + treap.GetMinValue().ToString());
				treap.RemoveMin();
				Console.WriteLine(Environment.NewLine);
				
				DumpTreap(true);
				Console.WriteLine(Environment.NewLine);
				
				Console.WriteLine("Remove Min Key:" +((MyKey)(treap.GetMinKey())).ToString());
				treap.RemoveMin();
				Console.WriteLine("Remove Max Key:" +((MyKey)(treap.GetMaxKey())).ToString());
				treap.RemoveMax();
				
				Console.WriteLine(Environment.NewLine);
				DumpTreap(true);
				Console.WriteLine(Environment.NewLine);
				
				// add some more and clear the treap
				Console.WriteLine("- Adding MyKeyObjs - ");
				Console.WriteLine(Environment.NewLine);
				
				MyKeyObj myKeyObj1 = new MyKeyObj(0025, "MyKeyObj W");
				MyKeyObj myKeyObj2 = new MyKeyObj(0023, "MyKeyObj X");
				MyKeyObj myKeyObj3 = new MyKeyObj(0026, "MyKeyObj Y");
				MyKeyObj myKeyObj4 = new MyKeyObj(0024, "MyKeyObj Z");
				treap.Add(myKeyObj1.Key, myKeyObj1);
				Console.WriteLine("Added to Treap, key: " + myKeyObj1.ToString());
				treap.Add(myKeyObj2.Key, myKeyObj2);
				Console.WriteLine("Added to Treap, key: " + myKeyObj2.ToString());
				treap.Add(myKeyObj3.Key, myKeyObj3);
				Console.WriteLine("Added to Treap, key: " + myKeyObj3.ToString());
				treap.Add(myKeyObj4.Key, myKeyObj4);
				Console.WriteLine("Added to Treap, key: " + myKeyObj4.ToString());
				Console.WriteLine(Environment.NewLine);
				
				TraverseEnumerator();
				Console.WriteLine(Environment.NewLine);
				
				Console.WriteLine("- Clearing Treap -");
				Console.WriteLine(Environment.NewLine);
				treap.Clear();
				DumpTreap(true);
				Console.WriteLine(Environment.NewLine);
				
				Console.WriteLine("Press enter to terminate");
				Console.ReadLine();
				
			}
			catch (Exception ex)
			{
                Console.WriteLine(ex.Message);
                Console.WriteLine("Press enter to terminate");
                Console.ReadLine();
            }
		}
		public static void DumpTreap (bool boolDesc)
		{
			// returns keys only
			TreapEnumerator k = treap.Keys(boolDesc);
			// returns data only, in this case, MyObjs
			TreapEnumerator e = treap.Elements(boolDesc);
			
			if(boolDesc)
				Console.WriteLine("** Dumping Treap: Ascending **");
			else
				Console.WriteLine("** Dumping Treap: Descending **");
			
			Console.WriteLine("Treap Size: " + treap.Size().ToString() + Environment.NewLine);
			
			Console.WriteLine("- keys -");
			while (k.HasMoreElements())
				Console.WriteLine(k.NextElement());
			
			Console.WriteLine("- my objects -");
			MyObj cmmMyObj;
			while (e.HasMoreElements())
			{
				cmmMyObj = ((MyObj)(e.NextElement()));
				Console.Write("Key:" + cmmMyObj.ToString());
				Console.WriteLine(" Data:" + cmmMyObj.Data);
			}
			
		}
		public static void TraverseEnumerator ()
		{
			Console.WriteLine("** Traversing using Enumerator **");
			
			TreapEnumerator myEnumerator = treap.GetEnumerator();
			
			while (myEnumerator.MoveNext())
				Console.WriteLine("{0}:" + "{1}", myEnumerator.Key,((MyKeyObj)(myEnumerator.Value)).Data);
			
		}
		public static void DumpMinMaxValue ()
		{
			Console.WriteLine("** Dumping Min/Max Values  **");
			Console.WriteLine("Min MyObj value: " + ((MyObj) treap.GetMinValue()).Data);
			Console.WriteLine("Max MyObj value: " + ((MyObj)treap.GetMaxValue()).Data);
			Console.WriteLine("Min MyObj key: " + ((MyKey)treap.GetMinKey()).ToString());
			Console.WriteLine("Max MyObj key: " + ((MyKey)treap.GetMaxKey()).ToString());
		}
	}
}
