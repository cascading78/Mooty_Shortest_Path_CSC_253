namespace Mooty_Shortest_Path;

public enum ALGORITHM_CHOICE
{
    DIJKSTRAS = 0,
    DIJKSTRAS_PRIORTY_QUEUE = 1,
    BELLMAN_FORD = 2
}

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