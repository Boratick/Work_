using System;
using System.Windows.Forms;

namespace TerminologyApp
{
    static class Program
    {
        [STAThread] 
        static void Main()
        {
            //check
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form.MainForm());
        }
    }
}
