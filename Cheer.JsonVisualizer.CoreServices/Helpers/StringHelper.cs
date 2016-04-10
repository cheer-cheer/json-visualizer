using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Cheer.JsonVisualizer.CoreServices.Helpers
{
    public static class StringHelper
    {
        public static string MakeSafe(this string s, bool trim = false) => 
            string.IsNullOrEmpty(s) ? string.Empty : trim ? s.Trim() : s;

        public static bool IsNullOrEmpty(this string s) => string.IsNullOrEmpty(s);
        public static bool IsHighSurrogate(this char ch) => char.IsHighSurrogate(ch);
        public static bool IsLowSurrogate(this char ch) => char.IsLowSurrogate(ch);
        public static bool IsWhiteSpace(this char ch) => char.IsWhiteSpace(ch);
    }
}
