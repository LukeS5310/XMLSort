using System;
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Globs.LoadRA();
            var Checker = new INIT.StartupFolderAnalyzer();
            Checker.Analyze(System.AppDomain.CurrentDomain.BaseDirectory);
            LBL_Status.Content = Checker.ReadableResult;
            if (Checker.ReadyState == 1)
            {
                BTN_START.IsEnabled = true;
            }
        }

        private void BTN_START_Click(object sender, RoutedEventArgs e)
        {
           
            INPUT.FileGrabber TestGrabber = new INPUT.FileGrabber();
            TestGrabber.GetFiles();
        }

        private void NUD_PartNum_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
           Globs.PartNum = NUD_PartNum.Value ?? 0;
           
        }
    }
}
