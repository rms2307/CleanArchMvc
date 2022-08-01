using System.Collections.Generic;

namespace CleanArchMvc.Application.DTOs
{
    public class ApiResponse<T>
    {
        public T Data { get; private set; }
        public List<string> Errors { get; private set; } = new();

        public ApiResponse(T data)
        {
            Data = data;
        }

        public ApiResponse(List<string> errors)
        {
            Errors = errors;
        }

        public ApiResponse(string error)
        {
            Errors.Add(error);
        }

        public ApiResponse(T data, List<string> errors)
        {
            Data = data;
            Errors = errors;
        }
    }
}
