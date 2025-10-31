using System;
using System.Runtime.Serialization;

namespace OrletSoir.JSON
{
    [Serializable]
    internal class JsonParseException : ApplicationException
    {
        public JsonParseException()
        {
        }

        public JsonParseException(string message) : base(message)
        {
        }

        public JsonParseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected JsonParseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}