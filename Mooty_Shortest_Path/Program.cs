namespace Mooty_Shortest_Path
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new frmMain());
        }

        public static void ShowMessage(string message, string form_caption, Point location)
        {
            frmMessage frmMsgDialog = new frmMessage();
            frmMsgDialog.ShowMessage(message, form_caption, location);
            frmMsgDialog.ShowDialog();
        }

    }

}