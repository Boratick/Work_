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
                MessageBox.Show("Будь-ласка, заповніть необхідні поля.");
                return;
            }

            
            if (TermManager.Terms.Exists(t => t.Name.Equals(nameBox.Text, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Термін з такою назвою вже існує. Виберіть іншу назву.");
                return;
            }

            Term term = new Term
            {
                Name = nameBox.Text,
                Definition = definitionBox.Text,
                Category = categoryBox.SelectedItem?.ToString() ?? "Інше",
                RelatedTerms = new List<string>(relatedTermsBox.Text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            };

            TermManager.SaveTerm(term);
            MessageBox.Show("Термін збережено!");

            nameBox.Clear();
            definitionBox.Clear();
            relatedTermsBox.Clear();
            categoryBox.SelectedIndex = -1;
        }

        public void UpdateTerm(string originalName, TextBox nameBox, TextBox definitionBox, ComboBox categoryBox, TextBox relatedTermsBox)
        {
            if (string.IsNullOrWhiteSpace(nameBox.Text) || string.IsNullOrWhiteSpace(definitionBox.Text))
            {
                MessageBox.Show("Будь-ласка, заповніть необхідні поля.");
                return;
            }

            
            if (!originalName.Equals(nameBox.Text, StringComparison.OrdinalIgnoreCase) &&
                TermManager.Terms.Exists(t => t.Name.Equals(nameBox.Text, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Термін з такою назвою вже існує. Виберіть іншу назву.");
                return;
            }

            Term updatedTerm = new Term
            {
                Name = nameBox.Text,
                Definition = definitionBox.Text,
                Category = categoryBox.SelectedItem?.ToString() ?? "Інше",
                RelatedTerms = new List<string>(relatedTermsBox.Text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            };

            TermManager.UpdateTerm(originalName, updatedTerm);
            MessageBox.Show("Термін оновлено!");

            
            nameBox.FindForm()?.Close();
        }
    }
}
