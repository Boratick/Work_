using System;
using System.Drawing;
using System.Windows.Forms;

namespace TerminologyApp.Forms
{
    public partial class HelpForm : Form
    {
        public HelpForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Help";
            this.Size = new System.Drawing.Size(500, 400);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            
            RichTextBox rtbHelp = new RichTextBox
            {
                Location = new System.Drawing.Point(10, 10),
                Size = new System.Drawing.Size(460, 340),
                ReadOnly = true,
                BorderStyle = BorderStyle.None
            };

            
            rtbHelp.SelectionFont = new Font(rtbHelp.Font, FontStyle.Bold);
            rtbHelp.AppendText("Опис програми\n");
            rtbHelp.SelectionFont = new Font(rtbHelp.Font, FontStyle.Regular);
            rtbHelp.AppendText("Програма \"Термінологія\" призначена для створення та управління базою термінів будь-якої науки. Вона дозволяє вводити терміни, їх визначення, категорії та пов’язані терміни, а також переглядати ланцюжки пов’язаних понять.\n\n");

            rtbHelp.SelectionFont = new Font(rtbHelp.Font, FontStyle.Bold);
            rtbHelp.AppendText("Основні функції:\n");
            rtbHelp.SelectionFont = new Font(rtbHelp.Font, FontStyle.Regular);
            rtbHelp.AppendText("- Введення терміна: На вкладці \"Add Term\" введіть назву терміна, його визначення, виберіть категорію та вкажіть пов’язані терміни (через кому).\n");
            rtbHelp.AppendText("- Пошук термінів: На вкладці \"Search Terms\" введіть перші літери терміна для автодоповнення. Виберіть термін зі списку, щоб переглянути його деталі.\n");
            rtbHelp.AppendText("- Перегляд пов’язаних термінів: Пов’язані терміни відображаються як гіперпосилання. Клікніть на них, щоб перейти до відповідного терміна.\n");
            rtbHelp.AppendText("- Збереження та завантаження: Терміни зберігаються як JSON-файли в папці \"Terms\". Програма автоматично завантажує всі збережені терміни при запуску пошуку.\n\n");

            rtbHelp.SelectionFont = new Font(rtbHelp.Font, FontStyle.Bold);
            rtbHelp.AppendText("Важливі нюанси:\n");
            rtbHelp.SelectionFont = new Font(rtbHelp.Font, FontStyle.Regular);
            rtbHelp.AppendText("- Папка \"Terms\" створюється автоматично в корені проекту для зберігання файлів термінів.\n");
            rtbHelp.AppendText("- Пов’язані терміни вводяться через кому (наприклад, \"сила, енергія, маса\").\n");
            rtbHelp.AppendText("- Назви термінів не повинні містити символи, недопустимі для імен файлів (наприклад, /, \\, :, *).\n");
            rtbHelp.AppendText("- Для коректної роботи автодоповнення вводьте щонайменше 1-2 літери терміна.\n");
            rtbHelp.AppendText("- Якщо пов’язаний термін не існує в базі, при кліку на нього з’явиться повідомлення про помилку.\n");

            this.Controls.Add(rtbHelp);
        }
    }
}