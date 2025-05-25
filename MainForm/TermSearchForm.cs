using System;
using System.Windows.Forms;
using TerminologyApp.Models;

namespace TerminologyApp.Forms
{
    public partial class TermSearchForm : Form
    {
        private TextBox searchBox;
        private ListBox resultsList;
        private RichTextBox detailsBox;
        private TermSearchHandler handler;

        public TermSearchForm()
        {
            handler = new TermSearchHandler();
            SetupControls();
            TermManager.GetTerms();
        }

        private void SetupControls()
        {
            this.Size = new System.Drawing.Size(600, 400);
            this.FormBorderStyle = FormBorderStyle.None;

            Label searchLabel = new Label { Text = "Знайти термін:", Location = new System.Drawing.Point(20, 20) };
            searchBox = new TextBox { Location = new System.Drawing.Point(120, 20), Width = 400 };
            searchBox.TextChanged += SearchBox_TextChanged;

            resultsList = new ListBox { Location = new System.Drawing.Point(120, 60), Width = 400, Height = 100 };
            resultsList.SelectedIndexChanged += ResultsList_SelectedIndexChanged;

            detailsBox = new RichTextBox
            {
                Location = new System.Drawing.Point(120, 180),
                Width = 400,
                Height = 150,
                ReadOnly = true,
                BorderStyle = BorderStyle.None
            };

            this.Controls.AddRange(new Control[] { searchLabel, searchBox, resultsList, detailsBox });
        }

        private void SearchBox_TextChanged(object sender, EventArgs e)
        {
            handler.SearchTerms(searchBox, resultsList);
        }

        private void ResultsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            handler.ShowTermDetails(resultsList, detailsBox);
        }

        public void SelectTerm(string termName)
        {
            handler.SelectTerm(termName, resultsList);
        }
    }
}