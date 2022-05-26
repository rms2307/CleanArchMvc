﻿using System;
using System.Runtime.Serialization;

namespace CleanArchMvc.Application.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException() : this("Dados de requisição inválidos.") { }
        public BadRequestException(string message) : base(message) { }
        public BadRequestException(string message, Exception innerException) : base(message, innerException) { }
        protected BadRequestException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    }
}
