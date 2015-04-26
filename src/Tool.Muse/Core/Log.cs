using System.ComponentModel;

namespace Tool.Muse
{
    public class Log
    {
        private static Main _mainWindow;

        public static void Init(Main main)
        {
            _mainWindow = main;
        }

        public static void Info(string message)
        {
            _mainWindow.AddLog("Info", message);
        }
    }
}
