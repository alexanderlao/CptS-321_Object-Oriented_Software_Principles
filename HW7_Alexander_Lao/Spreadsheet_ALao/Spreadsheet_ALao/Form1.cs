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
using CptS321;

namespace Spreadsheet_ALao
{
    public partial class Form1 : Form
    {
        private Spreadsheet m_spreadsheet;

        public Form1()
        {
            InitializeComponent();

            // initialize the spreadsheet with 50 rows and 26 columns
            this.m_spreadsheet = new Spreadsheet(50, 26);

            // subscribe to the property changed event
            m_spreadsheet.CellPropertyChanged += UpdateForm;
            dataGridView1.CellBeginEdit += dataGridView1_CellBeginEdit;
            dataGridView1.CellEndEdit += dataGridView1_CellEndEdit;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Alexander Lao - 11481444 - HW4";

            button1.Text = "Do spreadsheet modification demo where " +
                           "changes in engine trigger UI updates";

            // ascii value of A is 65, loop starts at 1
            int ascii = 64;

            // generate columns A-Z
            for (int i = 1; i <= 26; i++)
            {
                // generate each letter
                char letter = (char)(ascii + i);
                string stringLetter = letter.ToString();

                // add each column (columnName, headerText)
                dataGridView1.Columns.Add("column" + i, stringLetter);
            }

            // generate rows 1-50
            dataGridView1.Rows.Add(50);

            // give each row a name
            for (int i = 0; i < 50; i++)
            {
                var row = dataGridView1.Rows[i];
                row.HeaderCell.Value = (i + 1).ToString();
            }
        }

        private void UpdateForm(object sender, PropertyChangedEventArgs e)
        {
            // only change the form if the cell's text changed
            if ("CellChanged" == e.PropertyName)
            {
                // get the cell that we need to update
                Cell cellToUpdate = sender as Cell;

                if (cellToUpdate != null)
                {
                    // find its row and column
                    int cellRow = cellToUpdate.RowIndex;
                    int cellColumn = cellToUpdate.ColumnIndex;

                    // update that cell in the form
                    dataGridView1.Rows[cellRow].Cells[cellColumn].Value = cellToUpdate.Value;
                }
            }
        }

        // fire when the user starts editing the cell
        private void dataGridView1_CellBeginEdit(object sender,
                                                 DataGridViewCellCancelEventArgs e)
        {
            // get the row and column of the cell that we need to update
            int cellRow = e.RowIndex;
            int cellColumn = e.ColumnIndex;

            // get the actual cell
            Cell cellToUpdate = m_spreadsheet.GetCell(cellRow, cellColumn);

            if (cellToUpdate != null)
            {
                // update that cell in the spreadsheet to display its Text property
                dataGridView1.Rows[cellRow].Cells[cellColumn].Value = cellToUpdate.Text;
            }
        }

        // fire when the user stops editing the cell
        private void dataGridView1_CellEndEdit(object sender,
                                               DataGridViewCellEventArgs e)
        {
            // get the row and column of the cell that we need to update
            int cellRow = e.RowIndex;
            int cellColumn = e.ColumnIndex;

            // get the actual cell
            Cell cellToUpdate = m_spreadsheet.GetCell(cellRow, cellColumn);

            if (cellToUpdate != null)
            {
                // check to see if the user deleted the text of a cell
                try
                {
                    // update the Text property of the cell to notify subscribers
                    cellToUpdate.Text = dataGridView1.Rows[cellRow].Cells[cellColumn].Value.ToString();
                }
                catch (NullReferenceException)
                {
                    cellToUpdate.Text = "";
                }
                
                // update that cell in the spreadsheet to display its Value property
                dataGridView1.Rows[cellRow].Cells[cellColumn].Value = cellToUpdate.Value;
            }
        }

        // user pressed the demo button
        private void button1_Click(object sender, EventArgs e)
        {
            // instantiate a random object
            Random random = new Random();

            // modifying 50 random cells to display a string
            for (int i = 0; i < 50; i++)
            {
                // create a random row 0-25
                // and random column 0-49
                int randomRow = random.Next(0, 50);
                int randomColumn = random.Next(0, 26);

                // set that random cell's value to a string
                Cell currentCell = this.m_spreadsheet.GetCell(randomRow, randomColumn);
                currentCell.Text = "Hello World!";
            }

            // modifying every cell in column B
            for (int i = 0; i < 50; i++)
            {
                // set every cell in column B (column 1) to a string
                Cell currentCell = this.m_spreadsheet.GetCell(i, 1);
                currentCell.Text = "This is cell B" + (i + 1);
            }

            // modifying every cell in column A to match column B
            for (int i = 0; i < 50; i++)
            {
                // set every cell in column A (column 0) to column B's value in the same row
                Cell currentCell = this.m_spreadsheet.GetCell(i, 0);
                currentCell.Text = "=B" + (i + 1);
            }
        }
    }
}