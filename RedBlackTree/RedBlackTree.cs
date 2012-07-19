// A red-black tree must satisfy these properties:
//
// 1. The root is black. 
// 2. All leaves are black. 
// 3. Red nodes can only have black children. 
// 4. All paths from a node to its leaves contain the same number of black nodes.

namespace System.Collections.Generic.RedBlack
{
    public class RedBlackTree<T> : ICollection<T>
        where T : class, IComparable
    {
        public enum RedBlackNodeType
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
        private RedBlackNode<T> _treeBaseNode = SentinelNode;

        // the node that was last found; used to optimize searches
        private RedBlackNode<T> _lastNodeFound = SentinelNode;

        private readonly Random _rand = new Random();

        // sentinelNode is convenient way of indicating a leaf node.
        // set up the sentinel node. the sentinel node is the key to a successfull
        // implementation and for understanding the red-black tree properties.
        internal static readonly RedBlackNode<T> SentinelNode =
            new RedBlackNode<T>
            {
                Left = null,
                Right = null,
                Parent = null,
                Color = RedBlackNodeType.Black
            };

        public RedBlackTree()
        {
            Initialize(base.ToString() + _rand.Next());
        }

        public RedBlackTree(string strIdentifier)
        {
            Initialize(strIdentifier);
        }

        public T this[IComparable key]
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
        public bool Remove(T item)
        {
            try
            {
                Delete(GetNode(item));
                return true;
            }
            catch (RedBlackException)
            {
                return false;
            }
        }

        public int Count { get; private set; }

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
        /// Add
        /// args: ByVal key As IComparable, ByVal data As T
        /// key is object that implements IComparable interface
        ///</summary>
        public void Add(T data)
        {
            if (data == null)
                throw (new RedBlackException(Properties.Resources.ExceptionNodeKeyAndDataMustNotBeNull));

            // traverse tree - find where node belongs
            // create new node
            RedBlackNode<T> _newNode = new RedBlackNode<T>(data);
            RedBlackNode<T> _workNode = _treeBaseNode; // grab the rbTree node of the tree

            while (_workNode != SentinelNode)
            {
                // find Parent
                _newNode.Parent = _workNode;
                int result = data.CompareTo(_workNode.Data);
                if (result == 0)
                    throw (new RedBlackException(Properties.Resources.ExceptionNodeWithSameKeyAlreadyExists));
                _workNode = result > 0 ? _workNode.Right : _workNode.Left;
            }

            // insert node into tree starting at parent's location
            if (_newNode.Parent != null)
            {
                if (_newNode.Data.CompareTo(_newNode.Parent.Data) > 0)
                    _newNode.Parent.Right = _newNode;
                else
                    _newNode.Parent.Left = _newNode;
            }
            else
                _treeBaseNode = _newNode; // first node added

            BalanceTreeAfterInsert(_newNode); // restore red-black properties

            _lastNodeFound = _newNode;

            Count++;
        }

        ///<summary>
        /// GetData
        /// Gets the data object associated with the specified key
        ///</summary>
        public T GetData(IComparable key)
        {
            return GetNode(key).Data;
        }

        ///<summary>
        /// GetMinKey
        /// Returns the minimum key value
        ///</summary>
        public IComparable GetMinKey()
        {
            RedBlackNode<T> _workNode = _treeBaseNode;

            if (_workNode == null || _workNode == SentinelNode)
                throw (new RedBlackException(Properties.Resources.ExceptionTreeIsEmpty));

            // traverse to the extreme left to find the smallest key
            while (_workNode.Left != SentinelNode)
                _workNode = _workNode.Left;

            _lastNodeFound = _workNode;

            return _workNode.Data;
        }

        ///<summary>
        /// GetMaxKey
        /// Returns the maximum key value
        ///</summary>
        public IComparable GetMaxKey()
        {
            RedBlackNode<T> _workNode = _treeBaseNode;

            if (_workNode == null || _workNode == SentinelNode)
                throw (new RedBlackException(Properties.Resources.ExceptionTreeIsEmpty));

            // traverse to the extreme right to find the largest key
            while (_workNode.Right != SentinelNode)
                _workNode = _workNode.Right;

            _lastNodeFound = _workNode;

            return _workNode.Data;
        }

        ///<summary>
        /// GetMinValue
        /// Returns the object having the minimum key value
        ///</summary>
        public T GetMinValue()
        {
            return GetData(GetMinKey());
        }

        ///<summary>
        /// GetMaxValue
        /// Returns the object having the maximum key
        ///</summary>
        public T GetMaxValue()
        {
            return GetData(GetMaxKey());
        }

        ///<summary>
        /// GetEnumerator
        /// return an enumerator that returns the tree nodes in order
        ///</summary>
        public IEnumerator<T> GetEnumerator()
        {
            Stack<T> stack = GetAll();

            return stack.GetEnumerator();
        }

        private Stack<T> GetAll()
        {
            Stack<T> stack = new Stack<T>();

            // use depth-first traversal to push nodes into stack
            // the lowest node will be at the top of the stack

            if (_treeBaseNode != SentinelNode)
            {
                WalkNextLevel(_treeBaseNode, stack);
            }
            return stack;
        }

        ///<summary>
        /// IsEmpty
        /// Is the tree empty?
        ///</summary>
        public bool IsEmpty()
        {
            return (_treeBaseNode == null);
        }

        ///<summary>
        /// Remove
        /// removes the key and data object (delete)
        ///</summary>
        public void Remove(IComparable key)
        {
            if (key == null)
                throw (new RedBlackException(Properties.Resources.ExceptionNodeKeyIsNull));

            // find node

            RedBlackNode<T> node = GetNode(key);

            if (node != SentinelNode)
                Delete(node);

            Count = Count - 1;
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

        ///<summary>
        /// Clear
        /// Empties or clears the tree
        ///</summary>
        public void Clear()
        {
            _treeBaseNode = SentinelNode;
            Count = 0;
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
        /// </returns>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        public bool Contains(T item)
        {
            try
            {
                return GetData(item) != null;
            }
            catch (RedBlackException)
            {
                return false;
            }
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="array"/> is multidimensional.-or-The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.-or-Type <paramref name="T"/> cannot be cast automatically to the type of the destination <paramref name="array"/>.</exception>
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException();
            if (array == null)
                throw new ArgumentNullException();
            if ((array.Length - arrayIndex) < Count)
                throw new ArgumentException();
            int _currentPosition = arrayIndex;
            foreach (T item in GetAll())
            {
                array[_currentPosition] = item;
                _currentPosition++;
            }
        }

        ///<summary>
        /// Equals
        ///</summary>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (!(obj is RedBlackNode<T>))
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

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        ///<summary>
        /// ToString
        ///</summary>
        public override string ToString()
        {
            return _strIdentifier;
        }

        #region "Private Methods"

        ///<summary>
        /// Delete
        /// Delete a node from the tree and restore red black properties
        ///</summary>
        private void Delete(RedBlackNode<T> node)
        {
            // A node to be deleted will be: 
            //		1. a leaf with no children
            //		2. have one child
            //		3. have two children
            // If the deleted node is red, the red black properties still hold.
            // If the deleted node is black, the tree needs rebalancing

            RedBlackNode<T> _workNode;					// work node 

            // find the replacement node (the successor to x) - the node one with 
            // at *most* one child. 
            if (node.Left == SentinelNode || node.Right == SentinelNode)
                _workNode = node;						// node has sentinel as a child
            else
            {
                // z has two children, find replacement node which will 
                // be the leftmost node greater than z
                _workNode = node.Right;				        // traverse right subtree	
                while (_workNode.Left != SentinelNode)		// to find next node in sequence
                    _workNode = _workNode.Left;
            }

            // at this point, y contains the replacement node. it's content will be copied 
            // to the valules in the node to be deleted

            // x (y's only child) is the node that will be linked to y's old parent. 
            RedBlackNode<T> x = _workNode.Left != SentinelNode ? _workNode.Left : _workNode.Right;

            // replace x's parent with y's parent and
            // link x to proper subtree in parent
            // this removes y from the chain
            x.Parent = _workNode.Parent;
            if (_workNode.Parent != null)
                if (_workNode == _workNode.Parent.Left)
                    _workNode.Parent.Left = x;
                else
                    _workNode.Parent.Right = x;
            else
                _treeBaseNode = x;			// make x the root node

            // copy the values from y (the replacement node) to the node being deleted.
            // note: this effectively deletes the node. 
            if (_workNode != node)
            {
                node.Data = _workNode.Data;
            }

            if (_workNode.Color == RedBlackNodeType.Black)
                BalanceTreeAfterDelete(x);

            _lastNodeFound = SentinelNode;
        }

        ///<summary>
        /// BalanceTreeAfterDelete
        /// Deletions from red-black trees may destroy the red-black 
        /// properties. Examine the tree and restore. Rotations are normally 
        /// required to restore it
        ///</summary>
        private void BalanceTreeAfterDelete(RedBlackNode<T> node)
        {
            // maintain Red-Black tree balance after deleting node 			

            while (node != _treeBaseNode && node.Color == RedBlackNodeType.Black)
            {
                RedBlackNode<T> _workNode;
                if (node == node.Parent.Left)			// determine sub tree from parent
                {
                    _workNode = node.Parent.Right;			// y is x's sibling 
                    if (_workNode.Color == RedBlackNodeType.Red)
                    {	// x is black, y is red - make both black and rotate
                        node.Parent.Color = RedBlackNodeType.Red;
                        _workNode.Color = RedBlackNodeType.Black;
                        RotateLeft(node.Parent);
                        _workNode = node.Parent.Right;
                    }
                    if (_workNode.Left.Color == RedBlackNodeType.Black &&
                        _workNode.Right.Color == RedBlackNodeType.Black)
                    {	// children are both black
                        _workNode.Color = RedBlackNodeType.Red;		// change parent to red
                        node = node.Parent;					// move up the tree
                    }
                    else
                    {
                        if (_workNode.Right.Color == RedBlackNodeType.Black)
                        {
                            _workNode.Left.Color = RedBlackNodeType.Black;
                            _workNode.Color = RedBlackNodeType.Red;
                            RotateRight(_workNode);
                            _workNode = node.Parent.Right;
                        }
                        node.Parent.Color = RedBlackNodeType.Black;
                        _workNode.Color = node.Parent.Color;
                        _workNode.Right.Color = RedBlackNodeType.Black;
                        RotateLeft(node.Parent);
                        node = _treeBaseNode;
                    }
                }
                else
                {	// right subtree - same as code above with right and left swapped
                    _workNode = node.Parent.Left;
                    if (_workNode.Color == RedBlackNodeType.Red)
                    {
                        node.Parent.Color = RedBlackNodeType.Red;
                        _workNode.Color = RedBlackNodeType.Black;
                        RotateRight(node.Parent);
                        _workNode = node.Parent.Left;
                    }
                    if (_workNode.Right.Color == RedBlackNodeType.Black &&
                        _workNode.Left.Color == RedBlackNodeType.Black)
                    {
                        _workNode.Color = RedBlackNodeType.Red;
                        node = node.Parent;
                    }
                    else
                    {
                        if (_workNode.Left.Color == RedBlackNodeType.Black)
                        {
                            _workNode.Right.Color = RedBlackNodeType.Black;
                            _workNode.Color = RedBlackNodeType.Red;
                            RotateLeft(_workNode);
                            _workNode = node.Parent.Left;
                        }
                        _workNode.Color = node.Parent.Color;
                        node.Parent.Color = RedBlackNodeType.Black;
                        _workNode.Left.Color = RedBlackNodeType.Black;
                        RotateRight(node.Parent);
                        node = _treeBaseNode;
                    }
                }
            }
            node.Color = RedBlackNodeType.Black;
        }

        private static void WalkNextLevel(RedBlackNode<T> node, Stack<T> stack)
        {
            if (node.Right != SentinelNode)
                WalkNextLevel(node.Right, stack);
            stack.Push(node.Data);
            if (node.Left != SentinelNode)
                WalkNextLevel(node.Left, stack);
        }

        private RedBlackNode<T> GetNode(IComparable key)
        {
            int result = key.CompareTo(_lastNodeFound.Data);
            if (result == 0)
                return _lastNodeFound;

            RedBlackNode<T> treeNode = _treeBaseNode; // begin at root

            // traverse tree until node is found
            while (treeNode != SentinelNode)
            {
                result = key.CompareTo(treeNode.Data);
                if (result == 0)
                {
                    _lastNodeFound = treeNode;
                    return treeNode;
                }
                treeNode = result < 0 ? treeNode.Left : treeNode.Right;
            }
            throw (new RedBlackException(Properties.Resources.ExceptionNodeKeyWasNotFound));
        }

        ///<summary>
        /// RotateRight
        /// Rebalance the tree by rotating the nodes to the right
        ///</summary>
        private void RotateRight(RedBlackNode<T> node)
        {
            // pushing node x down and to the Right to balance the tree. x's Left child (y)
            // replaces x (since x < y), and y's Right child becomes x's Left child 
            // (since it's < x but > y).

            RedBlackNode<T> _workNode = node.Left;			// get x's Left node, this becomes y

            // set x's Right link
            node.Left = _workNode.Right;					// y's Right child becomes x's Left child

            // modify parents
            if (_workNode.Right != SentinelNode)
                _workNode.Right.Parent = node;				// sets y's Right Parent to x

            if (_workNode != SentinelNode)
                _workNode.Parent = node.Parent;			// set y's Parent to x's Parent

            if (node.Parent != null)				// null=rbTree, could also have used rbTree
            {	// determine which side of it's Parent x was on
                if (node == node.Parent.Right)
                    node.Parent.Right = _workNode;			// set Right Parent to y
                else
                    node.Parent.Left = _workNode;			// set Left Parent to y
            }
            else
                _treeBaseNode = _workNode;						// at rbTree, set it to y

            // link x and y 
            _workNode.Right = node;						// put x on y's Right
            if (node != SentinelNode)				// set y as x's Parent
                node.Parent = _workNode;
        }

        ///<summary>
        /// RotateLeft
        /// Rebalance the tree by rotating the nodes to the left
        ///</summary>
        private void RotateLeft(RedBlackNode<T> node)
        {
            // pushing node x down and to the Left to balance the tree. x's Right child (y)
            // replaces x (since y > x), and y's Left child becomes x's Right child 
            // (since it's < y but > x).

            RedBlackNode<T> _workNode = node.Right;			// get x's Right node, this becomes y

            // set x's Right link
            node.Right = _workNode.Left;					// y's Left child's becomes x's Right child

            // modify parents
            if (_workNode.Left != SentinelNode)
                _workNode.Left.Parent = node;				// sets y's Left Parent to x

            if (_workNode != SentinelNode)
                _workNode.Parent = node.Parent;			// set y's Parent to x's Parent

            if (node.Parent != null)
            {	// determine which side of it's Parent x was on
                if (node == node.Parent.Left)
                    node.Parent.Left = _workNode;			// set Left Parent to y
                else
                    node.Parent.Right = _workNode;			// set Right Parent to y
            }
            else
                _treeBaseNode = _workNode;						// at rbTree, set it to y

            // link x and y 
            _workNode.Left = node;							// put x on y's Left 
            if (node != SentinelNode)						// set y as x's Parent
                node.Parent = _workNode;
        }

        private void Initialize(string identifier)
        {
            _strIdentifier = identifier;
            _intHashCode = _rand.Next();
        }

        ///<summary>
        /// BalanceTreeAfterInsert
        /// Additions to red-black trees usually destroy the red-black 
        /// properties. Examine the tree and restore. Rotations are normally 
        /// required to restore it
        ///</summary>
        private void BalanceTreeAfterInsert(RedBlackNode<T> node)
        {
            // x and y are used as variable names for brevity, in a more formal
            // implementation, you should probably change the names

            // maintain red-black tree properties after adding newNode
            while (node != _treeBaseNode && node.Parent.Color == RedBlackNodeType.Red)
            {
                // Parent node is .Colored red; 
                RedBlackNode<T> workNode;
                if (node.Parent == node.Parent.Parent.Left)	// determine traversal path			
                {										// is it on the Left or Right subtree?
                    workNode = node.Parent.Parent.Right;			// get uncle
                    if (workNode != null && workNode.Color == RedBlackNodeType.Red)
                    {	// uncle is red; change x's Parent and uncle to black
                        node.Parent.Color = RedBlackNodeType.Black;
                        workNode.Color = RedBlackNodeType.Black;
                        // grandparent must be red. Why? Every red node that is not 
                        // a leaf has only black children 
                        node.Parent.Parent.Color = RedBlackNodeType.Red;
                        node = node.Parent.Parent;	// continue loop with grandparent
                    }
                    else
                    {
                        // uncle is black; determine if newNode is greater than Parent
                        if (node == node.Parent.Right)
                        {	// yes, newNode is greater than Parent; rotate Left
                            // make newNode a Left child
                            node = node.Parent;
                            RotateLeft(node);
                        }
                        // no, newNode is less than Parent
                        node.Parent.Color = RedBlackNodeType.Black;	// make Parent black
                        node.Parent.Parent.Color = RedBlackNodeType.Red;		// make grandparent black
                        RotateRight(node.Parent.Parent);					// rotate right
                    }
                }
                else
                {	// newNode's Parent is on the Right subtree
                    // this code is the same as above with "Left" and "Right" swapped
                    workNode = node.Parent.Parent.Left;
                    if (workNode != null && workNode.Color == RedBlackNodeType.Red)
                    {
                        node.Parent.Color = RedBlackNodeType.Black;
                        workNode.Color = RedBlackNodeType.Black;
                        node.Parent.Parent.Color = RedBlackNodeType.Red;
                        node = node.Parent.Parent;
                    }
                    else
                    {
                        if (node == node.Parent.Left)
                        {
                            node = node.Parent;
                            RotateRight(node);
                        }
                        node.Parent.Color = RedBlackNodeType.Black;
                        node.Parent.Parent.Color = RedBlackNodeType.Red;
                        RotateLeft(node.Parent.Parent);
                    }
                }
            }
            _treeBaseNode.Color = RedBlackNodeType.Black;		// rbTree should always be black
        }

        #endregion

    }
}