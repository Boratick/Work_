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


        public MainForm()
        {
            InitializeComponent();
            _navigator = new TermNavigator(_repository);
        }

        private TextBox txtName;
        private TextBox txtDefinition;
        private TextBox txtReferences;
        private ListBox lstTerms;
        private TextBox txtChain;
        private Button btnAdd;
        private Button btnShowChain;
        private void BtnShowChain_Click(object sender, EventArgs e)
        {
            if (lstTerms.SelectedItem == null) return;

            string selected = lstTerms.SelectedItem.ToString();
            var chain = _navigator.GetChain(selected);

            txtChain.Text = string.Join(Environment.NewLine, chain.Select(t => $"{t.Name}: {t.Definition}"));
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
                UpdateTermList();
                MessageBox.Show("Дані завантажено успішно!");
            }
        }

        private void InitializeComponent()
        {
            this.Text = "Термінологія";
            this.Width = 600;
            this.Height = 500;

            Label lblName = new() { Text = "Назва терміну", Top = 20, Left = 20, Width = 120 };
            txtName = new() { Top = lblName.Bottom + 5, Left = 20, Width = 250 };

            Label lblDefinition = new() { Text = "Визначення", Top = txtName.Bottom + 10, Left = 20, Width = 120 };
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

            this.Controls.AddRange(new Control[] { btnSave, btnLoad });


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
            var refs = txtReferences.Text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                         .Select(r => r.Trim())
                                         .ToList();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(def))
            {
                MessageBox.Show("Будь ласка, введіть назву та визначення терміну.");
                return;
            }

            _repository.AddTerm(new Term(name, def, refs));
            UpdateTermList();
            MessageBox.Show("Термін додано!");
            txtName.Clear();
            txtDefinition.Clear();
            txtReferences.Clear();
        }

        private void UpdateTermList()
        {
            lstTerms.Items.Clear();
            foreach (var term in _repository.GetAllTerms())
            {
                lstTerms.Items.Add(term.Name);
            }
        }

        private void LstTerms_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtChain.Clear();
        }

        

    }

}
