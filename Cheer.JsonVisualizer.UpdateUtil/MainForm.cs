using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cheer.JsonVisualizer.CoreServices.Controls;

namespace Cheer.JsonVisualizer.UpdateUtil
{
    public partial class MainForm 
        : Window
    {
        private WebClient webClient;

        public MainForm()
        {
            InitializeComponent();

            Control.CheckForIllegalCrossThreadCalls = false;

            webClient = new WebClient();
            webClient.DownloadProgressChanged += (sender, e) =>
            {
                progressBar1.Style = ProgressBarStyle.Continuous;
                progressBar1.Value = e.ProgressPercentage;
            };
            webClient.DownloadStringCompleted += (sender, e) =>
            {
                textBox1.Text = e.Result;
            };
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            var requestUrl = "http://cheer-cheer.github.io/json-visualizer/client/latest-release.xml";

            webClient.Encoding = Encoding.UTF8;
            webClient.DownloadStringAsync(new Uri(requestUrl));
        }
    }
}
