using System;
using System.Windows.Forms;
using TerminologyApp.Models;

namespace TerminologyApp.Forms
{
    public partial class TermInputForm : Form
    {
        private TextBox nameBox;
        private TextBox definitionBox;
        private ComboBox categoryBox;
        private TextBox relatedTermsBox;
        private Button saveButton;
        private TermInputHandler handler;
        private readonly Term _termToEdit;
        private readonly bool _isEditMode; 

        public TermInputForm(Term termToEdit = null)
        {
            handler = new TermInputHandler();
            _termToEdit = termToEdit;
            _isEditMode = termToEdit != null;
            SetupControls();
        }

        private void SetupControls()
        {
            this.Size = new System.Drawing.Size(600, 400);
            this.FormBorderStyle = FormBorderStyle.None;

            Label nameLabel = new Label { Text = "Назва терміну:", Location = new System.Drawing.Point(20, 20) };
            nameBox = new TextBox { Location = new System.Drawing.Point(120, 20), Width = 400 };

            Label definitionLabel = new Label { Text = "Визначення:", Location = new System.Drawing.Point(20, 60) };
            definitionBox = new TextBox { Location = new System.Drawing.Point(120, 60), Width = 400, Height = 100, Multiline = true };

            Label categoryLabel = new Label { Text = "Категорія:", Location = new System.Drawing.Point(20, 180) };
            categoryBox = new ComboBox { Location = new System.Drawing.Point(120, 180), Width = 200 };
            categoryBox.Items.AddRange(new string[] { "Фізика", "Хімія", "Математика", "Біологія", "Інше" });

            Label relatedLabel = new Label { Text = "Позначення (через кому):", Location = new System.Drawing.Point(20, 220) };
            relatedTermsBox = new TextBox { Location = new System.Drawing.Point(120, 220), Width = 400 };

            saveButton = new Button
            {
                Text = _isEditMode ? "Оновити термін" : "Зберегти термін",
                Location = new System.Drawing.Point(120, 260),
                Width = 100
            };
            saveButton.Click += SaveButton_Click;

            
            if (_isEditMode)
            {
                nameBox.Text = _termToEdit.Name;
                definitionBox.Text = _termToEdit.Definition;
                categoryBox.SelectedItem = _termToEdit.Category;
                relatedTermsBox.Text = string.Join(", ", _termToEdit.RelatedTerms);
            }

            this.Controls.AddRange(new Control[] { nameLabel, nameBox, definitionLabel, definitionBox, categoryLabel, categoryBox, relatedLabel, relatedTermsBox, saveButton });
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (_isEditMode)
            {
                handler.UpdateTerm(_termToEdit.Name, nameBox, definitionBox, categoryBox, relatedTermsBox);
            }
            else
            {
                handler.SaveTerm(nameBox, definitionBox, categoryBox, relatedTermsBox);
            }
        }
    }
}
