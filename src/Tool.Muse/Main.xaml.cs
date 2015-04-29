using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Media;

namespace Tool.Muse
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Main : Window
    {
        private readonly UdpReceiver _updReceiver;

        public Main()
        {
            InitializeComponent();
            _updReceiver = new UdpReceiver();
            Log.Init(this);

            var observable = Observable.FromEventPattern<OscMessage>(
                ev => _updReceiver.OnReceive += ev,
                ev => _updReceiver.OnReceive -= ev);

            observable.Subscribe(m =>
            {
                if (m.EventArgs.IsConcentrationValue)
                    Dispatched(() => lblConcentration.Content = m.EventArgs.Data);

                if (m.EventArgs.IsConcentrationMellow)
                    Dispatched(() => lblMellow.Content = m.EventArgs.Data);

                if (m.EventArgs.IsHorseHoe)
                    Dispatched(() => lblConnctionTouch.Content = m.EventArgs.Data);

                if (m.EventArgs.IsQuality)
                    Dispatched(() => lblConnctionQuality.Content = m.EventArgs.Data);

                if (m.EventArgs.IsQuality)
                    Dispatched(() => lblConnctionQuality.Content = m.EventArgs.Data);

                if (m.EventArgs.IsBattery)
                    Dispatched(() => lblBattery.Content = m.EventArgs.Data);

                if(m.EventArgs.IsOnHead)
                    Dispatched(() =>
                    {
                        if (m.EventArgs.Data == "1")
                        {
                            lblOnHead.Content = "On Head";
                            lblOnHead.Background = Brushes.LawnGreen;
                        }
                        else
                        {
                            lblOnHead.Content = "Not on Head";
                            lblOnHead.Background = Brushes.Red;
                        }
                            
                    });
                    
            });
        }

        private void Dispatched(Action action)
        {
            Dispatcher.BeginInvoke(action);
        }

        private void BtnStartMuseIO_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start(
                "muse-io.exe", 
                "--preset 14  --osc osc.udp://localhost:5000");
        }

        public void AddLog(string type, string message)
        {
            Dispatcher.BeginInvoke(new Action(() => lvLog.Items.Insert(0, new { Type = type, Message = message })) );
        }

        private void BtnStartReceiver_OnClick(object sender, RoutedEventArgs e)
        {
            btnStopReceiver.IsEnabled = true; 
            btnStartReceiver.IsEnabled = false;

            _updReceiver.Start();
        }

        private void BtnStopReceiver_OnClick(object sender, RoutedEventArgs e)
        {
            btnStopReceiver.IsEnabled = false;
            btnStartReceiver.IsEnabled = true;

            _updReceiver.Stop();
        }

        private MemuchoConnection _memuchoConnection;
        private void BtnConnect_OnClick(object sender, RoutedEventArgs e)
        {
            _memuchoConnection = new MemuchoConnection();
            _memuchoConnection.Start(txtUser.Text, txtPassword.Password, txtUrl.Text);    
        }

        private void BtnSendConcentrationValue_OnClick(object sender, RoutedEventArgs e)
        {
            _memuchoConnection.SendConcentrationLevel(txtConcentrationValue.Text);
        }

        private void BtnSendMellowValue_OnClick(object sender, RoutedEventArgs e)
        {
            _memuchoConnection.SendMellowLevel(txtMellowValue.Text);
        }

        private void BtnShowDisconnected_OnClick(object sender, RoutedEventArgs e)
        {
            _memuchoConnection.SendDisconnected();
        }
    }
}