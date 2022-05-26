using System;
using System.Runtime.Serialization;

namespace CleanArchMvc.Application.Exceptions
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException() : this("Não autorizado.") { }
        public ForbiddenException(string message) : base(message) { }
        public ForbiddenException(string message, Exception innerException) : base(message, innerException) { }
        protected ForbiddenException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
