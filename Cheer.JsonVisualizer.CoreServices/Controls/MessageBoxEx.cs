using System.Windows.Forms;
using Cheer.JsonVisualizer.CoreServices.Helpers;

namespace Cheer.JsonVisualizer.CoreServices.Controls
{
    public static class MessageBoxEx
    {
        public static MessageBoxOptions CreateMessageBoxOptions()
        {
            if(BidiHelper.IsRightToLeft)
            {
                return MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading;
            }

            return (MessageBoxOptions)0;
        }

        public static bool Confirm(string text, string caption = null, IWin32Window owner = null, bool defaultButton = true)
        {
            if(caption == null)
            {
                caption = "确认操作";
            }

            var messageBoxDefaultButton = defaultButton ? MessageBoxDefaultButton.Button1 : MessageBoxDefaultButton.Button2;
            var result = MessageBox.Show(owner, text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation,
                messageBoxDefaultButton, CreateMessageBoxOptions());
            return result == DialogResult.Yes;
        }
    }
}
