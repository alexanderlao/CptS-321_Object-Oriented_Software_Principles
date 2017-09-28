// Alexander Lao
// 11481444

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Numerics;

namespace HW3_Alexander_Lao
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // loads text and writes it to the TextBox
        private void LoadText(TextReader sr)
        {
            // read all of the text through the TextReader
            // and display its content to the outputBox
            outputBox.Text = sr.ReadToEnd();
        }

        // opens a file and passes it on to LoadText()
        private void LoadFile(string fileName)
        {
            // instantiate a stream reader to read from a file
            StreamReader sr = new StreamReader(fileName);

            // pass the stream reader to the LoadText() function
            // to write its content to the text box
            LoadText(sr);
        }

        // if the user clicks on the "Load from file" option
        private void loadFromFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // instantiate a OpenFileDialog to assist with loading
            OpenFileDialog openFileDialogBox = new OpenFileDialog();

            // if the user hits "Ok"
            if (openFileDialogBox.ShowDialog() == DialogResult.OK)
            {
                // load the user's file using the LoadFile method
                LoadFile(openFileDialogBox.FileName);
            }
        }

        // if the user clicks on the "Load Fibonacci numbers (first 50)" option
        private void loadFibonacciNumbersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // instantiate a new FibonacciTextReader object with 50 lines
            FibonacciTextReader firstFifty = new FibonacciTextReader(50);

            // load its text
            LoadText(firstFifty);
        }

        // if the user clicks on the "Load Fibonacci numbers (first 100)" option
        private void loadFibonacciNumbersfirst100ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // instantiate a new FibonacciTextReader object with 100 lines
            FibonacciTextReader firstHundred = new FibonacciTextReader(100);

            // load its text
            LoadText(firstHundred);
        }

        // if the user clicks on the "Save to file" option
        // https://msdn.microsoft.com/en-us/library/system.windows.forms.savefiledialog(v=vs.110).aspx
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // instantiate a SaveFileDialog to assist with saving
            SaveFileDialog saveFileDialogBox = new SaveFileDialog();

            saveFileDialogBox.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialogBox.FilterIndex = 2;
            saveFileDialogBox.RestoreDirectory = true;

            // if the user hits "Ok"
            if (saveFileDialogBox.ShowDialog() == DialogResult.OK)
            {
                // open the outfile based on the user's file name
                using (StreamWriter outfile = new StreamWriter(saveFileDialogBox.FileName))
                {
                    // write to the file
                    outfile.Write(outputBox.Text);
                }
            }
        }
    }

    public class FibonacciTextReader : TextReader
    {
        // data members
        private int maximumLines;
        private int counter;               // used to keep track of which fibonacci number we need to calculate
        private BigInteger minusOne;       // used to keep track of the n - 1 number in the fibonacci sequence
        private BigInteger minusTwo;       // used to keep track of the n - 2 number in the fibonacci sequence

        // constructor
        public FibonacciTextReader(int newMaximumLines)
        {
            // initialize values
            this.maximumLines = newMaximumLines;
            this.counter = 0;
            this.minusOne = 0;
            this.minusTwo = 1;
        }

        // returns the next fibonacci number
        public override string ReadLine()
        {
            // once we hit the maximumLines' fibonacci number
            // return null
            if (counter == maximumLines) return null;
            else
            {
                // special cases for the first and second fibonacci numbers
                if (counter == 0) 
                {
                    // increment the counter and return the first fibonacci number
                    counter++;
                    return "0";
                }
                else if (counter == 1)
                {
                    // increment the counter and return the second fibonacci number
                    counter++;
                    return "1";
                } 

                // increment the counter
                counter++;

                // calculate the next fibonacci number
                BigInteger nextFibonacci = BigInteger.Add(this.minusOne, this.minusTwo);

                // update the previous numbers in the sequence
                this.minusOne = this.minusTwo;
                this.minusTwo = nextFibonacci;

                // return the next fibonacci number as a string
                return nextFibonacci.ToString();
            }
        }

        // continually calls the ReadLine() method to calculate
        // the fibonacci sequence then appends it to a result string
        public override string ReadToEnd()
        {
            // instantiate a StringBuilder object to hold the result
            StringBuilder result = new StringBuilder();

            // calculate the sequence of fibonacci numbers
            // up to the maximumLines
            for (int i = 1; i <= maximumLines; i++)
            {
                // append the fibonacci number to the result
                result.Append(i + ": " + ReadLine() + "\r\n");
            }

            return result.ToString();
        }
    }
}