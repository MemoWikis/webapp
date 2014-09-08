﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
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
                Sl.R<ProbabilityForUserUpdate>().Run(msg.UserId);
                Logg.r().Information("Dashboard-Probability-Stop: " + sp.Elapsed);
                
                sp.Stop();
            });
        }
    }
}
