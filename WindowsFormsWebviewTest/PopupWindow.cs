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
    public partial class PopupWindow : Form
    {
        public PopupWindow() => InitializeComponent();
        public PopupWindow(CoreWebView2Deferral deferral, CoreWebView2NewWindowRequestedEventArgs args)
            : this()
        {
            Core2Deferral = deferral;
            NewWindowArgs = args;
        }

        protected virtual CoreWebView2Deferral Core2Deferral { get; private set; }
        protected virtual CoreWebView2NewWindowRequestedEventArgs NewWindowArgs { get; private set; }

        protected async override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            webView2.CoreWebView2InitializationCompleted += OnCoreWebView2InitializationCompleted;

            var env = await CoreWebView2Environment.CreateAsync(null, null, null);
            await webView2.EnsureCoreWebView2Async(env);
        }

        private void OnCoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            webView2.CoreWebView2.Settings.AreDefaultContextMenusEnabled = true;
            if (Core2Deferral != null)
            {
                NewWindowArgs.NewWindow = webView2.CoreWebView2;
                Core2Deferral.Complete();
            }
            webView2.CoreWebView2.NewWindowRequested += OnNewWindowRequested;
        }

        private void OnNewWindowRequested(object sender, CoreWebView2NewWindowRequestedEventArgs e)
        {
            e.Handled = true;
            var popup = new PopupWindow(e.GetDeferral(), e);
            popup.Show();
        }
    }
}
