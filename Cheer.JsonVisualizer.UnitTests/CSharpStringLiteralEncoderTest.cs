using System;
using System.CodeDom.Compiler;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using Cheer.JsonVisualizer.CoreServices.CodeDom;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cheer.JsonVisualizer.UnitTests
{
    [TestClass]
    public class CSharpStringLiteralEncoderTest
    {
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
        public void TestDefaultEncoder()
        {
            TestEncode(CSharpStringLiteralEncoder.GetEncoder(false),
                null,
                string.Empty,
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789",
                "\u2028\u2029", // Special whitespace
                "abcdef123456@~!#$%^&*()\"\a\b\f\n\r\t\v中文",
                "" + (char)0 + (char)1 + (char)25 + (char)30 + (char)31 + (char)32 +
                    (char)33 + (char)126 + (char)127 + (char)128,
                new string(Enumerable.Range(0, 128).Select(i => (char)i).ToArray()), // ASCII,
                "𤭢" // 𤭢
            );

            TestEncode(CSharpStringLiteralEncoder.GetEncoder(true),
                null,
                string.Empty,
                "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ", // Letters/Digits
                "\u2028\u2029", // Special whitespace
                "abcdef123456@~!#$%^&*()\"\a\b\f\n\r\t\v中文",
                "" + (char)1 + (char)25 + (char)30 + (char)31 + (char)32 +
                    (char)33 + (char)126 + (char)127 + (char)128,
                new string(Enumerable.Range(1, 128).Reverse().Select(i => (char)i).ToArray()), // ASCII,
                "123456789𤭢1256" // 𤭢
            );
        }
        [TestMethod]
        public void TestWrappingEncoder()
        {
            TestEncode(CSharpStringLiteralEncoder.GetEncoder().EnableWrapping(10, 6),
                null,
                string.Empty,
                "1",
                "123456",
                "123456789",  // 9 characters
                "0123456789",  // 10 characters
                "0123456789a",  // 11 characters
                "0123456789abcdefghij", // 20 characters
                "0123456789abcdefghijk", // 21 characters,
                "𤭢",
                "12345678𤭢",
                "123456789𤭢45",
                "1234567890𤭢1231",
                "𤭢𤭢𤭢𤭢𤭢𤭢𤭢𤭢𤭢𤭢𤭢",
                "请在此处@~!#$%^&*()\"\a\b\f\n\r\t\v输入\u2028\u2029中文"
                );

            TestEncode(CSharpStringLiteralEncoder.GetEncoder().EnableWrapping(10, 6).UseVerbatimForm(),
                null,
                string.Empty,
                "1",
                "123456",
                "123456789",  // 9 characters
                "0123456789",  // 10 characters
                "0123456789a",  // 11 characters
                "0123456789abcdefghij", // 20 characters
                "0123456789abcdefghijk", // 21 characters,
                "𤭢",
                "12345678𤭢",
                "123456789𤭢45",
                "1234567890𤭢1231",
                "𤭢𤭢𤭢𤭢𤭢𤭢𤭢𤭢𤭢𤭢𤭢",
                "请在此处@~!#$%^&*()\"\a\b\f\n\r\t\v输入\u2028\u2029中文"
                );
        }

        [TestMethod]
        public void TestNewGuid() =>
            Console.WriteLine(string.Format(CultureInfo.InvariantCulture, "_{0:N}", Guid.NewGuid()));

        #region Test Helper Methods

        private static void TestEncode(CSharpStringLiteralEncoder encoder, params string[] inputs)
        {
            // Outputs
            var literals = inputs.Select(s => encoder.Encode(s)).ToArray();
            
            // 
            // Treat the encoded strings as string literals and put them in a simple C# source code,
            // (Static class field, e.g: public static readonly string field_0 = *literal*).
            // 
            string className;
            string[] fieldNames;
            var source = GenerateTestSource(literals, out className, out fieldNames);
            //
            // Then compile the source into a .NET assembly on-the-fly.
            //
            var testClassType = CompileAssembly(source).GetType(className);
            //
            // If the encoder works as expected, the value of each field
            // should be equal to the corresponding input.
            //
            for(var i = 0; i < fieldNames.Length; i++)
            {
                var fieldName = fieldNames[i];
                var fieldValue = (string)testClassType.GetField(fieldName, 
                    BindingFlags.Static | BindingFlags.Public).GetValue(null);

                Assert.AreEqual(inputs[i], fieldValue);
            }
        }
        private static string GenerateTestSource(string[] literals, out string className, out string[] fieldNames)
        {
            var source = new StringBuilder();

            var ns = string.Format(CultureInfo.InvariantCulture, "_{0:N}", Guid.NewGuid()).ToUpperInvariant();
            var cls = string.Format(CultureInfo.InvariantCulture, "_{0:N}", Guid.NewGuid()).ToUpperInvariant();

            className = ns + "." + cls;
            fieldNames = new string[literals.Length];
            
            source.AppendFormat("namespace {0}", ns).AppendLine();
            source.AppendLine("{");
            source.AppendLine("  using System;");
            source.AppendLine();
            source.AppendFormat("  public static class {0}", cls).AppendLine();
            source.AppendLine("  {");
            for(var i = 0; i < literals.Length; i++)
            {
                var literal = literals[i];
                var field = string.Format(CultureInfo.InvariantCulture, "_{0:N}", Guid.NewGuid()).ToUpperInvariant();

                source.AppendFormat("    public static readonly string {0} = {1};", 
                    fieldNames[i] = field, literal).AppendLine();
            }
            
            source.AppendLine("  }");
            source.AppendLine("}");

            return source.ToString();
        }
        private static Assembly CompileAssembly(string source)
        {
            Console.WriteLine("Generated source: {0}```{0}{1}{0}```", Environment.NewLine, source);

            using(var provider = CodeDomProvider.CreateProvider("CSharp"))
            {
                var result = provider.CompileAssemblyFromSource(new CompilerParameters
                {
                    GenerateExecutable = false,
                    GenerateInMemory = true,
                    IncludeDebugInformation = true,
                    CompilerOptions = "/target:library"
                }, source);

                if(result.Errors.HasErrors)
                {
                    var compilerErrors = string.Join(Environment.NewLine,
                        result.Errors.Cast<CompilerError>().Select(e => "- " + e));

                    Assert.Fail("One or more errors occurred compiling the source code. " +
                        "To view the generated source, see test output. {0}{1}",
                        Environment.NewLine, compilerErrors);
                }

                return result.CompiledAssembly;
            }
        }

        #endregion
    }
}