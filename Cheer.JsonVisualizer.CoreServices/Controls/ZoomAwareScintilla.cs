using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Cheer.JsonVisualizer.CoreServices.Interop.Scintilla;
using ScintillaNET;

namespace Cheer.JsonVisualizer.CoreServices.Controls
{
    public class ZoomAwareScintilla
         : Scintilla
    {
        private static readonly object ZoomedEventKey = new object();

        [Category("Notifications"), Description("Occurs when zoom factor is changed.")]
        public event EventHandler<EventArgs> Zoomed
        {
            add
            {
                base.Events.AddHandler(ZoomedEventKey, value);
            }
            remove
            {
                base.Events.RemoveHandler(ZoomedEventKey, value);
            }
        }

        protected virtual void OnZoomed(EventArgs e)
        {
            var handler = base.Events[ZoomedEventKey] as EventHandler<EventArgs>;
            if(handler != null)
            {
                handler(this, e);
            }
        }

        protected override void WndProc(ref Message m)
        {
            if(m.Msg == 0x204e)
            {
                WmReflectNotify(ref m);
            }
            else
            {
                base.WndProc(ref m);
            }
        }
        private void WmReflectNotify(ref Message m)
        {
            var scn = (SCNotification)Marshal.PtrToStructure(m.LParam, typeof(SCNotification));
            var code = scn.nmhdr.code;
            if(code >= 0x7d0 && code <= 0x7ee)
            {
                switch(code)
                {
                    case 0x7e2:
                        OnZoomed(EventArgs.Empty);
                        return;
                }
                base.WndProc(ref m);
            }
        }
    }
}