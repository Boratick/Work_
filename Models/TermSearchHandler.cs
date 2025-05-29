using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TerminologyApp.Models;

namespace TerminologyApp.Models
{   // Обробляє пошук термінів та відображення їх деталей, включаючи підтримку гіперпосилань.
    public class TermSearchHandler

    {  
        // Виконує пошук термінів за введеним запитом і оновлює список результатів.
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

        // Відображає детальну інформацію про вибраний термін у полі деталей.
        public void ShowTermDetails(ListBox resultsList, RichTextBox detailsBox)
        {
            if (resultsList.SelectedItem == null) return;

            string selectedTerm = resultsList.SelectedItem.ToString();
            Term term = TermManager.Terms.FirstOrDefault(t => t.Name == selectedTerm);
            if (term != null)
            {
                detailsBox.Clear();

                // Назва терміну
                detailsBox.SelectionFont = new Font(detailsBox.Font, FontStyle.Bold);
                detailsBox.SelectionColor = Color.Black;
                detailsBox.AppendText("Назва: ");
                detailsBox.SelectionFont = new Font(detailsBox.Font, FontStyle.Regular);
                detailsBox.AppendText($"{term.Name}\n\n");

                // Визначення
                detailsBox.SelectionFont = new Font(detailsBox.Font, FontStyle.Bold);
                detailsBox.AppendText("Визначення: ");
                detailsBox.SelectionFont = new Font(detailsBox.Font, FontStyle.Regular);
                detailsBox.AppendText($"{term.Definition}\n\n");

                // Категорія
                detailsBox.SelectionFont = new Font(detailsBox.Font, FontStyle.Bold);
                detailsBox.AppendText("Категорія: ");
                detailsBox.SelectionFont = new Font(detailsBox.Font, FontStyle.Regular);
                detailsBox.AppendText($"{term.Category}\n\n");

                // Пов'язані терміни з гіперпосиланнями
                detailsBox.SelectionFont = new Font(detailsBox.Font, FontStyle.Bold);
                detailsBox.SelectionColor = Color.Black;
                detailsBox.AppendText("Пов'язані терміни: ");

                if (term.RelatedTerms.Any())
                {
                    detailsBox.AppendText("\n");
                    for (int i = 0; i < term.RelatedTerms.Count; i++)
                    {
                        string related = term.RelatedTerms[i].Trim();

                        // Перевірка наявності терміна в базі даних
                        bool termExists = TermManager.Terms.Any(t => t.Name.Equals(related, StringComparison.OrdinalIgnoreCase));

                        if (termExists)
                        {
                            // Форматування як активне гіперпосилання
                            detailsBox.SelectionColor = Color.Blue;
                            detailsBox.SelectionFont = new Font(detailsBox.Font, FontStyle.Underline);
                        }
                        else
                        {
                            // Форматируем как неактивную ссылку
                            detailsBox.SelectionColor = Color.Gray;
                            detailsBox.SelectionFont = new Font(detailsBox.Font, FontStyle.Regular);
                        }

                        detailsBox.AppendText(related);

                        // Додаємо роздільник, якщо це не останній термін
                        if (i < term.RelatedTerms.Count - 1)
                        {
                            detailsBox.SelectionColor = Color.Black;
                            detailsBox.SelectionFont = new Font(detailsBox.Font, FontStyle.Regular);
                            detailsBox.AppendText(", ");
                        }
                    }
                    detailsBox.AppendText("\n");
                }
                else
                {
                    detailsBox.SelectionColor = Color.Black;
                    detailsBox.SelectionFont = new Font(detailsBox.Font, FontStyle.Regular);
                    detailsBox.AppendText("Відсутні.\n");
                }

                // Повертаження курсора до початку
                detailsBox.SelectionStart = 0;
                detailsBox.SelectionLength = 0;
            }
        }

        public void SelectTerm(string termName, ListBox resultsList)
        {
            // Шукаємо термін за назвою
            var exactMatch = TermManager.Terms.FirstOrDefault(t => t.Name.Equals(termName, StringComparison.OrdinalIgnoreCase));

            if (exactMatch != null)
            {
                // Оновлюємо список результатів та вибираємо знайдений термін
                resultsList.Items.Clear();
                resultsList.Items.Add(exactMatch.Name);
                resultsList.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show($"Термін '{termName}' не знайдено.", "Термін не знайдено", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Метод для обробки кліку по гіперпосиланню в полі деталей
        public void HandleHyperlinkClick(RichTextBox detailsBox, Point clickLocation, ListBox resultsList, TextBox searchBox)
        {
            // Отримуємо індекс символа, на якому був клік
            int charIndex = detailsBox.GetCharIndexFromPosition(clickLocation);
            if (charIndex < 0) return;

            // Перевіряємо, що індекс не виходить за межі тексту
            if (charIndex >= detailsBox.Text.Length) return;

            // Вставляємо курсор на позицію кліка
            detailsBox.SelectionStart = charIndex;
            detailsBox.SelectionLength = 0;

            // Перевіряємо, чи є символ на позиції кліка гіперпосиланням
            if (detailsBox.SelectionColor == Color.Blue && detailsBox.SelectionFont.Underline)
            {
                string termName = ExtractTermFromPosition(detailsBox.Text, charIndex);
                if (!string.IsNullOrEmpty(termName))
                {
                    // Оновлення поля пошуку
                    searchBox.Text = termName;

                    // Виконуємо пошук термінів та вибираємо знайдений термін
                    SearchTerms(searchBox, resultsList);
                    SelectTerm(termName, resultsList);
                }
            }
        }

        // Допоміжний метод для витягування терміна з тексту за позицією
        private string ExtractTermFromPosition(string text, int position)
        {
            if (string.IsNullOrEmpty(text) || position < 0 || position >= text.Length)
                return string.Empty;

            int start = position;
            int end = position;

            // Знаходимо початок терміна (идем назад)
            while (start > 0)
            {
                char prevChar = text[start - 1];
                if (char.IsWhiteSpace(prevChar) || prevChar == ',' || prevChar == ':' || prevChar == '\n')
                    break;
                start--;
            }

            // Знаходимо кінець терміна (идем вперед)
            while (end < text.Length)
            {
                char currentChar = text[end];
                if (char.IsWhiteSpace(currentChar) || currentChar == ',' || currentChar == '\n')
                    break;
                end++;
            }

            // Витягуємо термін, якщо він не порожній
            if (end > start)
            {
                string extractedTerm = text.Substring(start, end - start).Trim();
                // Убираємо розділові знаки в кінці терміна
                extractedTerm = extractedTerm.TrimEnd(',', '.', ';', ':', '!', '?');
                return extractedTerm;
            }

            return string.Empty;
        }
    }
}
