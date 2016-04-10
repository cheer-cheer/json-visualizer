using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cheer.JsonVisualizer.CoreServices.Helpers;
using System.CodeDom.Compiler;
using Cheer.JsonVisualizer.CoreServices.CodeDom;

namespace Cheer.JsonVisualizer.UnitTests
{
    [TestClass]
    public class CSharpStringLiteralEncoderTest
    {
        private void AssertProperties(CSharpStringLiteralEncoder encoder, bool verbatim, bool isWrappingEnabled)
        {
            if(verbatim)
            {
                Assert.IsTrue(encoder.Verbatim);
            }
            else
            {
                Assert.IsFalse(encoder.Verbatim);
            }
            if(isWrappingEnabled)
            {
                Assert.IsTrue(encoder.IsWrappingEnabled);
            }
            else
            {
                Assert.IsFalse(encoder.IsWrappingEnabled);
            }

            Assert.IsTrue(encoder.WrapChars <= 0);
            Assert.AreEqual(encoder.Indent, 0);
            Assert.AreEqual(encoder.IndentationChar, CSharpStringLiteralEncoder.DefaultIndentationChar);
        }
        [TestMethod]
        public void TestGetEncoder()
        {
            var encoder = CSharpStringLiteralEncoder.GetEncoder(false);
            Assert.IsFalse(encoder.Verbatim);
            Assert.IsFalse(encoder.IsWrappingEnabled);
            Assert.IsTrue(encoder.WrapChars <= 0);
            Assert.AreEqual(encoder.Indent, 0);
            Assert.AreEqual(encoder.IndentationChar, CSharpStringLiteralEncoder.DefaultIndentationChar);
            
            encoder = CSharpStringLiteralEncoder.GetEncoder(false, 80);
            Assert.IsFalse(encoder.Verbatim);
            Assert.IsTrue(encoder.IsWrappingEnabled);
            Assert.AreEqual(encoder.WrapChars, 80);
            Assert.AreEqual(encoder.Indent, 0);
            Assert.AreEqual(encoder.IndentationChar, CSharpStringLiteralEncoder.DefaultIndentationChar);

            encoder = CSharpStringLiteralEncoder.GetEncoder(false, 80, 4);
            Assert.IsFalse(encoder.Verbatim);
            Assert.IsTrue(encoder.IsWrappingEnabled);
            Assert.AreEqual(encoder.WrapChars, 80);
            Assert.AreEqual(encoder.Indent, 4);
            Assert.AreEqual(encoder.IndentationChar, CSharpStringLiteralEncoder.DefaultIndentationChar);

            encoder = CSharpStringLiteralEncoder.GetEncoder(false, 80, 4, '\t');
            Assert.IsFalse(encoder.Verbatim);
            Assert.IsTrue(encoder.IsWrappingEnabled);
            Assert.AreEqual(encoder.WrapChars, 80);
            Assert.AreEqual(encoder.Indent, 4);
            Assert.AreEqual(encoder.IndentationChar, '\t');


            encoder = CSharpStringLiteralEncoder.GetEncoder(true);
            Assert.IsTrue(encoder.Verbatim);
            Assert.IsFalse(encoder.IsWrappingEnabled);
            Assert.IsTrue(encoder.WrapChars <= 0);
            Assert.AreEqual(encoder.Indent, 0);
            Assert.AreEqual(encoder.IndentationChar, CSharpStringLiteralEncoder.DefaultIndentationChar);

            encoder = CSharpStringLiteralEncoder.GetEncoder(true, 80);
            Assert.IsTrue(encoder.Verbatim);
            Assert.IsTrue(encoder.IsWrappingEnabled);
            Assert.AreEqual(encoder.WrapChars, 80);
            Assert.AreEqual(encoder.Indent, 0);
            Assert.AreEqual(encoder.IndentationChar, CSharpStringLiteralEncoder.DefaultIndentationChar);

            encoder = CSharpStringLiteralEncoder.GetEncoder(true, 80, 4);
            Assert.IsTrue(encoder.Verbatim);
            Assert.IsTrue(encoder.IsWrappingEnabled);
            Assert.AreEqual(encoder.WrapChars, 80);
            Assert.AreEqual(encoder.Indent, 4);
            Assert.AreEqual(encoder.IndentationChar, CSharpStringLiteralEncoder.DefaultIndentationChar);

            encoder = CSharpStringLiteralEncoder.GetEncoder(true, 80, 4, '\t');
            Assert.IsTrue(encoder.Verbatim);
            Assert.IsTrue(encoder.IsWrappingEnabled);
            Assert.AreEqual(encoder.WrapChars, 80);
            Assert.AreEqual(encoder.Indent, 4);
            Assert.AreEqual(encoder.IndentationChar, '\t');
        }


        [TestMethod]
        public void TestCSharpStringEncode()
        {
            string input = null;

            var encoder = CSharpStringLiteralEncoder.GetEncoder(true);

            Assert.AreEqual("null", encoder.Encode(input));
            Assert.AreEqual("null", encoder.Encode(input));

            //input = "abcdef123456@~!#$%^&*()\"\a\b\f\n\r\t\v中文" +
            //    "\u2028\u2029" +
            //    (char)0 + (char)1 + (char)25 + (char)30 + (char)31 + (char)32 + (char)33 + (char)126 + (char)127 + (char)128;

            //var e = "abcdef123456@~!#$%^&*()\"\a\b\f\n\r\t\v中文\u2028\u2029\0\x0001\x0019\x001e\x001f !~\x007f";
            //Assert.AreEqual(e, input);
            //var output = input.CSharpStringEncode(false);


            //Console.WriteLine(output);
        }
    }
}
