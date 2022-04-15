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
            InitializeBrowserAsync();
            wv.CoreWebView2InitializationCompleted += Wv_CoreWebView2InitializationCompleted;
            wv.Source = new Uri("http://localhost:54622/firstfile.html");
        }
        public MainWindow(CoreWebView2Deferral deferral, CoreWebView2NewWindowRequestedEventArgs args)
        {
            Deferral = deferral;
            Args = args;

            InitializeComponent();
            InitializeBrowserAsync();
            wv.CoreWebView2InitializationCompleted += Wv_CoreWebView2InitializationCompleted;
        }

        void InitializeBrowserAsync()
        {
            wv.EnsureCoreWebView2Async();
        }



        private void Wv_CoreWebView2InitializationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2InitializationCompletedEventArgs e)
        {

            if (Deferral != null)
            {
                Args.Handled = true;
                Args.NewWindow = wv.CoreWebView2;
                Deferral.Complete();
            }

            wv.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;
        }


        private void CoreWebView2_NewWindowRequested(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NewWindowRequestedEventArgs e)
        {

            var deferral = e.GetDeferral();

            MainWindow mw = new MainWindow(Deferral = deferral, Args = e);

            mw.Show();

        }
    }
}
