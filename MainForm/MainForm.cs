using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TerminologyApp.Models;

namespace TerminologyApp.Form
{
    public partial class MainForm : System.Windows.Forms.Form
    {
        private TermRepository _repository = new();
        private TermNavigator _navigator;
        private List<string> _categories = new List<string> { "Фізика", "Хімія", "Біологія", "Історія" }; 

        public MainForm()
        {
            InitializeComponent();
            _navigator = new TermNavigator(_repository);
        }

        private TextBox txtName;
        private TextBox txtDefinition;
        private TextBox txtReferences;
        private ComboBox cmbCategory; 
        private ListBox lstTerms;
        private TextBox txtChain;
        private Button btnAdd;
        private Button btnShowChain;

        private void InitializeComponent()
        {
            this.MinimumSize = new System.Drawing.Size(600, 550); 
            this.Text = "Термінологія";
            this.Width = 600;
            this.Height = 550; 

            Label lblName = new() { Text = "Назва терміну", Top = 20, Left = 20, Width = 120 };
            txtName = new() { Top = lblName.Bottom + 5, Left = 20, Width = 250 };

            Label lblCategory = new() { Text = "Категорія", Top = txtName.Bottom + 10, Left = 20, Width = 120 };
            cmbCategory = new() { Top = lblCategory.Bottom + 5, Left = 20, Width = 250 };
            cmbCategory.Items.AddRange(_categories.ToArray());
            cmbCategory.DropDownStyle = ComboBoxStyle.DropDown; 
            cmbCategory.SelectedIndex = 0;

            Label lblDefinition = new() { Text = "Визначення", Top = cmbCategory.Bottom + 10, Left = 20, Width = 120 };
            txtDefinition = new() { Top = lblDefinition.Bottom + 5, Left = 20, Width = 400 };

            Label lblReferences = new() { Text = "Посилання (через кому)", Top = txtDefinition.Bottom + 10, Left = 20, Width = 200 };
            txtReferences = new() { Top = lblReferences.Bottom + 5, Left = 20, Width = 400 };

            btnAdd = new() { Text = "Додати термін", Top = txtReferences.Bottom + 10, Left = 20 };
            btnAdd.Click += BtnAdd_Click;

            lstTerms = new() { Top = btnAdd.Bottom + 15, Left = 20, Width = 200, Height = 100 };
            lstTerms.SelectedIndexChanged += LstTerms_SelectedIndexChanged;

            btnShowChain = new() { Text = "Показати ланцюжок", Top = lstTerms.Bottom + 5, Left = 20 };
            btnShowChain.Click += BtnShowChain_Click;

            txtChain = new() { Top = btnShowChain.Bottom + 10, Left = 20, Width = 500, Height = 100, Multiline = true, ReadOnly = true };
            Button btnSave = new() { Text = "Зберегти в JSON", Top = txtChain.Bottom + 10, Left = 20 };
            btnSave.Click += BtnSave_Click;

            Button btnLoad = new() { Text = "Завантажити з JSON", Top = btnSave.Top, Left = btnSave.Right + 10 };
            btnLoad.Click += BtnLoad_Click;

            this.Controls.AddRange(new Control[] { btnSave, btnLoad, lblCategory, cmbCategory });

            this.Controls.AddRange(new Control[]
            {
                lblName, txtName,
                lblDefinition, txtDefinition,
                lblReferences, txtReferences,
                btnAdd, lstTerms,
                btnShowChain, txtChain
            });
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            var name = txtName.Text.Trim();
            var def = txtDefinition.Text.Trim();
            var category = cmbCategory.Text.Trim(); 
            var refs = txtReferences.Text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                         .Select(r => r.Trim())
                                         .ToList();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(def) || string.IsNullOrWhiteSpace(category))
            {
                MessageBox.Show("Будь ласка, введіть назву, категорію та визначення терміну.");
                return;
            }

            
            if (!_categories.Contains(category))
            {
                _categories.Add(category);
                cmbCategory.Items.Add(category);
            }

            _repository.AddTerm(new Term(name, def, category, refs));
            UpdateTermList();
            MessageBox.Show("Термін додано!");
            txtName.Clear();
            txtDefinition.Clear();
            txtReferences.Clear();
            cmbCategory.SelectedIndex = 0;
        }

        private void UpdateTermList()
        {
            lstTerms.Items.Clear();
            foreach (var term in _repository.GetAllTerms())
            {
                lstTerms.Items.Add($"[{term.Category}] {term.Name}");
            }
        }

        private void LstTerms_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtChain.Clear();
        }

        private void BtnShowChain_Click(object sender, EventArgs e)
        {
            if (lstTerms.SelectedItem == null) return;

            string selected = lstTerms.SelectedItem.ToString();
            string termName = selected.Substring(selected.IndexOf(']') + 1).Trim();
            var chain = _navigator.GetChain(termName);

            txtChain.Text = string.Join(Environment.NewLine, chain.Select(t => $"[{t.Category}] {t.Name}: {t.Definition}"));
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            using SaveFileDialog dialog = new();
            dialog.Filter = "JSON файли (*.json)|*.json|Усі файли (*.*)|*.*";
            dialog.Title = "Зберегти терміни у JSON";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                _repository.SaveToFile(dialog.FileName);
                MessageBox.Show("Дані збережено успішно!");
            }
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            using OpenFileDialog dialog = new();
            dialog.Filter = "JSON файли (*.json)|*.json|Усі файли (*.*)|*.*";
            dialog.Title = "Завантажити терміни з JSON";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                _repository.LoadFromFile(dialog.FileName);
                
                foreach (var term in _repository.GetAllTerms())
                {
                    if (!string.IsNullOrEmpty(term.Category) && !_categories.Contains(term.Category))
                    {
                        _categories.Add(term.Category);
                        cmbCategory.Items.Add(term.Category);
                    }
                }
                UpdateTermList();
                MessageBox.Show("Дані завантажено успішно!");
            }
        }
    }
}