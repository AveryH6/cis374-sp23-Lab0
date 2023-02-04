using System;
using System.Xml.Linq;

namespace Lab0
{
	public class BinarySearchTree<T> : IBinarySearchTree<T>
	{

        private BinarySearchTreeNode<T> Root { get; set; }

        public BinarySearchTree()
		{
            Root = null;
            Count = 0;
		}

        public bool IsEmpty => Root == null;

        public int Count { get; private set; }

        // TODO
        public int Height => IsEmpty? 0: HeightRecursive(Root);

        private int HeightRecursive(BinarySearchTreeNode<T> node)
        {
            if (node == null)
            {
                return -1;
            }

                       
            int leftHeight = HeightRecursive(node.Left);
            int rightHeight = HeightRecursive(node.Right);

            return 1 + Math.Max(leftHeight, rightHeight);
        }

        // TODO
        public int? MinKey => MinKeyRecursive(Root);

        private int? MinKeyRecursive(BinarySearchTreeNode<T> node)
        {
            if (node == null)
            {
                return null;
            }
            else if (node.Left == null)
            {
                return node.Key;
            }
            else
            {
                return MinKeyRecursive(node.Left);
            }
        }

        // TODO
        public int? MaxKey => MaxKeyRecursive(Root);

        private int? MaxKeyRecursive(BinarySearchTreeNode<T> node)
        {
            if (node == null)
            {
                return null;
            }
            else if (node.Right == null)
            {
                return node.Key;
            }
            else
            {
                return MinKeyRecursive(node.Right);
            }
        }

        // TODO
        public Tuple<int, T> Min => throw new NotImplementedException();

        // TODO
        public Tuple<int, T> Max => throw new NotImplementedException();

        // TODO
        //public double MedianKey => throw new NotImplementedException();

        public double MedianKey
        {
            get
            {
                // get the in order keys
                var keys = InOrderKeys;
                //odd
                if( keys.Count % 2 == 1)
                {
                    int middleIndex = keys.Count / 2;
                    return keys[middleIndex];

                }

                else
                {
                    int middleIndex1 = keys.Count / 2 - 1;
                    int middleIndex2 = keys.Count / 2;

                    int sum = keys[middleIndex1] + keys[middleIndex2];

                    return (double)sum / 2.0;
                }
            }
        }


        public BinarySearchTreeNode<T> GetNode(int key)
        {
            return GetNodeRecursive(Root, key);
        }

        private BinarySearchTreeNode<T>? GetNodeRecursive(BinarySearchTreeNode<T> node, int key)
        {
            if( node == null)
            {
                return null;
            }

            if( node.Key == key )
            {
                return node;
            }

            else if( key < node.Key)
            {
                return GetNodeRecursive(node.Left, key);
            }

            else
            {
                return GetNodeRecursive(node.Right, key);
            }
        }


        public void Add(int key, T value)
        {
            if( Root == null )
            {
                Root = new BinarySearchTreeNode<T>(key, value);
                Count++;
            }
            else
            {
                AddRecursive(key, value, Root);
            }

        }

        private void AddRecursive(int key, T value, BinarySearchTreeNode<T> parent)
        {
            // duplicate found
            // do not add to BST
            if( key == parent.Key )
            {
                return;
            }
            if (key < parent.Key)
            {
                if (parent.Left == null)
                {
                    var newNode = new BinarySearchTreeNode<T>(key, value);
                    parent.Left = newNode;
                    newNode.Parent = parent;
                    Count++;
                }
                else
                {
                    AddRecursive(key, value, parent.Left);
                }
            }
            else
            {
                if( parent.Right == null )
                {
                    var newNode = new BinarySearchTreeNode<T>(key, value);
                    parent.Right = newNode;
                    newNode.Parent = parent;
                    Count++;
                }
                else
                {
                    AddRecursive(key, value, parent.Right);
                }
            }
            
        }

            // TODO
        public void Clear()
        {
            Root = null;
        }

