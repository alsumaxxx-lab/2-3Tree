using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp50
{
    internal class TwoThreeTree
    {
        private Node root;
        public int OperationCount { get; private set; }

        public TwoThreeTree()
        {
            root = null;
            OperationCount = 0;
        }

        private void ResetOperations() => OperationCount = 0;

        public bool Insert(int key)
        {
            ResetOperations();
            if (root == null)
            {
                root = new Node();
                root.keys.Add(key);
                OperationCount++;
                return true;
            }

            Node leaf = FindLeaf(key);
            OperationCount++;

            if (leaf.keys.Contains(key))
            {
                OperationCount++;
                return false;
            }

            InsertIntoNode(leaf, key);
            return true;
        }

        private Node FindLeaf(int key)
        {
            Node current = root;
            while (!current.IsLeaf)
            {
                OperationCount++;
                int i = 0;
                while (i < current.keys.Count && key > current.keys[i])
                {
                    OperationCount++;
                    i++;
                }
                current = current.children[i];
                OperationCount++;
            }
            return current;
        }

        private void InsertIntoNode(Node node, int key)
        {
            OperationCount++;
            node.keys.Add(key);
            node.keys.Sort();

            if (!node.IsFull) return;

            int midKey = node.keys[1];
            Node left = new Node();
            Node right = new Node();

            left.keys.Add(node.keys[0]);
            right.keys.Add(node.keys[2]);

            if (!node.IsLeaf)
            {
                left.children = new List<Node> { node.children[0], node.children[1] };
                right.children = new List<Node> { node.children[2], node.children[3] };

                foreach (var child in left.children) child.parent = left;
                foreach (var child in right.children) child.parent = right;
                OperationCount += 4;
            }

            OperationCount += 3;

            if (node.parent == null)
            {
                root = new Node();
                root.keys.Add(midKey);
                root.children = new List<Node> { left, right };
                left.parent = root;
                right.parent = root;
                OperationCount += 3;
            }
            else
            {
                left.parent = node.parent;
                right.parent = node.parent;
                InsertIntoParent(node.parent, midKey, left, right, node);
            }
        }

        private void InsertIntoParent(Node parent, int key, Node left, Node right, Node oldChild)
        {
            OperationCount++;
            int index = parent.children.IndexOf(oldChild);
            parent.children.RemoveAt(index);
            parent.children.Insert(index, left);
            parent.children.Insert(index + 1, right);
            parent.keys.Add(key);
            parent.keys.Sort();

            if (parent.IsFull)
            {
                int midKey = parent.keys[1];
                Node newLeft = new Node();
                Node newRight = new Node();

                newLeft.keys.Add(parent.keys[0]);
                newRight.keys.Add(parent.keys[2]);

                if (!parent.IsLeaf)
                {
                    newLeft.children = new List<Node> { parent.children[0], parent.children[1] };
                    newRight.children = new List<Node> { parent.children[2], parent.children[3] };
                    foreach (var child in newLeft.children) child.parent = newLeft;
                    foreach (var child in newRight.children) child.parent = newRight;
                    OperationCount += 4;
                }

                OperationCount += 3;

                if (parent.parent == null)
                {
                    root = new Node();
                    root.keys.Add(midKey);
                    root.children = new List<Node> { newLeft, newRight };
                    newLeft.parent = root;
                    newRight.parent = root;
                    OperationCount += 3;
                }
                else
                {
                    newLeft.parent = parent.parent;
                    newRight.parent = parent.parent;
                    InsertIntoParent(parent.parent, midKey, newLeft, newRight, parent);
                }
            }
        }

        public bool Search(int key)
        {
            ResetOperations();
            Node current = root;
            if (current == null) return false;

            while (current != null)
            {
                OperationCount++;
                if (current.keys.Contains(key))
                {
                    OperationCount++;
                    return true;
                }

                if (current.IsLeaf) return false;

                int i = 0;
                while (i < current.keys.Count && key > current.keys[i])
                {
                    OperationCount++;
                    i++;
                }
                current = current.children[i];
                OperationCount++;
            }
            return false;
        }

        public bool Delete(int key)
        {
            ResetOperations();
            if (root == null) return false;

            Node node = FindNodeWithKey(key);
            if (node == null) return false;

            if (!node.IsLeaf)
            {
                Node successor = FindSuccessor(node, key);
                int successorKey = successor.keys[0];
                OperationCount++;
                node.keys[node.keys.IndexOf(key)] = successorKey;
                key = successorKey;
                node = successor;
            }

            node.keys.Remove(key);
            OperationCount++;

            if (node == root)
            {
                if (node.keys.Count == 0) root = null;
                return true;
            }

            FixTree(node);
            return true;
        }

        private Node FindNodeWithKey(int key)
        {
            Node current = root;
            while (current != null)
            {
                OperationCount++;
                if (current.keys.Contains(key))
                {
                    OperationCount++;
                    return current;
                }
                if (current.IsLeaf) return null;

                int i = 0;
                while (i < current.keys.Count && key > current.keys[i])
                {
                    OperationCount++;
                    i++;
                }
                current = current.children[i];
                OperationCount++;
            }
            return null;
        }

        private Node FindSuccessor(Node node, int key)
        {
            int index = node.keys.IndexOf(key);
            Node current = node.children[index + 1];
            OperationCount += 2;

            while (!current.IsLeaf)
            {
                current = current.children[0];
                OperationCount++;
            }
            return current;
        }

        private void FixTree(Node node)
        {
            while (node != null && node.keys.Count == 0)
            {
                if (node == root)
                {
                    root = null;
                    return;
                }

                Node parent = node.parent;
                int childIndex = parent.children.IndexOf(node);
                Node leftSibling = childIndex > 0 ? parent.children[childIndex - 1] : null;
                Node rightSibling = childIndex < parent.children.Count - 1 ? parent.children[childIndex + 1] : null;
                OperationCount += 3;

                if (leftSibling != null && leftSibling.keys.Count > 1)
                {
                    node.keys.Insert(0, parent.keys[childIndex - 1]);
                    parent.keys[childIndex - 1] = leftSibling.keys.Last();
                    leftSibling.keys.RemoveAt(leftSibling.keys.Count - 1);
                    OperationCount += 4;
                    return;
                }
                else if (rightSibling != null && rightSibling.keys.Count > 1)
                {
                    node.keys.Add(parent.keys[childIndex]);
                    parent.keys[childIndex] = rightSibling.keys.First();
                    rightSibling.keys.RemoveAt(0);
                    OperationCount += 4;
                    return;
                }
                else
                {
                    if (leftSibling != null)
                    {
                        MergeNodes(leftSibling, node, parent, childIndex - 1);
                        node = parent;
                    }
                    else if (rightSibling != null)
                    {
                        MergeNodes(node, rightSibling, parent, childIndex);
                        node = parent;
                    }
                    OperationCount += 2;
                }
            }
        }

        private void MergeNodes(Node left, Node right, Node parent, int keyIndex)
        {
            left.keys.Add(parent.keys[keyIndex]);
            left.keys.AddRange(right.keys);
            left.keys.Sort();
            OperationCount += 3;

            if (!left.IsLeaf)
            {
                left.children.AddRange(right.children);
                foreach (var child in right.children) child.parent = left;
                OperationCount++;
            }

            parent.children.RemoveAt(keyIndex + 1);
            parent.keys.RemoveAt(keyIndex);
            OperationCount += 2;
        }
    }
}
