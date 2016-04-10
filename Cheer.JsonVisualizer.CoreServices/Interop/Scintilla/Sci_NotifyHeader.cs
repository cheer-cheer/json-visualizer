using System;
using System.Runtime.InteropServices;

namespace Cheer.JsonVisualizer.CoreServices.Interop.Scintilla
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct Sci_NotifyHeader
    {
        public IntPtr hwndFrom;
        public IntPtr idFrom;
        public int code;
    }
}
