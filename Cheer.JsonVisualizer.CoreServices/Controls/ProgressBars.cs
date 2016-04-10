using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cheer.JsonVisualizer.CoreServices.Interop;

namespace Cheer.JsonVisualizer.CoreServices.Controls
{
    public static class ProgressBars
    {
        public static ProgressBar SetState(this ProgressBar b, ProgressBarState state)
        {
            if(b == null)
            {
                return b;
            }

            if(b.Style == ProgressBarStyle.Marquee)
            {
                b.Style = ProgressBarStyle.Blocks;
            }

            User32.SendMessage(b.Handle, 1040, (IntPtr)(state + 1), IntPtr.Zero);

            return b;
        }
    }
}
