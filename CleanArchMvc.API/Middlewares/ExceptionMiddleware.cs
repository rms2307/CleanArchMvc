using CleanArchMvc.Application.DTOs;
using CleanArchMvc.Application.Exceptions;
using CleanArchMvc.Domain.Validation;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace CleanArchMvc.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = new InternalServerErrorException(GetGenericStatusCodeMessage(exception));
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = GetStatusCode(exception);
            var result = new ApiResponse<string>(response.Message);

            return context.Response.WriteAsync(JsonSerializer.Serialize(result,
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true,
                }));
        }

        private static int GetStatusCode(Exception ex)
        {
            switch (ex)
            {
                case BadRequestException _:
                    return (int)HttpStatusCode.BadRequest;
                case NotFoundException _:
                    return (int)HttpStatusCode.NotFound;
                case UnauthorizedException _:
                    return (int)HttpStatusCode.Unauthorized;
                case ForbiddenException _:
                    return (int)HttpStatusCode.Forbidden;
                case ServiceUnavailableException _:
                    return (int)HttpStatusCode.ServiceUnavailable;
                case InternalServerErrorException _:
                    return (int)HttpStatusCode.InternalServerError;
                case DomainExceptionValidation _:
                    return (int)HttpStatusCode.BadRequest;
                case StatusCodeException _:                
                    return GetGenericStatusCode(ex.Message);
                default:
                    return (int)HttpStatusCode.InternalServerError;
            }
        }

        private static int GetGenericStatusCode(string exMessage)
        {
            var croped = CropStatusCodeFromMessage(exMessage).Trim();
            croped = croped.Replace("_$!_", string.Empty);

            return int.Parse(croped);
        }

        private static string GetGenericStatusCodeMessage(Exception exception)
        {
            if (exception.GetBaseException().GetType() != typeof(StatusCodeException))
                return exception.Message;

            var newMessage = CropStatusCodeFromMessage(exception.Message);
            return exception.Message.Replace(newMessage, string.Empty).Trim();

        }

        private static string CropStatusCodeFromMessage(string message)
        {
            return message.Substring(0, 12);
        }
    }
}