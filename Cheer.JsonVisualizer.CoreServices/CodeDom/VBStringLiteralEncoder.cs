using System;
using System.Globalization;
using System.Text;
using Cheer.JsonVisualizer.CoreServices.Helpers;

namespace Cheer.JsonVisualizer.CoreServices.CodeDom
{
    public class VBStringLiteralEncoder
        : StringLiteralEncoder
    {
        private const string NothingLiteral = "Nothing";
        private const char DoubleQuoteChar = '"';
        public const char DefaultIndentationChar = ' ';

        protected VBStringLiteralEncoder()
        {
            IndentationChar = DefaultIndentationChar;
        }

        public VBStringLiteralEncoder EnableWrapping(int wrapChars, int indent = 0, char indentationChar = ' ')
        {
            if(wrapChars <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(wrapChars), "Value must be greater than zero.");
            }
            if(indent < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(indent), "Value must be non-negative.");
            }
            if(!indentationChar.IsWhiteSpace())
            {
                throw new ArgumentException(nameof(indentationChar), "It must be a white space character.");
            }

            return WrapChars == wrapChars && Indent == indent && IndentationChar == indentationChar ? this :
                new VBStringLiteralEncoder
                {
                    WrapChars = wrapChars,
                    Indent = indent,
                    IndentationChar = indentationChar
                };
        }

        public VBStringLiteralEncoder DisableWrapping() =>
             IsWrappingEnabled ? new VBStringLiteralEncoder
             {
                 Indent = this.Indent,
                 IndentationChar = this.IndentationChar,
                 WrapChars = 0
             } : this;

        public override StringBuilder Encode(string value, StringBuilder buffer)
        {
            if(value == null)
            {
                return buffer == null ? new StringBuilder(NothingLiteral) : buffer.Append(NothingLiteral);
            }
            
            var b = buffer == null ? new StringBuilder(value.Length + 5) : buffer;

            var fInDoubleQuotes = true;

            b.Append(DoubleQuoteChar);
            var i = 0;
            while(i < value.Length)
            {
                var ch = value[i];
                switch(ch)
                {
                    case '\"':
                    // These are the inward sloping quotes used by default in some cultures like CHS. 
                    // VBC.EXE does a mapping ANSI that results in it treating these as syntactically equivalent to a
                    // regular double quote.
                    case '\u201C':
                    case '\u201D':
                    case '\uFF02':
                        EnsureInDoubleQuotes(ref fInDoubleQuotes, b);
                        b.Append(ch).Append(ch);
                        break;
                    case '\r':
                        EnsureNotInDoubleQuotes(ref fInDoubleQuotes, b);
                        b.Append("&Global.Microsoft.VisualBasic.ChrW(13)");
                        if(i < value.Length - 1 && value[i + 1] == '\n')
                        {
                            b.Append("&Global.Microsoft.VisualBasic.ChrW(10)");
                            i++;
                        }
                        break;
                    case '\t':
                        EnsureNotInDoubleQuotes(ref fInDoubleQuotes, b);
                        b.Append("&Global.Microsoft.VisualBasic.ChrW(9)");
                        break;
                    case '\0':
                        EnsureNotInDoubleQuotes(ref fInDoubleQuotes, b);
                        b.Append("&Global.Microsoft.VisualBasic.ChrW(0)");
                        break;
                    case '\n':
                        EnsureNotInDoubleQuotes(ref fInDoubleQuotes, b);
                        b.Append("&Global.Microsoft.VisualBasic.ChrW(10)");
                        break;
                    case '\u2028':
                        EnsureNotInDoubleQuotes(ref fInDoubleQuotes, b);
                        b.Append("&Global.Microsoft.VisualBasic.ChrW(8232)");
                        break;
                    case '\u2029':
                        EnsureNotInDoubleQuotes(ref fInDoubleQuotes, b);
                        b.Append("&Global.Microsoft.VisualBasic.ChrW(8233)");
                        break;
                    default:
                        if((ch >= 0 && ch < 32) || ch == 127)
                        {
                            EnsureNotInDoubleQuotes(ref fInDoubleQuotes, b);
                            b.Append("&Global.Microsoft.VisualBasic.ChrW(");
                            b.Append(ch.ToString(NumberFormatInfo.InvariantInfo));
                            b.Append(')');
                        }
                        else
                        {
                            EnsureInDoubleQuotes(ref fInDoubleQuotes, b);
                            b.Append(ch);
                        }
                        break;
                }

                if(IsWrappingEnabled && i > 0 && i % WrapChars == 0)
                {
                    var isLastChar = i == value.Length - 1;

                    //
                    // If current character is a high surrogate and the following 
                    // character is a low surrogate, don't break them. 
                    // Otherwise when we write the string to a file, we might lose 
                    // the characters.
                    // 
                    if(ch.IsHighSurrogate() && !isLastChar && value[i + 1].IsLowSurrogate())
                    {
                        b.Append(value[++i]);
                    }

                    if(!isLastChar)
                    {
                        if(fInDoubleQuotes)
                        {
                            b.Append(DoubleQuoteChar);
                        }

                        fInDoubleQuotes = true;

                        b.Append("& _ ");
                        b.Append(Environment.NewLine);
                        if(Indent > 0)
                        {
                            for(var c = 0; c < Indent; c++)
                            {
                                buffer.Append(IndentationChar);
                            }
                        }
                        b.Append(DoubleQuoteChar);
                    }
                }
                ++i;
            }

            if(fInDoubleQuotes)
            {
                b.Append(DoubleQuoteChar);
            }

            return b;
        }
        
        private static void EnsureInDoubleQuotes(ref bool fInDoubleQuotes, StringBuilder b)
        {
            if(fInDoubleQuotes)
            {
                return;
            } 

            b.Append("&\"");
            fInDoubleQuotes = true;
        }
        private static void EnsureNotInDoubleQuotes(ref bool fInDoubleQuotes, StringBuilder b)
        {
            if(!fInDoubleQuotes)
            {
                return;
            }

            b.Append("\"");
            fInDoubleQuotes = false;
        }

        public static VBStringLiteralEncoder GetEncoder(int wrapChars = 0, int indent = 0, 
            char indentationChar = DefaultIndentationChar)
        {
            var encoder = new VBStringLiteralEncoder();

            return wrapChars > 0 ? encoder.EnableWrapping(wrapChars, indent, indentationChar) : encoder;
        }
    }
}
