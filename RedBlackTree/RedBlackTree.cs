using System.Linq;
using System.Threading;

namespace System.Collections.Generic.RedBlack
{
    /// <summary>
    /// Red Black Tree Implementation
    /// </summary>
    /// <typeparam name="TKey">Key type</typeparam>
    /// <typeparam name="TValue">Data type</typeparam>
    /// <example>
    /// var x = new RedBlackTree&lt;Guid,String&gt;();
    /// </example>
    public class RedBlackTree<TKey, TValue> : IDictionary<TKey, TValue>
        where TValue : class
        where TKey : IComparable
    {
        internal enum RedBlackNodeType
        {
            Red = 0,
            Black = 1
        }

        // a simple randomized hash code. The hash code could be used as a key
        // if it is "unique" enough.
        // The IComparable interface would need to be replaced with int.
        private int _intHashCode;

        // identifies the owner of the tree
        private string _strIdentifier;

        // the tree
        private RedBlackNode<TKey, TValue> _treeBaseNode = SentinelNode;

        // the node that was last found; used to optimize searches
        private RedBlackNode<TKey, TValue> _lastNodeFound = SentinelNode;

        private readonly Random _rand = new Random();

        // sentinelNode is convenient way of indicating a leaf node.
        // set up the sentinel node. the sentinel node is the key to a successfull
        // implementation and for understanding the red-black tree properties.
        internal static readonly RedBlackNode<TKey, TValue> SentinelNode =
            new RedBlackNode<TKey, TValue>
            {
                Left = null,
                Right = null,
                Parent = null,
                Color = RedBlackNodeType.Black
            };

        private int _count;

        /// <summary>
        /// Constructor that initializes a blank Red Black Tree
        /// </summary>
        /// <example>
        /// var x = new RedBlackTree&lt;Guid,String&gt;();
        /// </example>
        public RedBlackTree()
        {
            Initialize(base.ToString() + _rand.Next());
        }

        /// <summary>
        /// Get a value stored in the tree using a key
        /// </summary>
        /// <param name="key">key of the item to be returned</param>
        /// <returns>value stored in the tree, null if it does not exist</returns>
        /// <example>
        /// Guid id = Guid.NewGuid();
        /// var x = new RedBlackTree&lt;Guid,String&gt;();
        /// var y = new KeyValuePair&lt;Guid,String&gt; { id, "Hello" };
        /// x.Add(y);
        /// var y = x[id];
        /// </example>
        public TValue this[TKey key]
        {
            get { return GetNode(key).Data; }
            set { GetNode(key).Data = value; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        /// <example>
        /// Guid id = Guid.NewGuid();
        /// var x = new RedBlackTree&lt;Guid,String&gt;();
        /// var y = new KeyValuePair&lt;Guid,String&gt; { id, "Hello" };
        /// x.Add(y);
        /// x.Remove(y);
        /// </example>
        /// <remarks>The Red Black Tree implementation actually ignores the Value portion in the case of the delete, it removes the node with the matching Key</remarks>
        public virtual bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }

        /// <summary>
        /// Returns the number of items currently in the tree
        /// </summary>
        public int Count { get { return _count; } }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.
        /// </returns>
        public bool IsReadOnly
        {
            get { return false; }
        }

        ///<summary>
        /// GetData
        /// Gets the data object associated with the specified key
        ///</summary>
        /// <remarks>This is the equivalent of calling T this[K key]</remarks>
        public TValue GetData(TKey key)
        {
            return GetNode(key).Data;
        }

        ///<summary>
        /// GetMinKey
        /// Returns the minimum key value
        ///</summary>
        public TKey GetMinKey()
        {
            RedBlackNode<TKey, TValue> workNode = _treeBaseNode;

            if (workNode == null || workNode == SentinelNode)
                throw (new RedBlackException(Properties.Resources.ExceptionTreeIsEmpty));

            // traverse to the extreme left to find the smallest key
            while (workNode.Left != SentinelNode)
                workNode = workNode.Left;

            _lastNodeFound = workNode;

            return workNode.Key;
        }

        ///<summary>
        /// GetMaxKey
        /// Returns the maximum key value
        ///</summary>
        public TKey GetMaxKey()
        {
            RedBlackNode<TKey, TValue> workNode = _treeBaseNode;

            if (workNode == null || workNode == SentinelNode)
                throw (new RedBlackException(Properties.Resources.ExceptionTreeIsEmpty));

            // traverse to the extreme right to find the largest key
            while (workNode.Right != SentinelNode)
                workNode = workNode.Right;

            _lastNodeFound = workNode;

            return workNode.Key;
        }

        ///<summary>
        /// GetMinValue
        /// Returns the object having the minimum key value
        ///</summary>
        public TValue GetMinValue()
        {
            return GetData(GetMinKey());
        }

        ///<summary>
        /// GetMaxValue
        /// Returns the object having the maximum key
        ///</summary>
        public TValue GetMaxValue()
        {
            return GetData(GetMaxKey());
        }

        ///<summary>
        /// GetEnumerator
        /// return an enumerator that returns the tree nodes in order
        ///</summary>
        public IEnumerator<TValue> GetEnumerator()
        {
            return GetAll().Select(i => i.Data).GetEnumerator();
        }

        ///<summary>
        /// IsEmpty
        /// Is the tree empty?
        ///</summary>
        public bool IsEmpty()
        {
            return (_treeBaseNode == SentinelNode);
        }

        ///<summary>
        /// RemoveMin
        /// removes the node with the minimum key
        ///</summary>
        public void RemoveMin()
        {
            if (_treeBaseNode == null)
                throw (new RedBlackException(Properties.Resources.ExceptionNodeIsNull));

            Delete(GetNode(GetMinKey()));
        }

        ///<summary>
        /// RemoveMax
        /// removes the node with the maximum key
        ///</summary>
        public void RemoveMax()
        {
            if (_treeBaseNode == null)
                throw (new RedBlackException(Properties.Resources.ExceptionNodeIsNull));

            Delete(GetNode(GetMaxKey()));
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        public virtual void Add(KeyValuePair<TKey, TValue> item)
        {
            New(item.Key, item.Value);
        }

        ///<summary>
        /// Clear
        /// Empties or clears the tree
        ///</summary>
        public void Clear()
        {
            _treeBaseNode = SentinelNode;
            _count = 0;
            InvokeOnClear(new EventArgs());
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
        /// </returns>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return ContainsKey(item.Key);
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param><param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception><exception cref="T:System.ArgumentException"><paramref name="array"/> is multidimensional.-or-The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.-or-Type <paramref name="T"/> cannot be cast automatically to the type of the destination <paramref name="array"/>.</exception>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException();
            if (array == null)
                throw new ArgumentNullException();
            if ((array.Length - arrayIndex) < Count)
                throw new ArgumentException();
            int currentPosition = arrayIndex;
            foreach (TValue item in GetAll()
                .Select(i => i.Data))
            {
                array.SetValue(item, currentPosition);
                currentPosition++;
            }
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
        /// </returns>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        public bool Contains(TValue item)
        {
            return GetAll().Select(i => i.Data).Any(i => i == item);
        }

        ///<summary>
        /// Equals
        ///</summary>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (!(obj is RedBlackNode<TKey, TValue>))
                return false;

            return this == obj || (ToString().Equals(obj.ToString()));
        }

        ///<summary>
        /// HashCode
        ///</summary>
        public override int GetHashCode()
        {
            return _intHashCode;
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetAll().GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return GetAll()
                .Select(i => new KeyValuePair<TKey, TValue>(i.Key, i.Data))
                .GetEnumerator();
        }

        ///<summary>
        /// ToString
        ///</summary>
        public override string ToString()
        {
            return _strIdentifier;
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.IDictionary`2"/> contains an element with the specified key.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Collections.Generic.IDictionary`2"/> contains an element with the key; otherwise, false.
        /// </returns>
        /// <param name="key">The key to locate in the <see cref="T:System.Collections.Generic.IDictionary`2"/>.</param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception>
        public bool ContainsKey(TKey key)
        {
            try
            {
                var node = GetNode(key);
                return node != null;
            }
            catch (RedBlackException)
            {
                return false;
            }
        }

        /// <summary>
        /// Adds an element with the provided key and value to the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </summary>
        /// <param name="key">The object to use as the key of the element to add.</param><param name="value">The object to use as the value of the element to add.</param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception><exception cref="T:System.ArgumentException">An element with the same key already exists in the <see cref="T:System.Collections.Generic.IDictionary`2"/>.</exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IDictionary`2"/> is read-only.</exception>
        public virtual void Add(TKey key, TValue value)
        {
            New(key, value);
        }

        /// <summary>
        /// Removes the element with the specified key from the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </summary>
        /// <returns>
        /// true if the element is successfully removed; otherwise, false.  This method also returns false if <paramref name="key"/> was not found in the original <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </returns>
        /// <param name="key">The key of the element to remove.</param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IDictionary`2"/> is read-only.</exception>
        public virtual bool Remove(TKey key)
        {
            try
            {
                Delete(GetNode(key));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <returns>
        /// true if the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"/> contains an element with the specified key; otherwise, false.
        /// </returns>
        /// <param name="key">The key whose value to get.</param><param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value"/> parameter. This parameter is passed uninitialized.</param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception>
        public bool TryGetValue(TKey key, out TValue value)
        {
            value = GetNode(key).Data;
            return (value != null);
        }

        /// <summary>
        /// Gets or sets the element with the specified key.
        /// </summary>
        /// <returns>
        /// The element with the specified key.
        /// </returns>
        /// <param name="key">The key of the element to get or set.</param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception><exception cref="T:System.Collections.Generic.KeyNotFoundException">The property is retrieved and <paramref name="key"/> is not found.</exception><exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.Generic.IDictionary`2"/> is read-only.</exception>
        TValue IDictionary<TKey, TValue>.this[TKey key]
        {
            get { return GetNode(key).Data; }
            set { GetNode(key).Data = value; }
        }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1"/> containing the keys of the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.Generic.ICollection`1"/> containing the keys of the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </returns>
        public ICollection<TKey> Keys
        {
            get { return GetAll().Select(i => i.Key).ToArray(); }
        }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1"/> containing the values in the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.Generic.ICollection`1"/> containing the values in the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </returns>
        public ICollection<TValue> Values
        {
            get { return GetAll().Select(i => i.Data).ToArray(); }
        }
        /// <summary>
        /// Invoked when Item is added
        /// </summary>
        public event EventHandler OnAdd;

        protected void InvokeOnAdd(RedBlackEventArgs<TKey, TValue> e)
        {
            EventHandler handler = OnAdd;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        /// Invoked when Item is removed
        /// </summary>
        public event EventHandler OnRemove;

        protected void InvokeOnRemove(RedBlackEventArgs<TKey, TValue> e)
        {
            EventHandler handler = OnRemove;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        /// Invoked when tree is cleared
        /// </summary>
        public event EventHandler OnClear;

        protected void InvokeOnClear(EventArgs e)
        {
            EventHandler handler = OnClear;
            if (handler != null) handler(this, e);
        }

        #region "Private Methods"

        private void New(TKey key, TValue data)
        {
            if (data == null)
                throw (new RedBlackException(Properties.Resources.ExceptionNodeKeyAndDataMustNotBeNull));

            // traverse tree - find where node belongs

            // create new node
            RedBlackNode<TKey, TValue> newNode = new RedBlackNode<TKey, TValue>(key, data);

            // grab the rbTree node of the tree
            RedBlackNode<TKey, TValue> workNode = _treeBaseNode;

            while (workNode != SentinelNode)
            {
                // find Parent
                newNode.Parent = workNode;
                int result = key.CompareTo(workNode.Key);
                if (result == 0)
                    throw (new RedBlackException(Properties.Resources.ExceptionNodeWithSameKeyAlreadyExists));
                workNode = result > 0
                    ? workNode.Right
                    : workNode.Left;
            }

            // insert node into tree starting at parent's location
            if (newNode.Parent != null)
            {
                if (newNode.Key.CompareTo(newNode.Parent.Key) > 0)
                    newNode.Parent.Right = newNode;
                else
                    newNode.Parent.Left = newNode;
            }
            else
                // first node added
                _treeBaseNode = newNode;

            // restore red-black properties
            BalanceTreeAfterInsert(newNode);

            _lastNodeFound = newNode;

            Interlocked.Increment(ref _count);
            InvokeOnAdd(new RedBlackEventArgs<TKey, TValue> { Item = data, Key = key });
        }

        ///<summary>
        /// Delete
        /// Delete a node from the tree and restore red black properties
        ///</summary>
        private void Delete(RedBlackNode<TKey, TValue> deleteNode)
        {
            // A node to be deleted will be: 
            //		1. a leaf with no children
            //		2. have one child
            //		3. have two children
            // If the deleted node is red, the red black properties still hold.
            // If the deleted node is black, the tree needs rebalancing

            // work node
            RedBlackNode<TKey, TValue> workNode;

            // find the replacement node (the successor to x) - the node one with 
            // at *most* one child. 
            if (deleteNode.Left == SentinelNode || deleteNode.Right == SentinelNode)
                // node has sentinel as a child
                workNode = deleteNode;
            else
            {
                // z has two children, find replacement node which will 
                // be the leftmost node greater than z
                // traverse right subtree
                workNode = deleteNode.Right;
                // to find next node in sequence
                while (workNode.Left != SentinelNode)
                    workNode = workNode.Left;
            }

            // at this point, y contains the replacement node. it's content will be copied 
            // to the valules in the node to be deleted

            // x (y's only child) is the node that will be linked to y's old parent. 
            RedBlackNode<TKey, TValue> linkedNode = workNode.Left != SentinelNode
                                                 ? workNode.Left
                                                 : workNode.Right;

            // replace x's parent with y's parent and
            // link x to proper subtree in parent
            // this removes y from the chain
            linkedNode.Parent = workNode.Parent;
            if (workNode.Parent != null)
                if (workNode == workNode.Parent.Left)
                    workNode.Parent.Left = linkedNode;
                else
                    workNode.Parent.Right = linkedNode;
            else
                // make x the root node
                _treeBaseNode = linkedNode;

            // copy the values from y (the replacement node) to the node being deleted.
            // note: this effectively deletes the node. 
            if (workNode != deleteNode)
            {
                deleteNode.Key = workNode.Key;
                deleteNode.Data = workNode.Data;
            }

            if (workNode.Color == RedBlackNodeType.Black)
                BalanceTreeAfterDelete(linkedNode);

            _lastNodeFound = SentinelNode;

            Interlocked.Decrement(ref _count);
            InvokeOnRemove(new RedBlackEventArgs<TKey, TValue> { Item = deleteNode.Data, Key = deleteNode.Key });
        }

        ///<summary>
        /// BalanceTreeAfterDelete
        /// Deletions from red-black trees may destroy the red-black 
        /// properties. Examine the tree and restore. Rotations are normally 
        /// required to restore it
        ///</summary>
        private void BalanceTreeAfterDelete(RedBlackNode<TKey, TValue> linkedNode)
        {
            // maintain Red-Black tree balance after deleting node
            while (linkedNode != _treeBaseNode && linkedNode.Color == RedBlackNodeType.Black)
            {
                RedBlackNode<TKey, TValue> workNode;
                // determine sub tree from parent
                if (linkedNode == linkedNode.Parent.Left)
                {
                    // y is x's sibling
                    workNode = linkedNode.Parent.Right;
                    if (workNode.Color == RedBlackNodeType.Red)
                    {	
                        // x is black, y is red - make both black and rotate
                        linkedNode.Parent.Color = RedBlackNodeType.Red;
                        workNode.Color = RedBlackNodeType.Black;
                        RotateLeft(linkedNode.Parent);
                        workNode = linkedNode.Parent.Right;
                    }
                    if (workNode.Left.Color == RedBlackNodeType.Black &&
                        workNode.Right.Color == RedBlackNodeType.Black)
                    {
                        // children are both black
                        // change parent to red
                        workNode.Color = RedBlackNodeType.Red;
                        // move up the tree
                        linkedNode = linkedNode.Parent;
                    }
                    else
                    {
                        if (workNode.Right.Color == RedBlackNodeType.Black)
                        {
                            workNode.Left.Color = RedBlackNodeType.Black;
                            workNode.Color = RedBlackNodeType.Red;
                            RotateRight(workNode);
                            workNode = linkedNode.Parent.Right;
                        }
                        linkedNode.Parent.Color = RedBlackNodeType.Black;
                        workNode.Color = linkedNode.Parent.Color;
                        workNode.Right.Color = RedBlackNodeType.Black;
                        RotateLeft(linkedNode.Parent);
                        linkedNode = _treeBaseNode;
                    }
                }
                else
                {	// right subtree - same as code above with right and left swapped
                    workNode = linkedNode.Parent.Left;
                    if (workNode.Color == RedBlackNodeType.Red)
                    {
                        linkedNode.Parent.Color = RedBlackNodeType.Red;
                        workNode.Color = RedBlackNodeType.Black;
                        RotateRight(linkedNode.Parent);
                        workNode = linkedNode.Parent.Left;
                    }
                    if (workNode.Right.Color == RedBlackNodeType.Black &&
                        workNode.Left.Color == RedBlackNodeType.Black)
                    {
                        workNode.Color = RedBlackNodeType.Red;
                        linkedNode = linkedNode.Parent;
                    }
                    else
                    {
                        if (workNode.Left.Color == RedBlackNodeType.Black)
                        {
                            workNode.Right.Color = RedBlackNodeType.Black;
                            workNode.Color = RedBlackNodeType.Red;
                            RotateLeft(workNode);
                            workNode = linkedNode.Parent.Left;
                        }
                        workNode.Color = linkedNode.Parent.Color;
                        linkedNode.Parent.Color = RedBlackNodeType.Black;
                        workNode.Left.Color = RedBlackNodeType.Black;
                        RotateRight(linkedNode.Parent);
                        linkedNode = _treeBaseNode;
                    }
                }
            }
            linkedNode.Color = RedBlackNodeType.Black;
        }

        internal Stack<RedBlackNode<TKey, TValue>> GetAll()
        {
            Stack<RedBlackNode<TKey, TValue>> stack = new Stack<RedBlackNode<TKey, TValue>>();

            // use depth-first traversal to push nodes into stack
            // the lowest node will be at the top of the stack
            if (_treeBaseNode != SentinelNode)
            {
                WalkNextLevel(_treeBaseNode, stack);
            }
            return stack;
        }

        private static void WalkNextLevel(RedBlackNode<TKey, TValue> node, Stack<RedBlackNode<TKey, TValue>> stack)
        {
            if (node.Right != SentinelNode)
                WalkNextLevel(node.Right, stack);
            stack.Push(node);
            if (node.Left != SentinelNode)
                WalkNextLevel(node.Left, stack);
        }

        /// <summary>
        /// Returns a node from the tree using the supplied key
        /// </summary>
        /// <param name="key"></param>
        /// <returns>The Node or null if the key does not exist</returns>
        private RedBlackNode<TKey, TValue> GetNode(TKey key)
        {
            int result;
            if (_lastNodeFound != SentinelNode)
            {
                result = key.CompareTo(_lastNodeFound.Key);
                if (result == 0)
                    return _lastNodeFound;
            }

            // begin at root
            RedBlackNode<TKey, TValue> treeNode = _treeBaseNode;

            // traverse tree until node is found
            while (treeNode != SentinelNode)
            {
                result = key.CompareTo(treeNode.Key);
                if (result == 0)
                {
                    _lastNodeFound = treeNode;
                    return treeNode;
                }
                treeNode = result < 0 
                    ? treeNode.Left 
                    : treeNode.Right;
            }
            return null;
        }

        ///<summary>
        /// Rotate Right
        /// Rebalance the tree by rotating the nodes to the right
        ///</summary>
        private void RotateRight(RedBlackNode<TKey, TValue> rotateNode)
        {
            // pushing node rotateNode down and to the Right to balance the tree. rotateNode's Left child (_workNode)
            // replaces rotateNode (since rotateNode < _workNode), and _workNode's Right child becomes rotateNode's Left child 
            // (since it's < rotateNode but > _workNode).

            // get rotateNode's Left node, this becomes _workNode
            RedBlackNode<TKey, TValue> workNode = rotateNode.Left;

            // set rotateNode's Right link
            // _workNode's Right child becomes rotateNode's Left child
            rotateNode.Left = workNode.Right;

            // modify parents
            if (workNode.Right != SentinelNode)
                // sets _workNode's Right Parent to rotateNode
                workNode.Right.Parent = rotateNode;

            if (workNode != SentinelNode)
                // set _workNode's Parent to rotateNode's Parent
                workNode.Parent = rotateNode.Parent;

            // null=rbTree, could also have used rbTree
            if (rotateNode.Parent != null)
            {	// determine which side of it's Parent rotateNode was on
                if (rotateNode == rotateNode.Parent.Right)
                    // set Right Parent to _workNode
                    rotateNode.Parent.Right = workNode;
                else
                    // set Left Parent to _workNode
                    rotateNode.Parent.Left = workNode;
            }
            else
                // at rbTree, set it to _workNode
                _treeBaseNode = workNode;

            // link rotateNode and _workNode 
            // put rotateNode on _workNode's Right
            workNode.Right = rotateNode;
            // set _workNode as rotateNode's Parent
            if (rotateNode != SentinelNode)
                rotateNode.Parent = workNode;
        }

        ///<summary>
        /// Rotate Left
        /// Rebalance the tree by rotating the nodes to the left
        ///</summary>
        private void RotateLeft(RedBlackNode<TKey, TValue> rotateNode)
        {
            // pushing node rotateNode down and to the Left to balance the tree. rotateNode's Right child (_workNode)
            // replaces rotateNode (since _workNode > rotateNode), and _workNode's Left child becomes rotateNode's Right child 
            // (since it's < _workNode but > rotateNode).

            // get rotateNode's Right node, this becomes _workNode
            RedBlackNode<TKey, TValue> workNode = rotateNode.Right;

            // set rotateNode's Right link
            // _workNode's Left child's becomes rotateNode's Right child
            rotateNode.Right = workNode.Left;

            // modify parents
            if (workNode.Left != SentinelNode)
                // sets _workNode's Left Parent to rotateNode
                workNode.Left.Parent = rotateNode;

            if (workNode != SentinelNode)
                // set _workNode's Parent to rotateNode's Parent
                workNode.Parent = rotateNode.Parent;

            if (rotateNode.Parent != null)
            {	// determine which side of it's Parent rotateNode was on
                if (rotateNode == rotateNode.Parent.Left)
                    // set Left Parent to _workNode
                    rotateNode.Parent.Left = workNode;
                else
                    // set Right Parent to _workNode
                    rotateNode.Parent.Right = workNode;
            }
            else
                // at rbTree, set it to _workNode
                _treeBaseNode = workNode;

            // link rotateNode and _workNode
            // put rotateNode on _workNode's Left
            workNode.Left = rotateNode;
            // set _workNode as rotateNode's Parent
            if (rotateNode != SentinelNode)
                rotateNode.Parent = workNode;
        }

        private void Initialize(string identifier)
        {
            _strIdentifier = identifier;
            _intHashCode = _rand.Next();
            _count = 0;
        }

        ///<summary>
        /// Balance Tree After Insert
        /// Additions to red-black trees usually destroy the red-black 
        /// properties. Examine the tree and restore. Rotations are normally 
        /// required to restore it
        ///</summary>
        private void BalanceTreeAfterInsert(RedBlackNode<TKey, TValue> insertedNode)
        {
            // x and y are used as variable names for brevity, in a more formal
            // implementation, you should probably change the names

            // maintain red-black tree properties after adding newNode
            while (insertedNode != _treeBaseNode && insertedNode.Parent.Color == RedBlackNodeType.Red)
            {
                // Parent node is .Colored red; 
                RedBlackNode<TKey, TValue> workNode;
                if (insertedNode.Parent == insertedNode.Parent.Parent.Left)	// determine traversal path			
                {										// is it on the Left or Right subtree?
                    workNode = insertedNode.Parent.Parent.Right;			// get uncle
                    if (workNode != null && workNode.Color == RedBlackNodeType.Red)
                    {	// uncle is red; change x's Parent and uncle to black
                        insertedNode.Parent.Color = RedBlackNodeType.Black;
                        workNode.Color = RedBlackNodeType.Black;
                        // grandparent must be red. Why? Every red node that is not 
                        // a leaf has only black children 
                        insertedNode.Parent.Parent.Color = RedBlackNodeType.Red;
                        insertedNode = insertedNode.Parent.Parent;	// continue loop with grandparent
                    }
                    else
                    {
                        // uncle is black; determine if newNode is greater than Parent
                        if (insertedNode == insertedNode.Parent.Right)
                        {	// yes, newNode is greater than Parent; rotate Left
                            // make newNode a Left child
                            insertedNode = insertedNode.Parent;
                            RotateLeft(insertedNode);
                        }
                        // no, newNode is less than Parent
                        insertedNode.Parent.Color = RedBlackNodeType.Black;	// make Parent black
                        insertedNode.Parent.Parent.Color = RedBlackNodeType.Red;		// make grandparent black
                        RotateRight(insertedNode.Parent.Parent);					// rotate right
                    }
                }
                else
                {	// newNode's Parent is on the Right subtree
                    // this code is the same as above with "Left" and "Right" swapped
                    workNode = insertedNode.Parent.Parent.Left;
                    if (workNode != null && workNode.Color == RedBlackNodeType.Red)
                    {
                        insertedNode.Parent.Color = RedBlackNodeType.Black;
                        workNode.Color = RedBlackNodeType.Black;
                        insertedNode.Parent.Parent.Color = RedBlackNodeType.Red;
                        insertedNode = insertedNode.Parent.Parent;
                    }
                    else
                    {
                        if (insertedNode == insertedNode.Parent.Left)
                        {
                            insertedNode = insertedNode.Parent;
                            RotateRight(insertedNode);
                        }
                        insertedNode.Parent.Color = RedBlackNodeType.Black;
                        insertedNode.Parent.Parent.Color = RedBlackNodeType.Red;
                        RotateLeft(insertedNode.Parent.Parent);
                    }
                }
            }
            _treeBaseNode.Color = RedBlackNodeType.Black;		// rbTree should always be black
        }

        #endregion

    }
}