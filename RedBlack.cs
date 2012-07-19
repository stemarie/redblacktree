// A red-black tree must satisfy these properties:
//
// 1. The root is black. 
// 2. All leaves are black. 
// 3. Red nodes can only have black children. 
// 4. All paths from a node to its leaves contain the same number of black nodes.

namespace System.Collections.Generic.RedBlack
{
    public class RedBlack<T> : IEnumerable<T> where T
        : class, IComparable<T>
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

        // sentinelNode is convenient way of indicating a leaf node.
        // set up the sentinel node. the sentinel node is the key to a successfull
        // implementation and for understanding the red-black tree properties.
        protected internal static readonly RedBlackNode<T> SentinelNode =
            new RedBlackNode<T>
                {
                    Left = null,
                    Right = null,
                    Parent = null,
                    Color = RedBlackNodeType.Black
                };

        // the node that was last found; used to optimize searches
        private RedBlackNode<T> _lastNodeFound = SentinelNode;

        // locking object to accomodate multi-threading
        private readonly object _lock = new object();

        private readonly Random _rand = new Random();

        public RedBlack()
        {
            Initialize(base.ToString() + _rand.Next());
        }

        public RedBlack(string strIdentifier)
        {
            Initialize(strIdentifier);
        }

        public T this[IComparable key]
        {
            get { return GetNode(key).Data; }
            set { GetNode(key).Data = value; }
        }

        public int Count { get; private set; }

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
            int result;
            // create new node
            RedBlackNode<T> node = new RedBlackNode<T>(data);
            RedBlackNode<T> temp = _treeBaseNode; // grab the rbTree node of the tree

            while (temp != SentinelNode)
            {
                // find Parent
                node.Parent = temp;
                result = data.CompareTo(temp.Data);
                if (result == 0)
                    throw (new RedBlackException(Properties.Resources.ExceptionNodeWithSameKeyAlreadyExists));
                temp = result > 0 ? temp.Right : temp.Left;
            }

            // insert node into tree starting at parent's location
            if (node.Parent != null)
            {
                result = node.Data.CompareTo(node.Parent.Data);
                if (result > 0)
                    node.Parent.Right = node;
                else
                    node.Parent.Left = node;
            }
            else
                _treeBaseNode = node; // first node added

            BalanceTreeAfterInsert(node); // restore red-black properties

            _lastNodeFound = node;

            Count = Count + 1;
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
            RedBlackNode<T> treeNode = _treeBaseNode;

            if (treeNode == null || treeNode == SentinelNode)
                throw (new RedBlackException(Properties.Resources.ExceptionTreeIsEmpty));

            // traverse to the extreme left to find the smallest key
            while (treeNode.Left != SentinelNode)
                treeNode = treeNode.Left;

            _lastNodeFound = treeNode;

