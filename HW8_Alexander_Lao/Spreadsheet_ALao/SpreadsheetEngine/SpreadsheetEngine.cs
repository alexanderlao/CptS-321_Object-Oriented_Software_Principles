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
        protected string m_name;
        protected uint m_color;

        // empty base class constructor
        public Cell()
        {
        }

        // cell constructor
        public Cell(int newRowIndex, int newColumnIndex)
        {
            this.rowIndex = newRowIndex;
            this.columnIndex = newColumnIndex;

            // convert the rowIndex and columnIndex into a cell name
            // i.e. (0, 0) -> A1
            this.m_name += Convert.ToChar('A' + newColumnIndex);        // ascii value of 'A' + column = corresponding letter
            this.m_name += (newRowIndex + 1).ToString();                // rows start at one in the user spreadsheet
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

        // Name property
        public string Name
        {
            get { return this.m_name; }
        }

        // BGColor property
        public uint BGColor
        {
            get { return this.m_color; }

            set
            {
                // if the color is being changed to the same
                // color then just ignore it
                if (m_color == value) return;

                // otherwise update m_color
                this.m_color = value;

                // and notify subscribers that the property changed
                PropertyChanged(this, new PropertyChangedEventArgs("Color"));
            }
        }
    }

    public class Spreadsheet
    {
        public event PropertyChangedEventHandler CellPropertyChanged = delegate { };

        private int m_rows;
        private int m_columns;
        private Cell[,] m_spreadsheet;
        private Dictionary<Cell, List<Cell>> m_dependencies;    // Cell | Referenced by...

        private Stack<MultiCmd> m_undos = new Stack<MultiCmd>();
        private Stack<MultiCmd> m_redos = new Stack<MultiCmd>();

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
            this.m_dependencies = new Dictionary<Cell, List<Cell>>();

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

        // overloaded GetCell that takes a cell name as the parameter
        public Cell GetCell(string cellName)
        {
            // the column is always the first character of the cellName
            // convert the char to an index number
            int cellColumn = cellName[0] - 'A';

            // the row is the rest of the cell name, parse the cell name
            // and convert it to an int, minus one because the underlying array starts at 0,0
            int cellRow = Convert.ToInt32(cellName.Substring(1)) - 1;

            // return the cell
            return GetCell(cellRow, cellColumn);
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

        // UndoEmpty property
        // checks if there's something on the undo stack
        public bool UndoEmpty
        {
            get { return this.m_undos.Count == 0; }
        }

        // RedoEmpty property
        // checks if there's something on the redo stack
        public bool RedoEmpty
        {
            get { return this.m_redos.Count == 0; }
        }

        // UndoCommand property
        public String UndoCommand
        {
            get
            {
                // check if there's something on the undo stack
                if (!UndoEmpty)
                {
                    // return the command name if it exists
                    return m_undos.Peek().CommandName;
                }

                // otherwise return an empty string if there's
                // nothing to undo
                return "";
            }
        }

        // RedoCommand property
        public String RedoCommand
        {
            get
            {
                // check if there's something on the redo stack
                if (!RedoEmpty)
                {
                    // return the command name since it exists
                    return m_redos.Peek().CommandName;
                }

                // otherwise return an empty string if there's
                // nothing to redo
                return "";
            }
        }

        private void UpdateCell (object sender, PropertyChangedEventArgs e)
        {
            // only change the cell's text if the Text property changed
            if ("Text" == e.PropertyName)
            {
                // get the cell that we need to update
                Cell cellToUpdate = sender as Cell;

                // call the overloaded method
                UpdateCell(cellToUpdate);
            }
            else if ("Color" == e.PropertyName)
            {
                // get the cell that we need to update
                Cell cellToUpdate = sender as Cell;

                // and notify subscribers that the cell property changed
                // need to pass the sender which is the cell
                // passing the this reference would pass the spreadsheet
                CellPropertyChanged(cellToUpdate, new PropertyChangedEventArgs("CellColorChanged"));
            }
        }

        // overloaded UpdateCell, takes a cell as the parameter
        private void UpdateCell(Cell cellToUpdate)
        {
            // remove old dependencies
            RemoveDependencies(cellToUpdate);

            // first check if the cell is empty
            // i.e. the user deleted the text from the cell
            if (string.IsNullOrEmpty(cellToUpdate.Text))
            {
                cellToUpdate.Value = "";
            }
            // if the text doesn't begin with an =
            else if (cellToUpdate.Text[0] != '=')
            {
                double cellValue;

                // if the cell contains a double
                if (double.TryParse(cellToUpdate.Text, out cellValue))
                {
                    // build the ExpTree to set the cell's variable
                    ExpTree userExpTree = new ExpTree(cellToUpdate.Text);
                    cellValue = userExpTree.Eval();
                    userExpTree.SetVar(cellToUpdate.Name, cellValue);

                    // update the value to the double
                    cellToUpdate.Value = cellValue.ToString();
                }
                else
                {
                    // otherwise the value is just set to the text
                    cellToUpdate.Value = cellToUpdate.Text;
                }
            }
            // need to evaluate an expression here
            else
            {
                // parse the text to get the expression
                string exp = cellToUpdate.Text.Substring(1);

                // build an ExpTree, eval to add variables
                ExpTree userExpTree = new ExpTree(exp);
                userExpTree.Eval();

                // get the variable names
                string[] varNames = userExpTree.GetVarNames();

                // loop through each variable name in the array
                foreach (string variable in varNames)
                {
                    // get the cell and add its value to the dictionary
                    double value = 0.0;
                    Cell relevantCell = GetCell(variable);

                    // try to parse out the double
                    double.TryParse(relevantCell.Value, out value);

                    // set the variable
                    userExpTree.SetVar(variable, value);
                }

                // set the value to the computed value of the ExpTree
                cellToUpdate.Value = userExpTree.Eval().ToString();

                // add the dependencies
                AddDependencies(cellToUpdate, varNames);
            }

            // if there are cells that depend on the one we just updated
            if (m_dependencies.ContainsKey(cellToUpdate))
            {
                // update all dependent cells
                UpdateDependencies(cellToUpdate);
            }

            // and notify subscribers that the cell property changed
            // need to pass the sender which is the cell
            // passing the this reference would pass the spreadsheet
            CellPropertyChanged(cellToUpdate, new PropertyChangedEventArgs("CellChanged"));
        }
        
        // sets a cell's value in the ExpTree's variable dictionary
        private void SetCellVariable(ExpTree userExpTree, string varName)
        {
            // get the cell based on the varName
            Cell relevantCell = GetCell(varName);

            double value;

            // check if the cell's value is a double
            if (double.TryParse(relevantCell.Value, out value))
            {
                // if it's a double, set its variable to that double value
                userExpTree.SetVar(varName, value);
            }
            else
            {
                // otherwise set its variable to 0.0
                userExpTree.SetVar(varName, 0.0);
            }
        }

        // adds dependencies for a cell
        private void AddDependencies(Cell refCell, string[] independents)
        {
            foreach (string indCell in independents)
            {
                // get the independent cell
                Cell independentCell = GetCell(indCell);

                // if the dictionary does not contain the independentCell key
                if (!m_dependencies.ContainsKey(independentCell))
                {
                    // create a new list if the independentCell is a new key
                    m_dependencies[independentCell] = new List<Cell>();
                }

                // add the referenced cell to the independent cell's references
                m_dependencies[independentCell].Add(refCell);
            }
        }

        // removes dependencies for a cell
        private void RemoveDependencies(Cell referencedCell)
        {
            // loop through each list of dependent cells
            foreach (List<Cell> depCells in m_dependencies.Values)
            {
                // if the dependent list contains the referenced cell
                if (depCells.Contains(referencedCell))
                {
                    // remove it
                    depCells.Remove(referencedCell);
                }
            }
        }

        // updates all dependent cells
        private void UpdateDependencies(Cell indepCell)
        {
            // loop through each list of dependent cells
            foreach (Cell depCell in m_dependencies[indepCell].ToArray())
            {
                UpdateCell(depCell);
            }
        }

        // adds commands onto the undo stack
        public void AddUndo(MultiCmd undos)
        {
            // push the undos onto the undo stack
            this.m_undos.Push(undos);
        }

        // performs an undo
        public void Undo()
        {
            // check if the undo stack is empty
            if (!this.UndoEmpty)
            {
                // pop the commands off of the undo stack
                MultiCmd commands = this.m_undos.Pop();

                // push the commands onto the redo stack
                this.m_redos.Push(commands.Exec());
            }
        }

        public void Redo()
        {
            // check if the redo stack is empty
            if (!this.RedoEmpty)
            {
                // pop the commands off of the redo stack
                MultiCmd commands = this.m_redos.Pop();

                // push the commands onto the undo stack
                this.m_undos.Push(commands.Exec());
            }
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
        private static Dictionary<string, double> m_vars = new Dictionary<string, double>();

        // constructor
        public ExpTree(string exp)
        {
            // call the recursive Compile() method
            this.m_root = Compile(exp);

            m_vars.Clear();
        }

        // sets the specified variable within
        // the ExpTree variables dictionary
        public void SetVar(string varName, double varValue)
        {
            // add the varName and its value to the dictionary
            m_vars[varName] = varValue;
        }

        // returns a list of all the variable names
        public string[] GetVarNames()
        {
            return m_vars.Keys.ToArray();
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

    // =============== Homework 8 Code Below ===============

    public interface ICmd
    {
        ICmd Exec();
    }

    public class RestoreText : ICmd
    {
        private Cell m_cell;
        private string m_text;

        public RestoreText(Cell c, string t)
        {
            m_cell = c;
            m_text = t;
        }

        public ICmd Exec()
        {
            // build the inverse of the cell
            var inverse = new RestoreText(m_cell, m_cell.Text);

            // set the text
            m_cell.Text = m_text;

            // return the inverse
            return inverse;
        }
    }

    public class RestoreColor : ICmd
    {
        private Cell m_cell;
        private uint m_color;

        public RestoreColor(Cell c, uint u)
        {
            m_cell = c;
            m_color = u;
        }

        public ICmd Exec()
        {
            // build the inverse of the cell
            var inverse = new RestoreColor(m_cell, m_cell.BGColor);

            // set the color
            m_cell.BGColor = m_color;

            // return the inverse
            return inverse;
        }
    }

    public class MultiCmd
    {
        private ICmd[] m_cmds;
        private string m_commandName;

        public MultiCmd()
        {
            // empty constructor
        }

        public MultiCmd(ICmd[] multiCmds, string commandName)
        {
            this.m_cmds = multiCmds;
            this.m_commandName = commandName;
        }

        public string CommandName
        {
            get { return this.m_commandName; }
        }

        public MultiCmd Exec()
        {
            // initialize a list to hold the commands
            List<ICmd> cmdList = new List<ICmd>();

            // loop through the array of commands
            foreach (ICmd cmd in this.m_cmds)
            {
                // add the cmd to the list
                cmdList.Add(cmd.Exec());
            }

            // return the inverse array
            return new MultiCmd (cmdList.ToArray(), this.m_commandName);
        }
    }
}