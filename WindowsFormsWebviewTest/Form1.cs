using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsWebviewTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            webView2.CoreWebView2InitializationCompleted += OnCoreWebView2InitializationCompleted;
            var env = await CoreWebView2Environment.CreateAsync(null, null, null);
            await webView2.EnsureCoreWebView2Async(env);
            webView2.Source = new Uri("http://localhost:54622/firstfile.html");
        }

        private void OnCoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            webView2.CoreWebView2.NewWindowRequested += OnNewWindowRequested;
        }


        private void OnNewWindowRequested(object sender, CoreWebView2NewWindowRequestedEventArgs e)
        {
            // Open the Uri requested in a new instance of the PopupWindow Form
            var deferral = e.GetDeferral();
            e.Handled = true;
            var popup = new PopupWindow(deferral, e);
            popup.Show();
        }
    }
}
