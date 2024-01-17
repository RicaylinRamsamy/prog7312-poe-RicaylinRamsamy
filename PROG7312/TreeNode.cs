using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG7312
{


    public class TreeNode
    {
        public string Name { get; set; }
        public string CallNumber { get; set; }
        public List<TreeNode> Children { get; set; }

        // Are there no children? If so it is a call number
        public bool IsLeaf()
        {
            return Children.Count == 0;
        }
    }


    public class Tree
    {
        public List<TreeNode> Branches { get; set; }

        // Return a random call number
        public TreeNode GetRandomLeafNode()
        {
            List<TreeNode> leafNodes = new List<TreeNode>();
            getLeafs(Branches, leafNodes);

            Random seed = new Random();
            int randomIndex = seed.Next(0, leafNodes.Count);
            return leafNodes[randomIndex];
        }

        // Return all the call numbers that do not have children (leafs)
        private void getLeafs(List<TreeNode> nodes, List<TreeNode> leafNodes)
        {
            foreach (var node in nodes)
            {
                if (node.IsLeaf())
                {
                    leafNodes.Add(node);
                }
                else
                {
                    getLeafs(node.Children, leafNodes);
                }
            }
        }


        public TreeNode getParent(TreeNode leafNode)
        {
            return getTopmostBranch(Branches, leafNode);
        }


        // Given a call number like 856 will return 800 (the topmost parent)
        private TreeNode getTopmostBranch(List<TreeNode> nodes, TreeNode leafNode)
        {
            foreach (var node in nodes)
            {
                foreach (var leaf in node.Children)
                {
                    foreach (var x in leaf.Children)
                    {
                        if (x == leafNode)
                        {
                            return node;
                        }
                    }
                }

            }
            return null; // If there is an error

        }
    }
}
