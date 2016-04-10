using System;
using System.Runtime.InteropServices;

namespace Cheer.JsonVisualizer.CoreServices.Interop.Scintilla
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct SCNotification
    {
        public Sci_NotifyHeader nmhdr;
        public int position;
        public int ch;
        public int modifiers;
        public int modificationType;
        public IntPtr text;
        public int length;
        public int linesAdded;
        public int message;
        public IntPtr wParam;
        public IntPtr lParam;
        public int line;
        public int foldLevelNow;
        public int foldLevelPrev;
        public int margin;
        public int listType;
        public int x;
        public int y;
        public int token;
        public int annotationLinesAdded;
        public int updated;
        public int listCompletionMethod;
    }
}
