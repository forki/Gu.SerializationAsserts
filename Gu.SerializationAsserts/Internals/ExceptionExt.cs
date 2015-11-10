﻿namespace Gu.SerializationAsserts
{
    using System;
    using System.CodeDom.Compiler;
    using System.IO;

    public static class ExceptionExt
    {
        /// <summary>
        /// Writes all InnerException messages indented as a string.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string GetNestedMessages(this Exception e)
        {
            using (var writer = new StringWriter())
            {
                WriteMessages(writer, e);
                return writer.ToString();
            }
        }

        internal static void WriteMessages(this StringWriter writer, Exception e)
        {
            using (var indentedWriter = new IndentedTextWriter(writer, "  "))
            {
                WriteMessages(indentedWriter, e);
            }
        }

        internal static void WriteMessages(this IndentedTextWriter writer, Exception e)
        {
            writer.Write(e.GetType().Name);
            writer.Write(": ");
            writer.Write(e.Message);
            var innerException = e.InnerException;
            if (innerException != null)
            {
                writer.WriteLine();
                writer.Indent++;
                WriteMessages(writer, innerException);
            }
        }
    }
}