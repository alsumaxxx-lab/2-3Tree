using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp50
{
    internal class Node
    {
        public List<int> keys = new List<int>();
        public List<Node> children = new List<Node>();
        public Node parent;

        public bool IsLeaf => children.Count == 0;
        public bool IsFull => keys.Count == 3;
        public bool IsTwoNode => keys.Count == 1;
        public bool IsThreeNode => keys.Count == 2;
    }
}
