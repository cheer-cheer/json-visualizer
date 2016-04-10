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
        private UpdateChecker updateChecker;

        public MainForm()
        {
            InitializeComponent();

            Control.CheckForIllegalCrossThreadCalls = false;
            updateChecker = new UpdateChecker();
        }

        protected async override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            try
            {
                await updateChecker.GetProductReleaseInfoAsync();
                progressBar1.Visible = false;
            }
            catch(Exception ex)
            {
                progressBar1.Value = 100;
                progressBar1.SetState(ProgressBarState.Error);
                textBox1.Text = ex.ToString();
            }
        }
    }
}
