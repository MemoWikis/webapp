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

        public static void Info(string category, string message)
        {
            _mainWindow.AddLog(category, message);
        }

        public static void NoConnectionToMEMuchO()
        {
            Log.Info("No connection to MEMucho");
        }
    }
}
