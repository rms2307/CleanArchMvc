using System;
using System.Runtime.Serialization;

namespace CleanArchMvc.Application.Exceptions
{
    public class InternalServerErrorException : Exception
    {
        public InternalServerErrorException() : this("Ocorreu um erro interno, tente novamente mais tarde ou contate o suporte técnico.") { }
        public InternalServerErrorException(string message) : base(message) { }
        public InternalServerErrorException(string message, Exception innerException) : base(message, innerException) { }
        protected InternalServerErrorException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
