using System;
using System.Linq;
using System.Windows.Forms;
using TerminologyApp.Forms;
using TerminologyApp.Models;

namespace TerminologyApp.Models
{
    // Обробляє відображення та взаємодію зі списком термінів у таблиці.
    public class TermListHandler
    {
        // Завантажує терміни до таблиці DataGridView, відображаючи назву, категорію та короткий опис.
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
        
        // Відображає детальну інформацію про вибраний термін у новому вікні пошуку.
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

        // Видаляє вибраний термін із бази даних після підтвердження користувача.
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

        // Відкриває форму редагування для вибраного терміна.
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
