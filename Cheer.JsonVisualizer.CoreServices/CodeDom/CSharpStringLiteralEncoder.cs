using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Cheer.JsonVisualizer.CoreServices.Helpers;

namespace Cheer.JsonVisualizer.CoreServices.CodeDom
{
    public class CSharpStringLiteralEncoder
    {
        private const string NullLiteral = "null";
        private const char DoubleQuoteChar = '"';
        /// <summary>
        /// The default indentation character for the encoder, that is, a normal white space.
        /// </summary>
        public const char DefaultIndentationChar = ' ';

        protected CSharpStringLiteralEncoder()
        {
        }

        public bool Verbatim
        {
            get;
            private set;
        }

        public bool IsWrappingEnabled
        {
            get
            {
                return WrapChars > 0;
            }
        }

        public int WrapChars
        {
            get;
            private set;
        }
        public int Indent
        {
            get;
            private set;
        }
        public char IndentationChar
        {
            get;
            private set;
        } = DefaultIndentationChar;

        public CSharpStringLiteralEncoder UseVerbatimForm() =>
            Verbatim ? this : new CSharpStringLiteralEncoder
            {
                Verbatim = true,
                Indent = this.Indent,
                IndentationChar = this.IndentationChar,
                WrapChars = this.WrapChars
            };

        public CSharpStringLiteralEncoder UseRegularForm() =>
            Verbatim ? new CSharpStringLiteralEncoder
            {
                Verbatim = false,
                Indent = this.Indent,
                IndentationChar = this.IndentationChar,
                WrapChars = this.WrapChars
            } : this;

        public CSharpStringLiteralEncoder EnableWrapping(int wrapChars, int indent = 0, char indentationChar = ' ')
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
                new CSharpStringLiteralEncoder
                {
                    Verbatim = this.Verbatim,
                    WrapChars = wrapChars,
                    Indent = indent,
                    IndentationChar = indentationChar
                };
        }

        public CSharpStringLiteralEncoder DisableWrapping() =>
             IsWrappingEnabled ? new CSharpStringLiteralEncoder
             {
                 Verbatim = this.Verbatim,
                 Indent = this.Indent,
                 IndentationChar = this.IndentationChar,
                 WrapChars = 0
             } : this;
        
        public string Encode(string input) => Encode(input, null).ToString();
        
        public virtual StringBuilder Encode(string input, StringBuilder buffer)
        {
            if(input == null)
            {
                return buffer == null ? new StringBuilder(NullLiteral) : buffer.Append(NullLiteral);
            }

            return Verbatim ? EncodeVerbatimForm(input, buffer) : EncodeNormalForm(input, buffer);
        }
        private StringBuilder EncodeVerbatimForm(string input, StringBuilder buffer)
        {
            if(buffer == null)
            {
                buffer = new StringBuilder(input.Length + 5);
            }

            buffer.Append('@').Append(DoubleQuoteChar);

            for(var i = 0; i < input.Length; i++)
            {
                var ch = input[i];

                if(ch == DoubleQuoteChar)
                {
                    buffer.Append(DoubleQuoteChar).Append(DoubleQuoteChar);
                }
                else
                {
                    buffer.Append(ch);
                }

                if(IsWrappingEnabled && i > 0 && (i % WrapChars) == 0)
                {
                    var isLastChar = i == input.Length - 1;

                    if(ch.IsHighSurrogate() && !isLastChar && input[i + 1].IsLowSurrogate())
                    {
                        buffer.Append(input[++i]);
                    }

                    if(!isLastChar)
                    {
                        buffer.Append(DoubleQuoteChar).Append(' ').Append("+");
                        buffer.Append(Environment.NewLine);
                        if(Indent > 0)
                        {
                            for(var c = 0; c < Indent; c++)
                            {
                                buffer.Append(IndentationChar);
                            }
                        }
                        buffer.Append('@').Append(DoubleQuoteChar);
                    }
                }
            }
            buffer.Append(DoubleQuoteChar);

            return buffer;
        }
        private StringBuilder EncodeNormalForm(string input, StringBuilder buffer)
        {
            if(buffer == null)
            {
                buffer = new StringBuilder(input.Length + 5);
            }

            buffer.Append(DoubleQuoteChar);

            for(int i = 0; i < input.Length; i++)
            {
                var ch = input[i];

                switch(ch)
                {
                    case '\0':
                        buffer.Append(@"\0");
                        break;
                    case '\a':
                        buffer.Append(@"\a");
                        break;
                    case '\b':
                        buffer.Append(@"\b");
                        break;
                    case '\f':
                        buffer.Append(@"\f");
                        break;
                    case '\n':
                        buffer.Append(@"\n");
                        break;
                    case '\r':
                        buffer.Append(@"\r");
                        break;
                    case '\t':
                        buffer.Append(@"\t");
                        break;
                    case '\v':
                        // Vertical tab
                        buffer.Append(@"\v");
                        break;
                    case '"':
                        // Double quotation mark
                        buffer.Append("\\\"");
                        break;
                    case '\\':
                        // Backslash
                        buffer.Append(@"\\");
                        break;
                    case '\u2028':
                        buffer.Append(@"\u").Append("2028");
                        break;
                    case '\u2029':
                        buffer.Append(@"\u").Append("2029");
                        break;
                    default:
                        if((ch >= 0 && ch < 32) || ch == 127)
                        {
                            buffer.Append(@"\x").AppendFormat(CultureInfo.InvariantCulture, "{0:x4}", (int)ch);
                        }
                        else
                        {
                            buffer.Append(ch);
                        }
                        break;
                }

                if(IsWrappingEnabled && i > 0 && (i % WrapChars) == 0)
                {
                    var isLastChar = i == input.Length - 1;

                    if(ch.IsHighSurrogate() && !isLastChar && input[i + 1].IsLowSurrogate())
                    {
                        buffer.Append(input[++i]);
                    }

                    if(!isLastChar)
                    {
                        buffer.Append(DoubleQuoteChar).Append(' ').Append("+");
                        buffer.Append(Environment.NewLine);
                        if(Indent > 0)
                        {
                            for(var c = 0; c < Indent; c++)
                            {
                                buffer.Append(IndentationChar);
                            }
                        }
                        buffer.Append(DoubleQuoteChar);
                    }
                }
            }
            buffer.Append(DoubleQuoteChar);

            return buffer;
        }

        public static CSharpStringLiteralEncoder GetEncoder(bool verbatim = false,
            int wrapChars = 0, int indent = 0, char indentationChar = DefaultIndentationChar)
        {
            var encoder = new CSharpStringLiteralEncoder
            {
                Verbatim = verbatim
            };

            return wrapChars > 0 ? encoder.EnableWrapping(wrapChars, indent, indentationChar) : encoder;
        }
    }
}
