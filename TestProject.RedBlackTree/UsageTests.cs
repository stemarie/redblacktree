using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic.RedBlack;

namespace TestProject.RedBlackTree
{
    [TestClass]
    public class UsageTests
    {
        [TestMethod]
        public void TestUsage1()
        {
            RedBlackTree<Guid, string> tree = new RedBlackTree<Guid, string>();
            Guid item1 = Guid.NewGuid();
            Guid item2 = Guid.NewGuid();
            for (int i = 0; i < 100; i++)
            {
                tree.Add(Guid.NewGuid(), DateTime.UtcNow.ToString(CultureInfo.InvariantCulture));
            }
            tree.Add(item1, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture));
            tree.Add(item2, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture));

            Assert.AreEqual(102, tree.Count, "Should be 102 items in the tree");
            Assert.IsTrue(tree.ContainsKey(item1), "Should contain item 1");
            Assert.IsTrue(tree.ContainsKey(item2), "Should contain item 2");
            tree.Remove(item1);
            Assert.AreEqual(101, tree.Count, "Should be 101 items in the tree");
            Assert.IsFalse(tree.ContainsKey(item1), "Should not contain item 1");
            tree.Remove(item2);
            Assert.AreEqual(100, tree.Count, "Should be 100 items in the tree");
            Assert.IsFalse(tree.ContainsKey(item2), "Should not contain item 2");
        }
    }
}
