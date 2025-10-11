using BookManagement.Application.DTOs.Common;
using BookManagement.Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace BookManagement.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = HttpStatusCode.InternalServerError;
        var message = "Ichki server xatosi";
        List<string>? errors = null;

        switch (exception)
        {
            case NotFoundException:
                statusCode = HttpStatusCode.NotFound;
                message = exception.Message;
                break;

            case InvalidCredentialsException:
                statusCode = HttpStatusCode.Unauthorized;
                message = exception.Message;
                break;

            case UserAlreadyExistsException:
                statusCode = HttpStatusCode.Conflict;
                message = exception.Message;
                break;

            case BookNotAvailableException:
                statusCode = HttpStatusCode.BadRequest;
                message = exception.Message;
                break;

            case MaxBorrowLimitException:
                statusCode = HttpStatusCode.BadRequest;
                message = exception.Message;
                break;

            case FluentValidation.ValidationException validationException:
                statusCode = HttpStatusCode.BadRequest;
                message = "Validatsiya xatosi";
                errors = validationException.Errors.Select(e => e.ErrorMessage).ToList();
                break;

            case InvalidOperationException:
                statusCode = HttpStatusCode.BadRequest;
                message = exception.Message;
                break;

            default:
                _logger.LogError(exception, "Unhandled exception occurred");
                break;
        }

        var response = ApiResponse<object>.ErrorResponse(message, errors);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        return context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
    }
}