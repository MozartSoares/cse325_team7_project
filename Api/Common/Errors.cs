using Microsoft.AspNetCore.Http;

namespace cse325_team7_project.Api.Common;

//common errors we can throw, used in the middleware to return the correct status code and message
public abstract class HttpException(int statusCode, string message) : Exception(message)
{
    public int StatusCode { get; } = statusCode;
}

public class NotFoundException(string message) : HttpException(StatusCodes.Status404NotFound, message)
{
}

public class ConflictException(string message) : HttpException(StatusCodes.Status409Conflict, message)
{
}

public class ValidationException(string message) : HttpException(StatusCodes.Status422UnprocessableEntity, message)
{
}

public class BadRequestException(string message) : HttpException(StatusCodes.Status400BadRequest, message)
{
}
