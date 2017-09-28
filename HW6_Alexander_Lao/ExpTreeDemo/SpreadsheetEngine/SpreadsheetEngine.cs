// Alexander Lao
// 11481444

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace CptS321
{
    public abstract class Cell : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        protected int rowIndex;
        protected int columnIndex;
        protected string m_text;
        protected string m_value;

        // empty base class constructor
        public Cell()
        {
        }

        // cell constructor
        public Cell(int newRowIndex, int newColumnIndex)
        {
            this.rowIndex = newRowIndex;
            this.columnIndex = newColumnIndex;
        }

        // RowIndex read-only property
        public int RowIndex
        {
            get { return this.rowIndex; }
        }

        // RowColumn read-only property
        public int ColumnIndex
        {
            get { return this.columnIndex; }
        }

        // Text property
        public string Text
        {
            get { return this.m_text; }

            set
            {
                // if the text is being changed to the same
                // text then just ignore it
                if (m_text == value) return;

                // otherwise update m_text
                this.m_text = value;

                // and notify subscribers that the property changed
                PropertyChanged(this, new PropertyChangedEventArgs("Text"));
            }
        }
        
        // Value property
        public string Value
        {
            get { return this.m_value; }

            // https://msdn.microsoft.com/en-us/library/75e8y5dd.aspx
            // protect the value from being set by anything other
            // than the spreadsheet class
            protected internal set
            {
                // if the value is being changed to the same
                // value then just ignore it
                if (m_value == value) return;

                // otherwise update m_value
                this.m_value = value;

                // and notify subscribers that the property changed
                PropertyChanged(this, new PropertyChangedEventArgs("Value"));
            }
        }
    }

    public class Spreadsheet
    {
        public event PropertyChangedEventHandler CellPropertyChanged = delegate { };

        private int m_rows;
        private int m_columns;
        private Cell[,] m_spreadsheet;

        // inner class used to instaniate an instance of a cell
        private class InstanceCell : Cell
        {
            // call cell's constructor
            public InstanceCell(int newRow, int newColumn) : base(newRow, newColumn)
            {
            }
        }

        public Spreadsheet(int newRows, int newColumns)
        {
            this.m_rows = newRows;
            this.m_columns = newColumns;

            // initialize the 2D array
            this.m_spreadsheet = new Cell[newRows, newColumns];

            // loop through each cell
            for (int i = 0; i < newRows; i++)
            {
                for (int j = 0; j < newColumns; j++)
                {
                    // instantiate an InstanceCell
                    Cell newCell = new InstanceCell(i, j);

                    // the spreadsheet class needs to subscribe to every cell
                    newCell.PropertyChanged += UpdateCell;

                    // add that cell to the spreadsheet
                    m_spreadsheet[i, j] = newCell;
                }
            }
        }

        // return the cell at the specified location
        public Cell GetCell(int row, int column)
        {
            return m_spreadsheet[row, column];
        }

        // RowCount property
        public int RowCount
        {
            get { return this.m_rows; }
        }

        // ColumnCount property
        public int ColumnCount
        {
            get { return this.m_columns; }
        }

        private void UpdateCell (object sender, PropertyChangedEventArgs e)
        {
            // only change the cell's text if the Text property changed
            if ("Text" == e.PropertyName)
            {
                // get the cell that we need to update
                Cell cellToUpdate = sender as Cell;

                // if the text doesn't begin with an =
                if (cellToUpdate.Text[0] != '=')
                {
                    // then the value is just set to the text
                    cellToUpdate.Value = cellToUpdate.Text;
                }
                else
                {
                    // the column should always be at index 1
                    char copyColumn = cellToUpdate.Text[1];
                    int copyColumnNumber = copyColumn - 65;

                    // the row is the rest
                    string copyRow = cellToUpdate.Text.Substring(2);
                    int copyRowNumber = Int32.Parse(copyRow);

                    // retrieve the value from the other cell
                    string copyValue = this.m_spreadsheet[copyRowNumber, copyColumnNumber].Value;

                    // set the value to the value from the other cell
                    cellToUpdate.Value = copyValue;
                }
            }

            // and notify subscribers that the cell property changed
            // need to pass the sender which is the cell
            // passing the this reference would pass the spreadsheet
            CellPropertyChanged(sender, new PropertyChangedEventArgs("CellChanged"));
        }
    }

    // =============== Homework 5 and 6 Code Below ===============
    // // NOTE: Most of this code was adapted from the code we did in class.

    public class ExpTree
    {
        // abstract base class for every type of node
        private abstract class Node
        {
            public abstract double Eval();
        }

        private class ConstNode : Node
        {
            private double m_value;

            // constructor
            public ConstNode(double newValue)
            {
                this.m_value = newValue;
            }

            // override the abstract function in the base class
            public override double Eval()
            {
                return m_value;
            }
        }

        private class OpNode : Node
        {
            private char m_op;
            private Node m_left, m_right;

            // constructor
            public OpNode(char newOp, Node newLeft, Node newRight)
            {
                this.m_op = newOp;
                this.m_left = newLeft;
                this.m_right = newRight;
            }

            // override the abstract function in the base class
            public override double Eval()
            {
                // evaluate the left and right nodes
                double left = m_left.Eval();
                double right = m_right.Eval();

                // call the correct operation on those nodes
                switch (m_op)
                {
                    case '+': return left + right;
                    case '-': return left - right;
                    case '*': return left * right;
                    case '/': return left / right;
                    default: return double.NaN;
                }
            }
        }

        private class VarNode : Node
        {
            private string m_varName;

            // constructor
            public VarNode(string newVarName)
            {
                this.m_varName = newVarName;
            }

            // override the abstract function in the base class
            public override double Eval()
            {
                // if the variable is not in the dictionary, add it and give it the value of 0
                if (!m_vars.ContainsKey(m_varName)) m_vars[m_varName] = 0.0;
                
                return m_vars[m_varName];
            }
        }

        private Node m_root;
        private static Dictionary<string, double> m_vars;

        // constructor
        public ExpTree(string exp)
        {
            // clear the old dictionary
            m_vars = new Dictionary<string, double>();

            // call the recursive Compile() method
            this.m_root = Compile(exp);
        }

        // sets the specified variable within
        // the ExpTree variables dictionary
        public void SetVar(string varName, double varValue)
        {
            // add the varName and its value to the dictionary
            m_vars[varName] = varValue;
        }

        // evaluates the expression to a double value
        public double Eval()
        {
            if (m_root != null) return m_root.Eval();
            else return double.NaN;
        }

        // builds a ConstNode or VarNode based on the term
        private static Node BuildSimple(string term)
        {
            double num;

            // if the string can be converted to a double
            if (double.TryParse(term, out num))
            {
                // instantiate and return a new ConstNode
                return new ConstNode(num);
            }

            // otherwise instantiate and return a new VarNode
            return new VarNode(term);
        }

        private static Node Compile(string exp)
        {
            // remove all spaces from the exp
            exp = exp.Replace(" ", "");

            // check if the entire exp is enclosed in parenthesis
            bool truth = CheckForEnclosedParenthesis(exp);

            // keep checking for enclosed parenthesis
            while (truth == true)
            {
                // parse out the enclosing parenthesis
                exp = exp.Substring(1, exp.Length - 2);
                truth = CheckForEnclosedParenthesis(exp);
            }

            // get the low op index
            int index = GetLowOpIndex(exp);

            if (index != -1)
            {
                return new OpNode(exp[index],
                                   Compile(exp.Substring(0, index)),
                                   Compile(exp.Substring(index + 1)));
            }

            // the last node shouldn't be an OpNode
            return BuildSimple(exp);
        }

        // finds the index of the lowest precedence operator
        private static int GetLowOpIndex(string exp)
        {
            int parenCounter = 0, index = -1;

            // loop through the entire expression
            for (int i = exp.Length - 1; i >= 0; i--)
            {
                switch (exp[i])
                {
                    case ')':
                        parenCounter--;
                        break;

                    case '(':
                        parenCounter++;
                        break;

                    // if we see a + or - without anything
                    // else in the exp inside parenthesis
                    // return its index
                    case '+':
                    case '-':
                        if (parenCounter == 0) return i;
                        break;

                    // if we see a * or / inside parenthesis
                    // return this index
                    case '*':
                    case '/':
                        if (parenCounter == 0 && index == -1) index = i;
                        break;
                }
            }

            return index;
        }

        private static bool CheckForEnclosedParenthesis(string exp)
        {
            int parenCounter = 0;

            // check if the first and last character is
            // open and close parenthesis, respectively
            if ((exp[0] == '(') && (exp[exp.Length - 1] == ')'))
            {
                // check if they're matching parenthesis
                for (int i = 1; i < exp.Length - 1; i++)
                {
                    switch (exp[i])
                    {
                        case ')':
                            if (parenCounter == 0) return false;        
                            parenCounter--;
                            break;

                        case '(':
                            parenCounter++;
                            break;

                        default: 
                            break;
                    }
                }

                if (parenCounter == 0) return true;
            }

            return false;
        }
    }
}