using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cheer.JsonVisualizer.CoreServices.Helpers
{
    public static class BidiHelper
    {
        public static bool IsRightToLeft
        {
            get
            {
                var c = System.Threading.Thread.CurrentThread.CurrentUICulture;

                return c.TextInfo.IsRightToLeft;
            }
        }
        public static void RtlLayoutFixup(this Control control)
        {
            control.RtlLayoutFixup(true);
        }

        public static void RtlLayoutFixup(this Control control, bool recursive)
        {
            control.RtlLayoutFixup(recursive, control.Controls.Cast<Control>().ToArray());
        }
        public static void RtlLayoutFixup(this Control control, bool recursive, params Control[] childControls)
        {
            control.RtlLayoutFixup(recursive, false, childControls);
        }

        public static void RtlLayoutFixup(this Control control, bool recursive, bool forceAutoLayout, params Control[] childControls)
        {
            if(IsRightToLeft && control.RightToLeft != RightToLeft.No)
            {
                var form = control as Form;
                var isMirroredForm = form != null && form.RightToLeftLayout;

                foreach(var childControl in childControls)
                {
                    if(!isMirroredForm)
                    {
                        childControl.Left = control.Width - childControl.Right;

                        switch(childControl.Dock)
                        {
                            case DockStyle.Left:
                                childControl.Dock = DockStyle.Right;
                                break;
                            case DockStyle.Right:
                                childControl.Dock = DockStyle.Left;
                                break;
                        }

                        if(childControl.Dock == DockStyle.None)
                        {
                            switch(childControl.Anchor & (AnchorStyles.Left | AnchorStyles.Right))
                            {
                                case AnchorStyles.Left:
                                    childControl.Anchor &= ~AnchorStyles.Left;
                                    childControl.Anchor |= AnchorStyles.Right;
                                    break;
                                case AnchorStyles.Right:
                                    childControl.Anchor &= ~AnchorStyles.Right;
                                    childControl.Anchor |= AnchorStyles.Left;
                                    break;
                                case AnchorStyles.Left | AnchorStyles.Right:
                                    // do nothing
                                    break;
                            }
                        }
                    }

                    if(recursive)
                    {
                        childControl.RtlLayoutFixup();
                    }
                }

                if(!isMirroredForm)
                {
                    var leftMargin = control.Margin.Left;
                    var rightMargin = control.Margin.Right;
                    if(leftMargin != rightMargin)
                    {
                        control.Margin = new Padding(rightMargin, control.Margin.Top, leftMargin, control.Margin.Bottom);
                    }

                    // NOTE: This handles ScrollableControl.DockPadding as well!
                    var leftPadding = control.Padding.Left;
                    var rightPadding = control.Padding.Right;
                    if(leftPadding != rightPadding)
                    {
                        control.Padding = new Padding(rightPadding, control.Padding.Top, leftPadding, control.Padding.Bottom);
                    }
                }
            }
        }
    }
}
