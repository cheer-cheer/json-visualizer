using System;
using System.Drawing;
using System.Windows.Forms;
using Cheer.JsonVisualizer.CoreServices.Interop;

namespace Cheer.JsonVisualizer.CoreServices.Helpers
{
    public static class DisplayHelper
    {
        private const int DefaultDpi = 96;

        private static readonly Lazy<int> pixelsPerLogicalInchX = new Lazy<int>(() =>
        {
            return GetDesktopDeviceCaps(DEVICECAPS.LOGPIXELSX);
        });
        public static int PixelsPerLogicalInchX
        {
            get
            {
                return pixelsPerLogicalInchX.Value;
            }
        }

        private static readonly Lazy<int> pixelsPerLogicalInchY = new Lazy<int>(() =>
        {
            return GetDesktopDeviceCaps(DEVICECAPS.LOGPIXELSY);
        });
        public static int PixelsPerLogicalInchY
        {
            get
            {
                return pixelsPerLogicalInchY.Value;
            }
        }
        private static int GetDesktopDeviceCaps(int deviceCaps)
        {
            IntPtr? hWndDesktop = null;
            IntPtr? hDCDesktop = null;
            try
            {
                hWndDesktop = User32.GetDesktopWindow();
                hDCDesktop = User32.GetDC((IntPtr)hWndDesktop);
                return Gdi32.GetDeviceCaps((IntPtr)hDCDesktop, deviceCaps);
            }
            finally
            {
                if(hWndDesktop != null && hDCDesktop != null)
                {
                    User32.ReleaseDC((IntPtr)hWndDesktop, (IntPtr)hDCDesktop);
                }
            }
        }

        /// <summary>
        /// Scales a control from 96dpi to the actual screen dpi.
        /// </summary>
        public static void Scale(this Control c)
        {
            c.Scale(new SizeF((float)PixelsPerLogicalInchX / DefaultDpi, (float)PixelsPerLogicalInchY / DefaultDpi));
        }
    }
}
