using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cheer.JsonVisualizer.CoreServices.Helpers;

namespace Cheer.JsonVisualizer.CoreServices.CodeDom
{
    public abstract class StringLiteralEncoder
    {
        public virtual bool IsWrappingEnabled
        {
            get
            {
                return WrapChars > 0;
            }
        }

        public virtual int WrapChars
        {
            get;
            protected set;
        }
        public virtual int Indent
        {
            get;
            protected set;
        }
        public virtual char IndentationChar
        {
            get;
            protected set;
        }

        public abstract StringBuilder Encode(string value, StringBuilder buffer);

        public virtual string Encode(string value) => Encode(value, null).ToString();
    }
}
