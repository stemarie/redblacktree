using System.Collections.Generic.RedBlack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace TestProject.RedBlackTree
{
    /// <summary>
    ///This is a test class for RedBlackTreeTest and is intended
    ///to contain all RedBlackTreeTest Unit Tests
    ///</summary>
    [TestClass]
    public class RedBlackTreeTest
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for RedBlackTree`1 Constructor
        ///</summary>
        public void RedBlackTreeConstructorTestHelper<T>()
            where T : class , IComparable
        {
            const string strIdentifier = "identifier"; // TODO: Initialize to an appropriate value
            RedBlackTree<T> target = new RedBlackTree<T>(strIdentifier);
            Assert.AreEqual(strIdentifier, target.ToString(), "Identifiers are not equal");
        }

        [TestMethod]
        public void RedBlackTreeConstructorTest()
        {
            RedBlackTreeConstructorTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for RedBlackTree`1 Constructor
        ///</summary>
        public void RedBlackTreeConstructorTest1Helper<T>()
            where T : class , IComparable
        {
            RedBlackTree<T> target = new RedBlackTree<T>();
            Assert.IsTrue(target.IsEmpty());
            Assert.AreEqual(0, target.Count);
        }

        [TestMethod]
        public void RedBlackTreeConstructorTest1()
        {
            RedBlackTreeConstructorTest1Helper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for Add
        ///</summary>
        public void AddTestHelper<T>(T x)
            where T : class , IComparable
        {
            RedBlackTree<T> target = new RedBlackTree<T>(); // TODO: Initialize to an appropriate value
            T data = x; // TODO: Initialize to an appropriate value
            target.Add(data);
            Assert.AreEqual(x, data, "Objects are not the same");
        }

        [TestMethod]
        public void AddTest()
        {
            AddTestHelper(new GenericParameterHelper(5));
        }

        /// <summary>
        ///A test for Clear
        ///</summary>
        ///<param name="testItem"> </param>
        public void ClearTestHelper<T>(T testItem)
            where T : class , IComparable
        {
            RedBlackTree<T> target = new RedBlackTree<T>(); // TODO: Initialize to an appropriate value
            Assert.IsTrue(target.IsEmpty());
            target.Add(testItem);
            Assert.AreEqual(1, target.Count);
            target.Clear();
            Assert.IsTrue(target.IsEmpty());
        }

        [TestMethod]
        public void ClearTest()
        {
            ClearTestHelper(new GenericParameterHelper(5));
        }

        /// <summary>
        ///A test for Contains
        ///</summary>
        public void ContainsTestHelper<T>(T testItem)
            where T : class , IComparable
        {
            RedBlackTree<T> target = new RedBlackTree<T>();
            T item = testItem;
            const bool expected = false;
            bool actual = target.Contains(item);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ContainsTest()
        {
            ContainsTestHelper(new GenericParameterHelper(5));
        }

        /// <summary>
        ///A test for CopyTo
        ///</summary>
        public void CopyToTestHelper<T>()
            where T : class , IComparable
        {
            RedBlackTree<T> target = new RedBlackTree<T>();
            T[] array = new T[1];
            const int arrayIndex = 0;
            target.CopyTo(array, arrayIndex);
        }

        [TestMethod]
        public void CopyToTest()
        {
            CopyToTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        public void EqualsTestHelper<T>()
            where T : class , IComparable
        {
            RedBlackTree<T> target = new RedBlackTree<T>();
            object obj = 5;
            const bool expected = false;
            bool actual = target.Equals(obj);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EqualsTest()
        {
            EqualsTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for GetData
        ///</summary>
        public void GetDataTestHelper<T>(T testItem)
            where T : class , IComparable
        {
            RedBlackTree<T> target = new RedBlackTree<T>();
            target.Add(testItem);
            IComparable key = testItem;
            T expected = testItem;
            T actual = target.GetData(key);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetDataTest()
        {
            GetDataTestHelper(new GenericParameterHelper(5));
        }

        /// <summary>
        ///A test for GetEnumerator
        ///</summary>
        public void GetEnumeratorTestHelper<T>(IEnumerator<T> enumerator)
            where T : class , IComparable
        {
            RedBlackTree<T> target = new RedBlackTree<T>();
            IEnumerator<T> expected = enumerator;
            target.Add(enumerator.Current);
            IEnumerator<T> actual = target.GetEnumerator();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod, Ignore]
        public void GetEnumeratorTest()
        {
            GetEnumeratorTestHelper((IEnumerator<GenericParameterHelper>)(new[] { new GenericParameterHelper(5) }).GetEnumerator());
        }

        /// <summary>
        ///A test for GetMaxKey
        ///</summary>
        public void GetMaxKeyTestHelper<T>(T[] items, T max)
            where T : class , IComparable
        {
            RedBlackTree<T> target = new RedBlackTree<T>();
            foreach (T item in items)
                target.Add(item);
            IComparable expected = max;
            IComparable actual = target.GetMaxKey();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetMaxKeyTest()
        {
            GetMaxKeyTestHelper(new[]
                                    {
                                        new GenericParameterHelper(5),
                                        new GenericParameterHelper(10),
                                        new GenericParameterHelper(6)
                                    }, new GenericParameterHelper(10));
        }

        /// <summary>
        ///A test for GetMaxValue
        ///</summary>
        public void GetMaxValueTestHelper<T>(T[] items, T max)
            where T : class , IComparable
        {
            RedBlackTree<T> target = new RedBlackTree<T>(); // TODO: Initialize to an appropriate value
            foreach (T item in items)
                target.Add(item);
            T expected = max;
            T actual = target.GetMaxValue();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetMaxValueTest()
        {
            GetMaxValueTestHelper(new[]
                                      {
                                          new GenericParameterHelper(5),
                                          new GenericParameterHelper(10),
                                          new GenericParameterHelper(6)
                                      }, new GenericParameterHelper(10));
        }

        /// <summary>
        ///A test for GetMinKey
        ///</summary>
        public void GetMinKeyTestHelper<T>(T[] items, T min)
            where T : class , IComparable
        {
            RedBlackTree<T> target = new RedBlackTree<T>(); // TODO: Initialize to an appropriate value
            foreach (T item in items)
                target.Add(item);
            IComparable expected = min; // TODO: Initialize to an appropriate value
            IComparable actual = target.GetMinKey();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetMinKeyTest()
        {
            GetMinKeyTestHelper(new[]
                                    {
                                        new GenericParameterHelper(5),
                                        new GenericParameterHelper(10),
                                        new GenericParameterHelper(6)
                                    }, new GenericParameterHelper(5));
        }

        /// <summary>
        ///A test for GetMinValue
        ///</summary>
        public void GetMinValueTestHelper<T>(T[] items, T min)
            where T : class , IComparable
        {
            RedBlackTree<T> target = new RedBlackTree<T>();
            foreach (T item in items)
                target.Add(item);
            T expected = min;
            T actual = target.GetMinValue();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetMinValueTest()
        {
            GetMinValueTestHelper(new[]
                                      {
                                          new GenericParameterHelper(5),
                                          new GenericParameterHelper(10),
                                          new GenericParameterHelper(6)
                                      }, new GenericParameterHelper(5));
        }

        /// <summary>
        ///A test for IsEmpty
        ///</summary>
        public void IsEmptyTestHelper<T>()
            where T : class , IComparable
        {
            RedBlackTree<T> target = new RedBlackTree<T>();
            const bool expected = true;
            bool actual = target.IsEmpty();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IsEmptyTest()
        {
            IsEmptyTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for Remove
        ///</summary>
        public void RemoveTestHelper<T>(T item)
            where T : class , IComparable
        {
            RedBlackTree<T> target = new RedBlackTree<T>();
            target.Add(item);
            const bool expected = true;
            bool actual = target.Remove(item);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RemoveTest()
        {
            RemoveTestHelper(new GenericParameterHelper(5));
        }

        /// <summary>
        ///A test for Remove
        ///</summary>
        public void RemoveTest1Helper<T>(T item)
            where T : class , IComparable
        {
            RedBlackTree<T> target = new RedBlackTree<T>();
            target.Add(item);
            IComparable key = item;
            target.Remove(key);
            Assert.IsTrue(target.IsEmpty());
        }

        [TestMethod]
        public void RemoveTest1()
        {
            RemoveTest1Helper(new GenericParameterHelper(5));
        }

        /// <summary>
        ///A test for RemoveMax
        ///</summary>
        public void RemoveMaxTestHelper<T>(T[] items, T min, T max)
            where T : class , IComparable
        {
            RedBlackTree<T> target = new RedBlackTree<T>();
            foreach (T item in items)
                target.Add(item);
            Assert.AreEqual(max, target.GetMaxKey());
            target.RemoveMax();
            Assert.AreEqual(min, target.GetMaxKey());
        }

        [TestMethod]
        public void RemoveMaxTest()
        {
            RemoveMaxTestHelper(new[]
                                    {
                                        new GenericParameterHelper(5),
                                        new GenericParameterHelper(6)
                                    }, new GenericParameterHelper(5),
                                    new GenericParameterHelper(6));
        }

        /// <summary>
        ///A test for RemoveMin
        ///</summary>
        public void RemoveMinTestHelper<T>(T[] items, T min, T max)
            where T : class , IComparable
        {
            RedBlackTree<T> target = new RedBlackTree<T>();
            foreach (T item in items)
                target.Add(item);
            Assert.AreEqual(min, target.GetMinKey());
            target.RemoveMin();
            Assert.AreEqual(max, target.GetMinKey());
        }

        [TestMethod]
        public void RemoveMinTest()
        {
            RemoveMinTestHelper(new[]
                                    {
                                        new GenericParameterHelper(5),
                                        new GenericParameterHelper(6)
                                    }, new GenericParameterHelper(5),
                                new GenericParameterHelper(6));
        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        public void ToStringTestHelper<T>()
            where T : class , IComparable
        {
            const string expected = "test";
            RedBlackTree<T> target = new RedBlackTree<T>(expected);
            string actual = target.ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToStringTest()
        {
            ToStringTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for Count
        ///</summary>
        public void CountTestHelper<T>(T x)
            where T : class , IComparable
        {
            RedBlackTree<T> target = new RedBlackTree<T>();
            Assert.AreEqual(0, target.Count);
            target.Add(x);
            const int expected = 1;
            int actual = target.Count;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DeploymentItem("System.Collections.Generic.RedBlack.dll")]
        public void CountTest()
        {
            CountTestHelper(new GenericParameterHelper(5));
        }

        /// <summary>
        ///A test for IsReadOnly
        ///</summary>
        public void IsReadOnlyTestHelper<T>()
            where T : class , IComparable
        {
            RedBlackTree<T> target = new RedBlackTree<T>();
            bool actual = target.IsReadOnly;
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void IsReadOnlyTest()
        {
            IsReadOnlyTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for Item
        ///</summary>
        public void ItemTestHelper<T>(T item)
            where T : class , IComparable
        {
            RedBlackTree<T> target = new RedBlackTree<T>();
            target.Add(item);
            IComparable key = item;
            T expected = item;
            target[key] = expected;
            T actual = target[key];
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ItemTest()
        {
            ItemTestHelper(new GenericParameterHelper(5));
        }
    }
}