        public bool Contains(int key)
        {

            if( GetNode(key) == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        // TODO
        public BinarySearchTreeNode<T> Next(BinarySearchTreeNode<T> node)
        {

            if (node.Right != null)
            {
                return MinNode(node.Right);
            }

            var p = node.Parent;
            while (p != null && node == p.Right)
            {
                node = p;
                p = p.Parent;
            }
            return p;
        }

        // TODO
        public BinarySearchTreeNode<T> Prev(BinarySearchTreeNode<T> node)
        {
            

            if (node.Left != null)
            {
                return MinNode(node.Left);
            }

            var p = node.Parent;
            while (p != null && node == p.Left)
            {
                node = p;
                p = p.Parent;
            }

            return p;

            
        }

        // TODO

        public List<BinarySearchTreeNode<T>> RangeSearch(int min, int max)
        {
            // make a list

            // find min node ?

            // until max is reached, find next node, add to list
            //

            // find closest node greater than or equal to min

            // METHOD 2 ==> Use InOrderKey

            List<BinarySearchTreeNode<T>> nodeList = new List<BinarySearchTreeNode<T>>();

            if( min > max)
            {
                return nodeList;
            }



            var orderedKeys = this.InOrderKeys;

            foreach( int key in orderedKeys)
            {
                if(key >= min && key <= max)
                {
                    nodeList.Add(GetNode(key));
                }
            }

            return nodeList;
        }

        public void Remove(int key)
        {
            var node = GetNode(key);
            var parent = node.Parent;
            var current = node.Parent;

            if( node == null)
            {
                return;
            }

            Count--;

            // 1) leaf node
            if (node.Left == null && node.Right == null)
            {
                if( parent.Left == node)
                {
                    parent.Left = null;
                    node.Parent = null;
                }
                else if( parent.Right == node )
                {
                    parent.Right = null;
                    node.Parent = null;
                }
                return;

            }

            // 2) parent with one child
            if( node.Left == null && node.Right != null)
            {
                // only has a right child
                var child = node.Right;
                if( parent.Left == node)
                {
                    parent.Left = child;
                    child.Parent = parent;

                    node.Parent = null;
                    node.Left = null;
                }
                else if ( parent.Right == node)
                {
                    parent.Right = child;
                    child.Parent = parent;

                    node.Parent = null;
                    node.Right = null;
                }
                return;
            }

            if( node.Left != null && node.Right == null)
            {
                var child = node.Left;
                if (parent.Left == node)
                {
                    parent.Left = child;
                    child.Parent = parent;

                    node.Parent = null;
                    node.Left = null;
                }
                else if (parent.Right == node)
                {
                    parent.Right = child;
                    child.Parent = parent;

                    node.Parent = null;
                    node.Right = null;
                }
                return;
            }

            // 3) parent with 2 children

            // Find the node to remove

            // Find the next node ( successor )

            // Swap key and data from sucessor to node
            // Remove the successor ( a leaf node ) ( like case 1 )

            //else
            //{
            //    var successor = GetSuccessor(current);
            //    if( current == Root )
            //    {
            //        Root = successor;
            //    }
            //    else if( node == successor.Left)
            //    {
            //        parent.Left = successor;
            //    }
            //    else
            //    {
            //        parent.Right = successor;

            //    }
            //    successor.Left = current.Left;
            //    successor.Right = current.Right;
               

            //}
            //return;


        }

        //public BinarySearchTreeNode<T> GetSuccessor(BinarySearchTreeNode<T> node)
        //{
        //    var successorParent = node;
        //    var successor = node;
        //    var current = node.Right;
        //    while (!(current == null))
        //    {
        //        successorParent = current;
        //        successor = current;
        //        current = current.Left;

        //    }
        //    if (!(successor == node.Right))
        //    {
        //        successorParent.Left = successor.Right;
        //        successor.Right = node.Right;

        //    }
        //    return successor;
        //}

        //public BinarySearchTreeNode<T> GetSuccessor( BinarySearchTreeNode<T> node)
        //{
        //    var successorParent = node;
        //    var successor = node;
        //    var current = node.Right;
        //    while (!(current == null))
        //    {
        //        successorParent = current;
        //        successor = current;
        //        current = current.Left;

        //    }
        //    if( !(successor == node.Right))
        //    {
        //        successorParent.Left = successor.Right;
        //        successor.Right = node.Right;
        //    }
        //}

        // TODO
        public T Search(int key)
        {
            if(Contains(key))
            {
                return GetNode(key).Value;
            }
            else
            {
                return default(T);
            }
        }

        // TODO
        public void Update(int key, T value)
        {
            throw new NotImplementedException();
        }


        // TODO
        public List<int> InOrderKeys
        {
            get
            {
                List<int> keys = new List<int>();
                InOrderKeysRecursive(Root, keys);

                return keys;

            }
        }

        private void InOrderKeysRecursive(BinarySearchTreeNode<T> node, List<int> keys)
        {
            // left, root, right

            if( node == null)
            {
                return;
            }

            InOrderKeysRecursive(node.Left, keys);
            keys.Add(node.Key);
            InOrderKeysRecursive(node.Right, keys);

        }

        // TODO
        public List<int> PreOrderKeys
        {
            get
            {
                List<int> keys = new List<int>();
                PreOrderKeysRecursive(Root, keys);

                return keys;
            }
        }

        private void PreOrderKeysRecursive(BinarySearchTreeNode<T> node, List<int> keys)
        {
            if (node == null)
            {
                return;
            }

            keys.Add(node.Key);
            PreOrderKeysRecursive(node.Left, keys);
            PreOrderKeysRecursive(node.Right, keys);
        }

        // TODO
        public List<int> PostOrderKeys
        {
            get
            {
                List<int> keys = new List<int>();
                PostOrderKeysRecursive(Root, keys);
                return keys;
            }
        }

        Tuple<int, T> IBinarySearchTree<T>.Min
        {
            get
            {
                if( IsEmpty)
                {
                    return null;
                }

                var minNode = MinNode(Root);
                return Tuple.Create(minNode.Key, minNode.Value);
            }
        }

        private void PostOrderKeysRecursive(BinarySearchTreeNode<T> node, List<int> keys)
        {
            if (node == null)
            {
                return;
            }
            
            PostOrderKeysRecursive(node.Left, keys);
            PostOrderKeysRecursive(node.Right, keys);
            keys.Add(node.Key);
        }

        public BinarySearchTreeNode<T> MinNode(BinarySearchTreeNode<T> node)
        {
            return MinNodeRecursive(node);
        }

        private BinarySearchTreeNode<T> MinNodeRecursive(BinarySearchTreeNode<T> node)
        {
            if (node.Left == null)
            {
                return node;
            }

            return MinNodeRecursive(node.Left);
        }

        public BinarySearchTreeNode<T> MaxNode(BinarySearchTreeNode<T> node)
        {
            return MaxNodeRecursive(node);
        }

        private BinarySearchTreeNode<T> MaxNodeRecursive(BinarySearchTreeNode<T> node)
        {
            if (node.Right == null)
            {
                return node;
            }

            return MaxNodeRecursive(node.Right);
        }
    }
}

