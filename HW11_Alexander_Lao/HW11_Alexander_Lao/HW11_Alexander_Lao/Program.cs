// Alexander Lao
// 11481444

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW11_Alexander_Lao
{
    class BSTNode
    {
        private int data;
        public BSTNode left;
        public BSTNode right;

        // parameterized constructor
        public BSTNode(int newData)
        {
            this.data = newData;
            this.left = null;
            this.right = null;
        }

        // getters
        public int getData()
        {
            return this.data;
        }

        public BSTNode getLeft()
        {
            return this.left;
        }

        public BSTNode getRight()
        {
            return this.right;
        }

        // setters
        public void setData(int newData)
        {
            this.data = newData;
        }

        public void setLeft(BSTNode newLeft)
        {
            this.left = newLeft;
        }

        public void setRight(BSTNode newRight)
        {
            this.right = newRight;
        }
    }

    class BST
    {
        private BSTNode root;

        // default constructor
        public BST()
        {
            this.root = null;
        }

        // public insertNode function will call 
        // the private insertNode function
        public void insertNode(int newData)
        {
            // pass the root as a reference
            // to retain the changes made to it
            insertNode(ref this.root, newData);
        }

        // private insertNode function to hide the root
        private void insertNode(ref BSTNode currentNode, int newData)
        {
            // base case: the currentNode is null
            if (currentNode == null)
            {
                // instantiate a new BSTNode
                BSTNode newNode = new BSTNode(newData);

                // set the currentNode to the newNode
                currentNode = newNode;

                return;
            }
            // the newData is less than the currentNode's data
            else if (newData < (currentNode.getData()))
            {
                // traverse left
                insertNode(ref currentNode.left, newData);
            }
            // the newData is greater than the currentNode's data
            else if (newData > (currentNode.getData()))
            {
                // traverse right
                insertNode(ref currentNode.right, newData);
            }
            // otherwise it's a duplicate
            else
            {
                // do not add the duplicates
                return;
            }
        }

        // calls the private inOrderTraversal
        public void inOrderTraversal()
        {
            Console.WriteLine("Traversal of the tree using recursion:");

            // hides the root
            inOrderTraversal(this.root);

            Console.Write("\n");
        }

        // private inOrderTraversal function 
        private void inOrderTraversal(BSTNode currentNode)
        {
            if (currentNode != null)
            {
                // recursive call to the left
                inOrderTraversal(currentNode.getLeft());

                // print out the data
                Console.Write(currentNode.getData() + " ");

                // recursive call to the right
                inOrderTraversal(currentNode.getRight());
            }
        }

        // algorithm is the one Evan showed us in lecture
        public void inOrderTraversalWithoutRecursion()
        {
            Console.WriteLine("Traversal of the tree using a stack but no recursion:");

            // instantiate a stack to simulate the call stack
            var nodes = new Stack<BSTNode>();

            // starting at the root
            BSTNode current = this.root;

            while (true)
            {
                // if the root is null
                if (current == null)
                {
                    // and there's nothing on the stack
                    if (nodes.Count == 0)
                    {
                        // we're done
                        break;
                    }

                    // otherwise pop a node off the stack
                    current = nodes.Pop();

                    // print out the data
                    Console.Write(current.getData() + " ");

                    // iterate to the right
                    current = current.right;

                    continue;
                }

                // if the node doesn't have a left
                if (current.left == null)
                {
                    // print out the data
                    Console.Write(current.getData() + " ");

                    // iterate to the right
                    current = current.right;
                }
                // it has a left
                else
                {
                    // push the current node on the stack
                    // and iterate to the left
                    nodes.Push(current);
                    current = current.left;
                }
            }

            Console.Write("\n");
        }

        // Morris traversal
        // algorithm from http://www.geeksforgeeks.org/inorder-tree-traversal-without-recursion-and-without-stack/
        public void inOrderTraversalWithoutRecursionOrStack()
        {
            Console.WriteLine("Traversal of the tree with NO stack and NO recursion:");

            // declare a node to keep track of predecessors
            // and start at the root
            BSTNode pre = null;
            BSTNode current = this.root;

            while (current != null)
            {
                // if the current node doesn't have a left child
                if (current.left == null)
                {
                    // print out the data
                    Console.Write(current.getData() + " ");

                    // iterate to the right
                    current = current.right;
                }
                else
                {
                    // find the right child of the right-most node
                    // in current's left subtree
                    pre = current.left;

                    while ((pre.right != null) && (pre.right != current))
                    {
                        // iterate to the right as much as possible
                        pre = pre.right;
                    }

                    // at this point we should be at the right-most node
                    // in the left subtree of current
                    if (pre.right == null)
                    {
                        // make current the right child of pre
                        // and iterate to the left
                        pre.right = current;
                        current = current.left;
                    }
                    // undo the changes we made to the tree
                    else
                    {
                        pre.right = null;

                        // print out the data
                        Console.Write(current.getData() + " ");

                        // iterate to the right
                        current = current.right;
                    }
                }
            }

            Console.Write("\n");
        }

        public void runProgram()
        {
            Random random = new Random ();
            HashSet<int> usedIntegers = new HashSet<int>();
            var running = true;

            while (running)
            {
                // build a tree with 25 random integers between [0, 100]
                for (int i = 0; i < 25; i++)
                {
                    // generate a random number between [0, 100]
                    int randomInt = random.Next(0, 101);

                    // ensure no duplicates by adding to a hash
                    while (usedIntegers.Add(randomInt) == false)
                    {
                        // regenerate if there's a duplicate
                        randomInt = random.Next(0, 101);
                    }

                    // add the random number to the tree
                    this.insertNode(randomInt);
                }

                // 3. call the in-order traversal without using a stack
                // and without using recursion
                // display this first
                this.inOrderTraversalWithoutRecursionOrStack();

                // 2. call the in-order traversal using a stack but no recursion
                // display this second
                this.inOrderTraversalWithoutRecursion();

                // 1. call the in-order traversal with two recursive calls
                // display this last
                this.inOrderTraversal();

                // promt the user if they want to run the program again
                Console.WriteLine("Again (y/n)?");
                string input = Console.ReadLine();

                // user wants to run again
                if (input.Equals("y") || input.Equals("Y") || input.Equals("yes"))
                {
                    // clear the tree and hashset then continue
                    this.root = null;
                    usedIntegers.Clear();
                    continue;
                }
                // user doesn't want to run again
                else if (input.Equals("n") || input.Equals("N") || input.Equals("no"))
                {
                    // set the loop condition to false to break out of it
                    running = false;
                }
            }
        }

        static void Main(string[] args)
        {
            // define a BST for the user
            BST userBST = new BST();

            // run the program
            userBST.runProgram();
        }
    }
}