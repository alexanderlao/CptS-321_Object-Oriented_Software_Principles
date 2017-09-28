// Alexander Lao
// 11481444

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW1_Alexander_Lao
{
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
            Console.Write("Tree contents: ");

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

        // calls the private countNodes function
        public void countNodes(ref int count)
        {
            countNodes(this.root, ref count);
        }

        // private countNodes function to hide the root
        // counts the number of nodes in a BST using an in-order approach
        private void countNodes(BSTNode currentNode, ref int count)
        {
            if (currentNode != null)
            {
                // recursive call to the left
                countNodes(currentNode.getLeft(), ref count);

                // count the node we just visited
                count++;

                // recursive call to the right
                countNodes(currentNode.getRight(), ref count);
            }
        }

        // calls the private countLevels function
        public int countLevels()
        {
            return countLevels(this.root);
        }

        // returns the number of levels in a BST
        private int countLevels(BSTNode currentNode)
        {
            // base case is when we hit a null node
            if (currentNode == null)
            {
                return 0;
            }

            // count the height of the left subtree
            int leftHeight = countLevels(currentNode.left);

            // count the height of the right subtree
            int rightHeight = countLevels(currentNode.right);

            // return the greater subheight
            return Math.Max(leftHeight, rightHeight) + 1;
        }

        // determines the theoretical minimum number of levels 
        // that the tree could have given the number of nodes it contains
        public int calculateMinimumLevels(int givenNodes)
        {
            // convert the int to a double
            // https://msdn.microsoft.com/en-us/library/ayes1wa5(v=vs.110).aspx
            double toDouble = System.Convert.ToDouble(givenNodes);
            
            // calculate the minimum number of levels 
            // given n number of nodes using the formula log_2(n) + 1
            double result = Math.Log(toDouble, 2.0);

            // round the result down
            double roundedResult = Math.Floor(result) + 1;

            // convert the double to an int
            return System.Convert.ToInt32(roundedResult);
        }

        // the main function of the program
        // prompts the user for the BST then calculates
        // the statistics of the user generated BST
        public void runProgram()
        {
            // prompt the user for input
            Console.WriteLine("Enter a collection of numbers in the range [0, 100], separated by spaces:");

            string userInput = Console.ReadLine();          // string to hold the user's input
            string[] userNumbers = userInput.Split(' ');    // string array to hold the user's numbers after the split

            int userCount = 0, userLevels = 0;              // int to hold the number of nodes and number of levels for the userBST
            int minimumLevels = 0;                          // into to hold the minimum number of levels of the BST

            // loop through each number in userNumber
            foreach (string number in userNumbers)
            {
                // convert the string number to its integer value
                // https://msdn.microsoft.com/en-us/library/bb397679.aspx
                int intValue = Int32.Parse(number);

                // add that number to the userBST
                insertNode(intValue);
            }

            // print out the userBST in order
            inOrderTraversal();

            Console.WriteLine("Tree statistics:");

            countNodes(ref userCount);                                              // count the number of nodes in the BST
            Console.WriteLine("\tNumber of nodes: " + userCount);                   // print out the number of nodes

            userLevels = countLevels();                                             // count the number of levels in the BST
            Console.WriteLine("\tNumber of levels: " + userLevels);                 // print out the number of levels

            minimumLevels = calculateMinimumLevels(userCount);                      // calculate the minimum numbers of levels in the BST
            Console.WriteLine("\tMinimum number of levels that a tree with " +
                               userCount + " nodes could have = " + minimumLevels); // print out the minimum number of levels

            Console.WriteLine("Done");
        }

        static void Main(string[] args)
        {
            // define a BST for the user
            BST userBST = new BST();

            // run the program
            userBST.runProgram();
        }
    }

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
}
