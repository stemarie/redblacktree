using System.Collections.Generic.RedBlack;
using System.Linq;
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
    public class RedBlackTreeTests
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
        public void RedBlackTreeConstructorTest1Helper<K, T>()
            where T : class
            where K : IComparable
        {
            RedBlackTree<K, T> target = new RedBlackTree<K, T>();
            Assert.IsTrue(target.IsEmpty());
            Assert.AreEqual(0, target.Count);
        }

        [TestMethod]
        public void RedBlackTreeConstructorTest1()
        {
            RedBlackTreeConstructorTest1Helper<int, GenericParameterHelper>();
        }

        /// <summary>
        ///A test for Add
        ///</summary>
        public void AddTestHelper<K, T>(K key, T x)
            where T : class
            where K : IComparable
        {
            RedBlackTree<K, T> target = new RedBlackTree<K, T>();
            T data = x;
            target.Add(key, data);
            Assert.AreEqual(x, data, "Objects are not the same");
        }

        [TestMethod]
        public void AddTest()
        {
            AddTestHelper(5, new GenericParameterHelper(5));
        }

        /// <summary>
        ///A test for Clear
        ///</summary>
        ///<param name="key"> </param>
        ///<param name="testItem"> </param>
        public void ClearTestHelper<K, T>(K key, T testItem)
            where T : class
            where K : IComparable
        {
            RedBlackTree<K, T> target = new RedBlackTree<K, T>();
            Assert.IsTrue(target.IsEmpty());
            target.Add(key, testItem);
            Assert.AreEqual(1, target.Count);
            target.Clear();
            Assert.IsTrue(target.IsEmpty());
        }

        [TestMethod]
        public void ClearTest()
        {
            ClearTestHelper(5, new GenericParameterHelper(5));
        }

        /// <summary>
        ///A test for Contains
        ///</summary>
        public void ContainsTestHelper<K, T>(K key, T testItem)
            where T : class
            where K : IComparable
        {
            RedBlackTree<K, T> target = new RedBlackTree<K, T>();
            T item = testItem;
            const bool expected = false;
            bool actual = target.Contains(item);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ContainsTest()
        {
            ContainsTestHelper(5, new GenericParameterHelper(5));
        }

        /// <summary>
        ///A test for CopyTo
        ///</summary>
        public void CopyToTestHelper<K, T>()
            where T : class
            where K : IComparable
        {
            RedBlackTree<K, T> target = new RedBlackTree<K, T>();
            KeyValuePair<K, T>[] array = new KeyValuePair<K, T>[1];
            const int arrayIndex = 0;
            target.CopyTo(array, arrayIndex);
        }

        [TestMethod]
        public void CopyToTest()
        {
            CopyToTestHelper<string, GenericParameterHelper>();
        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        public void EqualsTestHelper<K, T>()
            where T : class
            where K : IComparable
        {
            RedBlackTree<K, T> target = new RedBlackTree<K, T>();
            object obj = 5;
            const bool expected = false;
            bool actual = target.Equals(obj);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EqualsTest()
        {
            EqualsTestHelper<string, GenericParameterHelper>();
        }

        /// <summary>
        ///A test for GetData
        ///</summary>
        public void GetDataTestHelper<K, T>(K key, T testItem)
            where T : class
            where K : IComparable
        {
            RedBlackTree<K, T> target = new RedBlackTree<K, T>();
            target.Add(key, testItem);
            T expected = testItem;
            T actual = target.GetData(key);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetDataTest()
        {
            GetDataTestHelper(5, new GenericParameterHelper(5));
        }

        /// <summary>
        ///A test for GetEnumerator
        ///</summary>
        public void GetEnumeratorTestHelper<K, T>(KeyValuePair<K, T>[] items)
            where T : class
            where K : IComparable
        {
            RedBlackTree<K, T> target = new RedBlackTree<K, T>();
            foreach (KeyValuePair<K, T> item in items)
            {
                target.Add(item);
            }
            List<T> list = new List<T>(target.Select(i => i.Value));
            Assert.AreEqual(3, list.Count);
        }

        [TestMethod]
        public void GetEnumeratorTest()
        {
            GetEnumeratorTestHelper(
                new[]
                    {
                        new KeyValuePair<int, GenericParameterHelper>
                            (5, new GenericParameterHelper(5)),
                        new KeyValuePair<int, GenericParameterHelper>
                            (6, new GenericParameterHelper(6)),
                        new KeyValuePair<int, GenericParameterHelper>
                            (7, new GenericParameterHelper(7))
                    });
        }

        /// <summary>
        ///A test for GetMaxKey
        ///</summary>
        public void GetMaxKeyTestHelper<K, T>(KeyValuePair<K, T>[] items, K max)
            where T : class
            where K : IComparable
        {
            RedBlackTree<K, T> target = new RedBlackTree<K, T>();
            foreach (KeyValuePair<K, T> item in items)
                target.Add(item);
            K expected = max;
            IComparable actual = target.GetMaxKey();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetMaxKeyTest()
        {
            GetMaxKeyTestHelper(
                new[]
                    {
                        new KeyValuePair<int, GenericParameterHelper>(
                            5, new GenericParameterHelper(5)),
                        new KeyValuePair<int, GenericParameterHelper>(
                            10, new GenericParameterHelper(10)),
                        new KeyValuePair<int, GenericParameterHelper>(
                            6, new GenericParameterHelper(6))
                    }, 10);
        }

        /// <summary>
        ///A test for GetMaxValue
        ///</summary>
        public void GetMaxValueTestHelper<K, T>(KeyValuePair<K, T>[] items, T max)
            where T : class
            where K : IComparable
        {
            RedBlackTree<K, T> target = new RedBlackTree<K, T>();
            foreach (KeyValuePair<K, T> item in items)
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
                                          new KeyValuePair<int, GenericParameterHelper>(
                                              5, new GenericParameterHelper(5)),
                                          new KeyValuePair<int, GenericParameterHelper>(
                                              10, new GenericParameterHelper(10)),
                                          new KeyValuePair<int, GenericParameterHelper>(
                                              6, new GenericParameterHelper(6))
                                      }, new GenericParameterHelper(10));
        }

        /// <summary>
        ///A test for GetMinKey
        ///</summary>
        public void GetMinKeyTestHelper<K, T>(KeyValuePair<K, T>[] items, K min)
            where T : class
            where K : IComparable
        {
            RedBlackTree<K, T> target = new RedBlackTree<K, T>();
            foreach (KeyValuePair<K, T> item in items)
                target.Add(item);
            IComparable expected = min;
            IComparable actual = target.GetMinKey();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetMinKeyTest()
        {
            GetMinKeyTestHelper(new[]
                                    {
                                        new KeyValuePair<int, GenericParameterHelper>(
                                            5, new GenericParameterHelper(5)),
                                        new KeyValuePair<int, GenericParameterHelper>(
                                            10, new GenericParameterHelper(10)),
                                        new KeyValuePair<int, GenericParameterHelper>(
                                            6, new GenericParameterHelper(6))
                                    }, 5);
        }

        /// <summary>
        ///A test for GetMinValue
        ///</summary>
        public void GetMinValueTestHelper<K, T>(KeyValuePair<K, T>[] items, T min)
            where T : class
            where K : IComparable
        {
            RedBlackTree<K, T> target = new RedBlackTree<K, T>();
            foreach (KeyValuePair<K, T> item in items)
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
                                          new KeyValuePair<int, GenericParameterHelper>(
                                              5, new GenericParameterHelper(5)),
                                          new KeyValuePair<int, GenericParameterHelper>(
                                              10, new GenericParameterHelper(10)),
                                          new KeyValuePair<int, GenericParameterHelper>(
                                              6, new GenericParameterHelper(6))
                                      }, new GenericParameterHelper(5));
        }

        /// <summary>
        ///A test for IsEmpty
        ///</summary>
        public void IsEmptyTestHelper<K, T>()
            where T : class
            where K : IComparable
        {
            RedBlackTree<K, T> target = new RedBlackTree<K, T>();
            const bool expected = true;
            bool actual = target.IsEmpty();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IsEmptyTest()
        {
            IsEmptyTestHelper<string, GenericParameterHelper>();
        }

        /// <summary>
        ///A test for Remove
        ///</summary>
        public void RemoveTestHelper<K, T>(KeyValuePair<K, T> item)
            where T : class
            where K : IComparable
        {
            RedBlackTree<K, T> target = new RedBlackTree<K, T>();
            target.Add(item);
            const bool expected = true;
            bool actual = target.Remove(item);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RemoveTest()
        {
            RemoveTestHelper(
                new KeyValuePair<int, GenericParameterHelper>(5, new GenericParameterHelper(5)));
        }

        /// <summary>
        ///A test for Remove
        ///</summary>
        public void RemoveTest1Helper<K, T>(KeyValuePair<K, T> item)
            where T : class
            where K : IComparable
        {
            RedBlackTree<K, T> target = new RedBlackTree<K, T>();
            target.Add(item);
            K key = item.Key;
            target.Remove(key);
            Assert.IsTrue(target.IsEmpty());
        }

        [TestMethod]
        public void RemoveTest1()
        {
            RemoveTest1Helper(
                new KeyValuePair<int, GenericParameterHelper>(5, new GenericParameterHelper(5)));
        }

        /// <summary>
        ///A test for RemoveMax
        ///</summary>
        public void RemoveMaxTestHelper<K, T>(KeyValuePair<K, T>[] items, K min, K max)
            where T : class
            where K : IComparable
        {
            RedBlackTree<K, T> target = new RedBlackTree<K, T>();
            foreach (KeyValuePair<K, T> item in items)
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
                                        new KeyValuePair<int, GenericParameterHelper>(
                                            5, new GenericParameterHelper(5)),
                                        new KeyValuePair<int, GenericParameterHelper>(
                                            6, new GenericParameterHelper(6))
                                    }, 5, 6);
        }

        /// <summary>
        ///A test for RemoveMin
        ///</summary>
        public void RemoveMinTestHelper<K, T>(KeyValuePair<K, T>[] items, K min, K max)
            where T : class
            where K : IComparable
        {
            RedBlackTree<K, T> target = new RedBlackTree<K, T>();
            foreach (KeyValuePair<K, T> item in items)
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
                                        new KeyValuePair<int, GenericParameterHelper>(
                                            5, new GenericParameterHelper(5)),
                                        new KeyValuePair<int, GenericParameterHelper>(
                                            6, new GenericParameterHelper(6))
                                    }, 5, 6);
        }

        /// <summary>
        ///A test for Count
        ///</summary>
        public void CountTestHelper<K, T>(KeyValuePair<K, T> x)
            where T : class
            where K : IComparable
        {
            RedBlackTree<K, T> target = new RedBlackTree<K, T>();
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
            CountTestHelper(
                new KeyValuePair<int, GenericParameterHelper>(5, new GenericParameterHelper(5)));
        }

        /// <summary>
        ///A test for IsReadOnly
        ///</summary>
        public void IsReadOnlyTestHelper<K, T>()
            where T : class
            where K : IComparable
        {
            RedBlackTree<K, T> target = new RedBlackTree<K, T>();
            bool actual = target.IsReadOnly;
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void IsReadOnlyTest()
        {
            IsReadOnlyTestHelper<string, GenericParameterHelper>();
        }

        /// <summary>
        ///A test for Item
        ///</summary>
        public void ItemTestHelper<K, T>(KeyValuePair<K, T> item)
            where T : class
            where K : IComparable
        {
            RedBlackTree<K, T> target = new RedBlackTree<K, T>();
            target.Add(item);
            K key = item.Key;
            T expected = item.Value;
            target[key] = expected;
            T actual = target[key];
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ItemTest()
        {
            ItemTestHelper(new KeyValuePair<int, GenericParameterHelper>(5, new GenericParameterHelper(5)));
        }
    }
}
