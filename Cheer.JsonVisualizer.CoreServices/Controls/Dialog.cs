using System.Windows.Forms;

namespace Cheer.JsonVisualizer.CoreServices.Controls
{
    public class Dialog 
        : Window
    {
        public Dialog()
        {
            AccessibleRole = AccessibleRole.Dialog;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false; 
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
        }
    }
}
