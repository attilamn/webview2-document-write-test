using Microsoft.Web.WebView2.Core;
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

namespace WebviewTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public CoreWebView2Deferral Deferral;
        public CoreWebView2NewWindowRequestedEventArgs Args;

        public MainWindow()
        {
            InitializeComponent();

        }
        public MainWindow(CoreWebView2Deferral deferral, CoreWebView2NewWindowRequestedEventArgs args)
        {
            Deferral = deferral;
            Args = args;

            InitializeComponent();
        }
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            wv.CoreWebView2InitializationCompleted += Wv_CoreWebView2InitializationCompleted;
            var env = await CoreWebView2Environment.CreateAsync(null, null, null);
            await wv.EnsureCoreWebView2Async(env);

            wv.Source = new Uri("http://localhost:54622/firstfile.html");

        }


        private void Wv_CoreWebView2InitializationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2InitializationCompletedEventArgs e)
        {

            if (Deferral != null)
            {
                Args.NewWindow = wv.CoreWebView2;
                Deferral.Complete();
            }

            wv.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;
        }


        private void CoreWebView2_NewWindowRequested(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NewWindowRequestedEventArgs e)
        {

            var deferral = e.GetDeferral();
            e.Handled = true;
            MainWindow mw = new MainWindow(Deferral = deferral, Args = e);

            mw.Show();

        }

    }
}
