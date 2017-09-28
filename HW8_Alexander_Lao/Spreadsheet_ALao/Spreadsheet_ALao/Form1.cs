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

            // update the edit menu options to display correctly
            UpdateEditMenu();
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
            else if ("CellColorChanged" == e.PropertyName)
            {
                // get the cell that we need to update
                Cell cellToUpdate = sender as Cell;

                if (cellToUpdate != null)
                {
                    // find its row and column
                    int cellRow = cellToUpdate.RowIndex;
                    int cellColumn = cellToUpdate.ColumnIndex;

                    // get the color from the cell
                    int intColor = (int)cellToUpdate.BGColor;
                    Color color = Color.FromArgb (intColor);

                    // update that cell in the form
                    dataGridView1.Rows[cellRow].Cells[cellColumn].Style.BackColor = color;
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

            // boolean to check whether the cell's text
            // was actually changed (i.e. the user clicked
            // into the cell then clicked out without
            // changing anything
            bool checkEdit = true;

            // create a RestoreText ICmd for the text change
            RestoreText[] undoText = new RestoreText[1];

            // store the old text of the cell for a potential undo
            string oldText = cellToUpdate.Text;

            // instantiate the RestoreText with the oldText
            undoText[0] = new RestoreText(cellToUpdate, oldText);

            if (cellToUpdate != null)
            {
                // check to see if the user deleted the text of a cell
                try
                {
                    // if the cell's text didn't change but there was text in the cell
                    if (cellToUpdate.Text == dataGridView1.Rows[cellRow].Cells[cellColumn].Value.ToString()) 
                        checkEdit = false;

                    // update the Text property of the cell to notify subscribers
                    cellToUpdate.Text = dataGridView1.Rows[cellRow].Cells[cellColumn].Value.ToString();
                }
                catch (NullReferenceException)
                {
                    // if the cell didn't have text before and after the edit
                    if (cellToUpdate.Text == null) checkEdit = false;

                    cellToUpdate.Text = "";
                }
                
                // update that cell in the spreadsheet to display its Value property
                dataGridView1.Rows[cellRow].Cells[cellColumn].Value = cellToUpdate.Value;

                // only add an undo if the cell was actually edited
                if (checkEdit == true)
                {
                    // add the text change to the undo stack
                    m_spreadsheet.AddUndo(new MultiCmd(undoText, "cell text change"));

                    // update the edit menu options to display correctly
                    UpdateEditMenu();
                }
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

        // the user clicked on the change background color menu option
        private void chooseBackgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // instantiate a ColorDialog to assist with picking a color
            ColorDialog colorDialog = new ColorDialog();

            // instaniate a list of ICmds for multiple color changes
            List<RestoreColor> undoColors = new List<RestoreColor>();

            // if the user clicks "OK"
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                // loop through all selected cells on the form
                foreach (DataGridViewCell cell in dataGridView1.SelectedCells)
                {
                    // convert the form cell to a CptS321 cell
                    Cell cellToUpdate = m_spreadsheet.GetCell(cell.RowIndex, cell.ColumnIndex);

                    // save a copy of the cell's old color for a potential undo
                    uint oldColor = cellToUpdate.BGColor;

                    // if the old color was initially 0, set it to white
                    if (oldColor == 0) oldColor = (uint)Color.White.ToArgb();
                   
                    // update the cell's background color
                    cellToUpdate.BGColor = (uint)colorDialog.Color.ToArgb();

                    // add the old color to the list of undoColors
                    RestoreColor undoColor = new RestoreColor(cellToUpdate, oldColor);
                    undoColors.Add(undoColor);
                }
            }

            // add all of the color changes to the undo stack
            m_spreadsheet.AddUndo(new MultiCmd(undoColors.ToArray(),
                                               "changing cell background color"));

            // update the edit menu options to display correctly
            UpdateEditMenu();
        }

        // the user clicked on the undo menu option
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // call the spreadsheet's undo function
            // and update the edit menu
            m_spreadsheet.Undo();
            UpdateEditMenu();
        }

        // the user clicked on the redo menu option
        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // call the spreadsheet's redo function
            // and update the edit menu
            m_spreadsheet.Redo();
            UpdateEditMenu();
        }

        // 
        private void UpdateEditMenu()
        {
            // retrieve the menu items for "Edit"
            ToolStripMenuItem editMenuItems = menuStrip1.Items[0] as ToolStripMenuItem;

            // loop through the drop down options (Undo and Redo)
            foreach (ToolStripMenuItem menuItem in editMenuItems.DropDownItems)
            {
                // check if it's undo
                // get the first four characters because we
                // append the command name to the menuItem text
                if (menuItem.Text.Substring(0, 4) == "Undo")
                {
                    // the undo option being enabled is dependent
                    // on the undo stack
                    menuItem.Enabled = !(this.m_spreadsheet.UndoEmpty);

                    // update the menuItem text to display the correct
                    // possible undo command
                    menuItem.Text = "Undo " + this.m_spreadsheet.UndoCommand;
                }
                // check if it's redo
                else if (menuItem.Text.Substring(0, 4) == "Redo")
                {
                    // the redo option being enabled is dependent
                    // on the redo stack
                    menuItem.Enabled = !(this.m_spreadsheet.RedoEmpty);

                    // update the menuItem text to display the correct
                    // possible redo command
                    menuItem.Text = "Redo " + this.m_spreadsheet.RedoCommand;
                }
            }
        }
    }
}