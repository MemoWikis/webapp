using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Tool.Muse
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Main : Window
    {
        public Main()
        {
            InitializeComponent();
            Log.Init(this);
        }

        private void BtnStartMuseIO_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start(
                "muse-io.exe", 
                "--preset 14  --osc osc.udp://localhost:5000");
        }

        public void AddLog(string type, string message)
        {
            Application.Current.Dispatcher.BeginInvoke(
                new Action(() => lvLog.Items.Add(new { Type = type, Message = message })) );
        }

        private void BtnStartReceiver_OnClick(object sender, RoutedEventArgs e)
        {
            btnStopReceiver.IsEnabled = true; 
            btnStartReceiver.IsEnabled = false;

            UdpReceiver.Start();
        }

        private void BtnStopReceiver_OnClick(object sender, RoutedEventArgs e)
        {
            btnStopReceiver.IsEnabled = false;
            btnStartReceiver.IsEnabled = true;

            UdpReceiver.Stop();
        }
    }
}
