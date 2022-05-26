using System.Net;

namespace CleanArchMvc.Application.Exceptions
{
    public static class ExceptionHelper
    {
        public static void ThrowsByStatusCode(HttpStatusCode statusCode, string message = null)
        {
            switch (statusCode)
            {
                case HttpStatusCode.BadRequest:
                case HttpStatusCode.Conflict:
                    throw new BadRequestException(message);
                case HttpStatusCode.Forbidden:
                    throw new ForbiddenException(message);
                case HttpStatusCode.InternalServerError:
                    throw new InternalServerErrorException(message);
                case HttpStatusCode.NotFound:
                    throw new NotFoundException(message);
                case HttpStatusCode.ServiceUnavailable:
                    throw new ServiceUnavailableException(message);
                case HttpStatusCode.Unauthorized:
                    throw new UnauthorizedException(message);
                default:
                    throw new InternalServerErrorException(message);
            }
        }
    }
}
