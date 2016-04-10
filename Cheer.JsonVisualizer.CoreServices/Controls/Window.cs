using System;
using System.Drawing;
using System.Windows.Forms;
using Cheer.JsonVisualizer.CoreServices.Helpers;
using Cheer.JsonVisualizer.Localization;

namespace Cheer.JsonVisualizer.CoreServices.Controls
{
    public class Window
        : Form
    {
        public Window()
        {
            AccessibleRole = AccessibleRole.Window;

#pragma warning disable 612, 618
            AutoScale = false;
#pragma warning restore 612, 618
            AutoScaleMode = AutoScaleMode.Dpi;

            BackColor = SystemColors.Window;

            Font = Res.DefaultFont;

            if(BidiHelper.IsRightToLeft)
            {
                RightToLeft = RightToLeft.Yes;
                RightToLeftLayout = true;
            }
            else
            {
                RightToLeft = RightToLeft.No;
                RightToLeftLayout = false;
            }
        }

        private bool scaled;
        protected override void SetVisibleCore(bool value)
        {
            if(value && !scaled)
            {
                scaled = true;

                if(AutoScaleMode == AutoScaleMode.Dpi || AutoScaleMode == AutoScaleMode.Inherit)
                {
                    this.Scale();
                }
            }

            base.SetVisibleCore(value);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            this.RtlLayoutFixup();

            if(SystemInformation.HighContrast)
            {
                this.Walk(c =>
                {
                    var link = c as LinkLabel;

                    if(link != null)
                    {
                        if(link.LinkColor == SystemColors.HotTrack)
                        {
                            link.LinkColor = SystemColors.ControlText;
                        }
                        if(link.LinkBehavior != LinkBehavior.AlwaysUnderline)
                        {
                            link.LinkBehavior = LinkBehavior.AlwaysUnderline;
                        }
                    }
                });
            }
        }
    }
}