            return treeNode.Data as IComparable;
        }

        ///<summary>
        /// GetMaxKey
        /// Returns the maximum key value
        ///</summary>
        public IComparable GetMaxKey()
        {
            RedBlackNode<T> treeNode = _treeBaseNode;

            if (treeNode == null || treeNode == SentinelNode)
                throw (new RedBlackException(Properties.Resources.ExceptionTreeIsEmpty));

            // traverse to the extreme right to find the largest key
            while (treeNode.Right != SentinelNode)
                treeNode = treeNode.Right;

            _lastNodeFound = treeNode;

            return treeNode.Data as IComparable;
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
            Stack<T> stack = new Stack<T>();

            // use depth-first traversal to push nodes into stack
            // the lowest node will be at the top of the stack

            if (_treeBaseNode != SentinelNode)
            {
                WalkNextLevel(_treeBaseNode, stack);
            }

            return stack.GetEnumerator();
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

            RedBlackNode<T> y;					// work node 

            // find the replacement node (the successor to x) - the node one with 
            // at *most* one child. 
            if (node.Left == SentinelNode || node.Right == SentinelNode)
                y = node;						// node has sentinel as a child
            else
            {
                // z has two children, find replacement node which will 
                // be the leftmost node greater than z
                y = node.Right;				        // traverse right subtree	
                while (y.Left != SentinelNode)		// to find next node in sequence
                    y = y.Left;
            }

            // at this point, y contains the replacement node. it's content will be copied 
            // to the valules in the node to be deleted

            // x (y's only child) is the node that will be linked to y's old parent. 
            RedBlackNode<T> x = y.Left != SentinelNode ? y.Left : y.Right;

            // replace x's parent with y's parent and
            // link x to proper subtree in parent
            // this removes y from the chain
            x.Parent = y.Parent;
            if (y.Parent != null)
                if (y == y.Parent.Left)
                    y.Parent.Left = x;
                else
                    y.Parent.Right = x;
            else
                _treeBaseNode = x;			// make x the root node

            // copy the values from y (the replacement node) to the node being deleted.
            // note: this effectively deletes the node. 
            if (y != node)
            {
                node.Data = y.Data;
            }

            if (y.Color == RedBlackNodeType.Black)
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

            RedBlackNode<T> tempWorkNode;

            while (node != _treeBaseNode && node.Color == RedBlackNodeType.Black)
            {
                if (node == node.Parent.Left)			// determine sub tree from parent
                {
                    tempWorkNode = node.Parent.Right;			// y is x's sibling 
                    if (tempWorkNode.Color == RedBlackNodeType.Red)
                    {	// x is black, y is red - make both black and rotate
                        node.Parent.Color = RedBlackNodeType.Red;
                        tempWorkNode.Color = RedBlackNodeType.Black;
                        RotateLeft(node.Parent);
                        tempWorkNode = node.Parent.Right;
                    }
                    if (tempWorkNode.Left.Color == RedBlackNodeType.Black &&
                        tempWorkNode.Right.Color == RedBlackNodeType.Black)
                    {	// children are both black
                        tempWorkNode.Color = RedBlackNodeType.Red;		// change parent to red
                        node = node.Parent;					// move up the tree
                    }
                    else
                    {
                        if (tempWorkNode.Right.Color == RedBlackNodeType.Black)
                        {
                            tempWorkNode.Left.Color = RedBlackNodeType.Black;
                            tempWorkNode.Color = RedBlackNodeType.Red;
                            RotateRight(tempWorkNode);
                            tempWorkNode = node.Parent.Right;
                        }
                        node.Parent.Color = RedBlackNodeType.Black;
                        tempWorkNode.Color = node.Parent.Color;
                        tempWorkNode.Right.Color = RedBlackNodeType.Black;
                        RotateLeft(node.Parent);
                        node = _treeBaseNode;
                    }
                }
                else
                {	// right subtree - same as code above with right and left swapped
                    tempWorkNode = node.Parent.Left;
                    if (tempWorkNode.Color == RedBlackNodeType.Red)
                    {
                        node.Parent.Color = RedBlackNodeType.Red;
                        tempWorkNode.Color = RedBlackNodeType.Black;
                        RotateRight(node.Parent);
                        tempWorkNode = node.Parent.Left;
                    }
                    if (tempWorkNode.Right.Color == RedBlackNodeType.Black &&
                        tempWorkNode.Left.Color == RedBlackNodeType.Black)
                    {
                        tempWorkNode.Color = RedBlackNodeType.Red;
                        node = node.Parent;
                    }
                    else
                    {
                        if (tempWorkNode.Left.Color == RedBlackNodeType.Black)
                        {
                            tempWorkNode.Right.Color = RedBlackNodeType.Black;
                            tempWorkNode.Color = RedBlackNodeType.Red;
                            RotateLeft(tempWorkNode);
                            tempWorkNode = node.Parent.Left;
                        }
                        tempWorkNode.Color = node.Parent.Color;
                        node.Parent.Color = RedBlackNodeType.Black;
                        tempWorkNode.Left.Color = RedBlackNodeType.Black;
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

            RedBlackNode<T> y = node.Left;			// get x's Left node, this becomes y

            // set x's Right link
            node.Left = y.Right;					// y's Right child becomes x's Left child

            // modify parents
            if (y.Right != SentinelNode)
                y.Right.Parent = node;				// sets y's Right Parent to x

            if (y != SentinelNode)
                y.Parent = node.Parent;			// set y's Parent to x's Parent

            if (node.Parent != null)				// null=rbTree, could also have used rbTree
            {	// determine which side of it's Parent x was on
                if (node == node.Parent.Right)
                    node.Parent.Right = y;			// set Right Parent to y
                else
                    node.Parent.Left = y;			// set Left Parent to y
            }
            else
                _treeBaseNode = y;						// at rbTree, set it to y

            // link x and y 
            y.Right = node;						// put x on y's Right
            if (node != SentinelNode)				// set y as x's Parent
                node.Parent = y;
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

            RedBlackNode<T> y = node.Right;			// get x's Right node, this becomes y

            // set x's Right link
            node.Right = y.Left;					// y's Left child's becomes x's Right child

            // modify parents
            if (y.Left != SentinelNode)
                y.Left.Parent = node;				// sets y's Left Parent to x

            if (y != SentinelNode)
                y.Parent = node.Parent;			// set y's Parent to x's Parent

            if (node.Parent != null)
            {	// determine which side of it's Parent x was on
                if (node == node.Parent.Left)
                    node.Parent.Left = y;			// set Left Parent to y
                else
                    node.Parent.Right = y;			// set Right Parent to y
            }
            else
                _treeBaseNode = y;						// at rbTree, set it to y

            // link x and y 
            y.Left = node;							// put x on y's Left 
            if (node != SentinelNode)						// set y as x's Parent
                node.Parent = y;
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

            RedBlackNode<T> workNode;

            // maintain red-black tree properties after adding newNode
            while (node != _treeBaseNode && node.Parent.Color == RedBlackNodeType.Red)
            {
                // Parent node is .Colored red; 
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