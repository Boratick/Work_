using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TerminologyApp.Models;

namespace TerminologyApp.Models
{
    public class TermInputHandler
    {
        public void SaveTerm(TextBox nameBox, TextBox definitionBox, ComboBox categoryBox, TextBox relatedTermsBox)
        {
            if (string.IsNullOrWhiteSpace(nameBox.Text) || string.IsNullOrWhiteSpace(definitionBox.Text))
            {
                MessageBox.Show("Будь-ласка, заповніть необхнідні поля.");
                return;
            }

            Term term = new Term
            {
                Name = nameBox.Text,
                Definition = definitionBox.Text,
                Category = categoryBox.SelectedItem?.ToString() ?? "Інші",
                RelatedTerms = new List<string>(relatedTermsBox.Text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            };

            TermManager.SaveTerm(term);
            MessageBox.Show("Термін збережено!");

            nameBox.Clear();
            definitionBox.Clear();
            relatedTermsBox.Clear();
            categoryBox.SelectedIndex = -1;
        }
    }
}