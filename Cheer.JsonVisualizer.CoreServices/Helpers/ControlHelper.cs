using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cheer.JsonVisualizer.CoreServices.Helpers
{
    public static class ControlHelper
    {
        public static void Walk(this Control control, Action<Control> walker)
        {
            Walk(control, (c, o) =>
            {
                walker(control);
            }, null);
        }
        public static void Walk(this Control control, Action<Control, object> walker, object state)
        {
            if(control != null)
            {
                walker(control, state);

                foreach(Control c in control.Controls)
                {
                    c.Walk(walker, state);
                }
            }
        }
    }
}
