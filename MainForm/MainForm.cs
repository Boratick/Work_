using System;
using System.Windows.Forms;
using TerminologyApp.Forms;

namespace TerminologyApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            SetupControls();
        }

        private void SetupControls()
        {
            this.Text = "Термінологія";
            this.Size = new System.Drawing.Size(800, 600);

            MenuStrip menu = new MenuStrip();
            ToolStripMenuItem helpItem = new ToolStripMenuItem("Help");
            helpItem.Click += (s, e) => new HelpForm().ShowDialog();
            ToolStripMenuItem listItem = new ToolStripMenuItem("Терміни");
            listItem.Click += (s, e) => new TermListForm().ShowDialog();
            menu.Items.AddRange(new ToolStripMenuItem[] { listItem, helpItem });

            TabControl tabs = new TabControl
            {
                Dock = DockStyle.Fill
            };

            TabPage addTab = new TabPage("Додати термін");
            TermInputForm addForm = new TermInputForm();
            addForm.TopLevel = false;
            addForm.FormBorderStyle = FormBorderStyle.None;
            addForm.Dock = DockStyle.Fill;
            addTab.Controls.Add(addForm);
            addForm.Show();

            TabPage searchTab = new TabPage("Пошук");
            TermSearchForm searchForm = new TermSearchForm();
            searchForm.TopLevel = false;
            searchForm.FormBorderStyle = FormBorderStyle.None;
            searchForm.Dock = DockStyle.Fill;
            searchTab.Controls.Add(searchForm);
            searchForm.Show();

            tabs.TabPages.Add(addTab);
            tabs.TabPages.Add(searchTab);

            this.Controls.Add(tabs);
            this.Controls.Add(menu);
            this.MainMenuStrip = menu;
        }
    }
}