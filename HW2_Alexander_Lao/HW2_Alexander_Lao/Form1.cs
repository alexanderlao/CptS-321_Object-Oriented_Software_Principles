using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HW2_Alexander_Lao
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // instantiate the list to hold the random numbers
            List<int> randomList = new List<int>();

            // instantiate a random object
            Random random = new Random();

            // do this 10,000 times
            for (int i = 0; i < 10000; i++)
            {
                // add a random number between [0, 20,001) to the list
                randomList.Add(random.Next(0, 20001));
            }

            // initialize a string to hold the output
            String outputString;

            // ============================ 1. ============================

            // instantiate the hash set to hold the
            // numbers without repeats
            HashSet<int> myHash = new HashSet<int>();

            // loop through each number in the random list
            foreach (int number in randomList)
            {
                // add the unique number to the hash set
                myHash.Add(number);
            }

            // add the output and time complexity analysis to the outputString
            // https://msdn.microsoft.com/en-us/library/bb353005(v=vs.110).aspx
            outputString = "1. HashSet method: " + myHash.Count + " unique numbers\r\n\r\n" +
                            "This method has an O(1) time complexity on average. When adding " +
                            "a new element to the HashSet, if the count of the HashSet " +
                            "is less than the capacity of its internal array it is an O(1) operation. " +
                            "However, if the HashSet has to be resized, meaning its internal array is full, " +
                            "it becomes an O(n) operation where n is equal to the count of the HashSet.\r\n\r\n";

            // ============================ 2. ============================

            // this algorithm starts with two pointers i and j.
            // i starts at the beginning of the list and j
            // starts at i + 1 (i and j are always next to each other at the beginning).
            // the outer for-loop iterates through each number in the list once.
            // also, inside the outer loop we first increment the count for a potential
            // unique number. the inner loop checks the potential number against every other
            // number in the list. as soon as we find a duplicate, we decrement the count to
            // undo to increment for a potential unique number and move on to check the next number.
            // if no duplicate was found, we do not undo the count increment because we found
            // a unique number.

            // initialize a variable to keep track
            // of the unique integers
            int count = 0;

            // loop through each number in the list
            for (int i = 0; i < randomList.Count(); i++)
            {
                // increment the count by 1 at the beginning
                // for a potential unique number
                count++;

                // check the i number against every other number j
                for (int j = i + 1; j < randomList.Count(); j++)
                {
                    // as soon as we find a duplicate
                    if (randomList[i] == randomList[j])
                    {
                        // undo the increment done earlier
                        // and check the next i number
                        count--;
                        break;
                    }
                }
            }

            // append the result to the outputString
            outputString += "2. O(1) storage method: " + count + " unique numbers\r\n\r\n";

            // ============================ 3. ============================

            // since the list is sorted, all we have to do is look for
            // sequential numbers that are not equal. for example, if we had a list
            // that consisted of 1 1 1 1 1 1 2 2 2 2 2, we would need to loop through until we get
            // to index i = the number 1 and index i + 1 = the number 2. this can all be done
            // with one pass through the list which makes this an O(n) time complexity since
            // all other operations (such as checking for equal numbers) happens in O(1) time

            // sort the list
            randomList.Sort();

            // initialize a variable to keep track
            // of the unique integers
            int countTwo = 0;

            // loop through the entire list
            for (int i = 0; i < randomList.Count(); i++)
            {
                // check if we're at the end of the list
                if (i == randomList.Count() - 1)
                {
                    // increment the count for the last unique number
                    // and break out of the loop
                    countTwo++;
                    break;
                }

                // check the current number against its successor.
                // the previous if-statement protects against
                // checking against an out-of-bounds index
                // (i.e. at the end of the list)
                if (randomList[i] == randomList[i + 1])
                {
                    // if they're the same number continue
                    continue;
                }
                // otherwise increment the count
                else countTwo++;
            }

            // append the result to the outputString
            outputString += "3. Sorted method: " + countTwo + " unique numbers";

            // write the outputString to the output TextBox
            outputBox.Text = outputString;
        }
    }
}