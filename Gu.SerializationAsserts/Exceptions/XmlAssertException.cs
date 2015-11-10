﻿namespace Gu.SerializationAsserts
{
    using System;

    [Serializable]
    public class XmlAssertException : Exception
    {
        public XmlAssertException(string message)
            : base(message)
        {
        }

        public XmlAssertException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}