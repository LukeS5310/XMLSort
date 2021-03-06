﻿using System;
using System.Collections.Generic;
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
using Xceed.Wpf.Toolkit;

namespace XMLSort
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LBL_Status.Content = "Подготовка...";
            await Task.Run(() => Globs.LoadRA());
            // Globs.LoadRA()
            var Checker = new INIT.StartupFolderAnalyzer();
            await Task.Run(() => Checker.Analyze(System.AppDomain.CurrentDomain.BaseDirectory));
            LBL_Status.Content = Checker.ReadableResult;
            if (Checker.ReadyState == 1)
            {
                BTN_START.IsEnabled = true;
            }
        }

        private async void BTN_START_Click(object sender, RoutedEventArgs e)
        {
            BTN_START.IsEnabled = false;
            PB_Progress.IsIndeterminate = true;
            INPUT.FileGrabber grabber = new INPUT.FileGrabber();
            OUTPUT.ReportGenerator PostProcess = new OUTPUT.ReportGenerator();
            await Task.Run(() => grabber.GetFiles());
            await Task.Run(() => PostProcess.GenerateReport());
            BTN_START.IsEnabled = true;
            PB_Progress.IsIndeterminate = false;
            System.Windows.MessageBox.Show("Операция успешно выполнена!");
            this.Close();
        }

        private void NUD_PartNum_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
           Globs.PartNum = NUD_PartNum.Value ?? 0;
           
        }
    }
}
