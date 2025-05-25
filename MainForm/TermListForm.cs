using System;
using System.Windows.Forms;
using TerminologyApp.Models;

namespace TerminologyApp.Forms
{
    public partial class TermListForm : Form
    {
        private DataGridView termsTable;
        private Button deleteButton;
        private ContextMenuStrip rightClickMenu;
        private TermListHandler handler;

        public TermListForm()
        {
            handler = new TermListHandler();
            SetupControls();
            handler.LoadTerms(termsTable);
        }

        private void SetupControls()
        {
            this.Text = "Term List";
            this.Size = new System.Drawing.Size(700, 500);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            termsTable = new DataGridView
            {
                Location = new System.Drawing.Point(10, 40),
                Size = new System.Drawing.Size(660, 400),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false
            };
            termsTable.Columns.Add("Назва", "Назва");
            termsTable.Columns.Add("Категорія", "Категорія");
            termsTable.Columns.Add("Короткий опис", "Короткий опис");
            termsTable.CellDoubleClick += TermsTable_CellDoubleClick;

            deleteButton = new Button
            {
                Text = "Видалити",
                Location = new System.Drawing.Point(10, 10),
                Width = 100
            };
            deleteButton.Click += DeleteButton_Click;

            rightClickMenu = new ContextMenuStrip();
            ToolStripMenuItem deleteMenuItem = new ToolStripMenuItem("Видалити");
            deleteMenuItem.Click += DeleteMenuItem_Click;
            rightClickMenu.Items.Add(deleteMenuItem);
            termsTable.ContextMenuStrip = rightClickMenu;

            this.Controls.AddRange(new Control[] { termsTable, deleteButton });
        }

        private void TermsTable_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            handler.ShowTermDetails(termsTable, e.RowIndex);
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (termsTable.SelectedRows.Count > 0)
            {
                string termName = termsTable.SelectedRows[0].Cells["Назва"].Value.ToString();
                handler.DeleteTerm(termName, termsTable);
            }
            else
            {
                MessageBox.Show("Виберіть термін для видалення.");
            }
        }

        private void DeleteMenuItem_Click(object sender, EventArgs e)
        {
            if (termsTable.SelectedRows.Count > 0)
            {
                string termName = termsTable.SelectedRows[0].Cells["Назва"].Value.ToString();
                handler.DeleteTerm(termName, termsTable);
            }
        }
    }
}