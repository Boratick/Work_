namespace Курсовая
{
    internal static class Program
    {
        static void Main()
        {
            
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new TerminologyApp.Form.MainForm());


        }
    }
}