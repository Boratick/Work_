using System;
using System.Linq;
using System.Windows.Forms;
using TerminologyApp.Forms;
using TerminologyApp.Models;

namespace TerminologyApp.Models
{
    public class TermListHandler
    {
        public void LoadTerms(DataGridView termsTable)
        {
            termsTable.Rows.Clear();
            foreach (var term in TermManager.Terms.OrderBy(t => t.Name))
            {
                string definitionPreview = term.Definition.Length > 50
                    ? term.Definition.Substring(0, 50) + "..."
                    : term.Definition;
                termsTable.Rows.Add(term.Name, term.Category, definitionPreview);
            }
        }

        public void ShowTermDetails(DataGridView termsTable, int rowIndex)
        {
            if (rowIndex >= 0)
            {
                string termName = termsTable.Rows[rowIndex].Cells["Назва"].Value.ToString();
                TermSearchForm searchForm = new TermSearchForm();
                searchForm.Show();
                searchForm.SelectTerm(termName);
            }
        }

        public void DeleteTerm(string termName, DataGridView termsTable)
        {
            DialogResult result = MessageBox.Show($"Ви впевнені, що хочете видалити '{termName}'?", "Підтвердити", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                var (success, message) = TermManager.RemoveTerm(termName);
                MessageBox.Show(message, success ? "Успіх" : "Помилка", MessageBoxButtons.OK, success ? MessageBoxIcon.Information : MessageBoxIcon.Error);
                if (success)
                {
                    LoadTerms(termsTable);
                }
            }
        }

        public void EditTerm(string termName, DataGridView termsTable)
        {
            Term term = TermManager.Terms.FirstOrDefault(t => t.Name == termName);
            if (term != null)
            {
                TermInputForm editForm = new TermInputForm(term);
                editForm.FormClosed += (s, e) => LoadTerms(termsTable); 
                editForm.ShowDialog();
            }
            else
            {
                MessageBox.Show($"Термін '{termName}' не знайдено.");
            }
        }
    }
}
