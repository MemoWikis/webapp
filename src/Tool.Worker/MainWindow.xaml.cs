using System.Diagnostics;
using System.Windows;
using TrueOrFalse;

namespace Tool.Worker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Bus.Get().Subscribe<RecalcProbabilitiesMsg>("RecalcProbabilities", msg =>
            {
                var sp = Stopwatch.StartNew();

                Logg.r().Information("Dashboard-Probability-Start: " + sp.Elapsed);
                ProbabilityUpdate_Valuation.Run(msg.UserId);
                Logg.r().Information("Dashboard-Probability-Stop: " + sp.Elapsed);
                
                sp.Stop();
            });
        }
    }
}
