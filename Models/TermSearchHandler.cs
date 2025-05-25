using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TerminologyApp.Models;

namespace TerminologyApp.Models
{
    public class TermSearchHandler
    {
        public void SearchTerms(TextBox searchBox, ListBox resultsList)
        {
            resultsList.Items.Clear();
            if (!string.IsNullOrWhiteSpace(searchBox.Text))
            {
                var matches = TermManager.Terms.Where(t => t.Name.ToLower().StartsWith(searchBox.Text.ToLower())).ToList();
                foreach (var term in matches)
                {
                    resultsList.Items.Add(term.Name);
                }
            }
        }

        public void ShowTermDetails(ListBox resultsList, RichTextBox detailsBox)
        {
            if (resultsList.SelectedItem == null) return;

            string selectedTerm = resultsList.SelectedItem.ToString();
            Term term = TermManager.Terms.FirstOrDefault(t => t.Name == selectedTerm);
            if (term != null)
            {
                detailsBox.Clear();
                detailsBox.SelectionFont = new Font(detailsBox.Font, FontStyle.Bold);
                detailsBox.AppendText("Назва: ");
                detailsBox.SelectionFont = new Font(detailsBox.Font, FontStyle.Regular);
                detailsBox.AppendText($"{term.Name}\n");

                detailsBox.SelectionFont = new Font(detailsBox.Font, FontStyle.Bold);
                detailsBox.AppendText("Визначення: ");
                detailsBox.SelectionFont = new Font(detailsBox.Font, FontStyle.Regular);
                detailsBox.AppendText($"{term.Definition}\n");

                detailsBox.SelectionFont = new Font(detailsBox.Font, FontStyle.Bold);
                detailsBox.AppendText("Категорія: ");
                detailsBox.SelectionFont = new Font(detailsBox.Font, FontStyle.Regular);
                detailsBox.AppendText($"{term.Category}\n");

                detailsBox.SelectionFont = new Font(detailsBox.Font, FontStyle.Bold);
                detailsBox.AppendText("Пов'язані терміни: ");
                detailsBox.SelectionFont = new Font(detailsBox.Font, FontStyle.Regular);
                if (term.RelatedTerms.Any())
                {
                    foreach (string related in term.RelatedTerms)
                    {
                        detailsBox.AppendText($"\n{related}");
                    }
                }
                else
                {
                    detailsBox.AppendText("Відсутній.");
                }
            }
        }

        public void SelectTerm(string termName, ListBox resultsList)
        {
            if (TermManager.Terms.Any(t => t.Name == termName))
            {
                resultsList.SelectedItem = termName;
            }
            else
            {
                MessageBox.Show($"Термін '{termName}' не знайдено.");
            }
        }
    }
}