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
}