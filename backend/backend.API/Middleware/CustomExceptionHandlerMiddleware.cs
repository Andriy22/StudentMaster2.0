using System.Diagnostics;
using System.Net;
using System.Text.Json;
using backend.BLL.Common.Exceptions;
using FluentValidation;

namespace backend.API.Middleware;

public class CustomExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public CustomExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError;
        var result = string.Empty;

        if (exception as ValidationException != null)
        {
            code = HttpStatusCode.BadRequest;
            foreach (var item in ((ValidationException)exception).Errors) result += item.ErrorMessage;

            result = JsonSerializer.Serialize(new
                { code = (int)code, error = result, requestId = Activity.Current.Id });
        }
        else
        {
            switch (exception)
            {
                case CustomHttpException customHttpException:
                    code = customHttpException.StatusCode;
                    result = JsonSerializer.Serialize(new
                        { code = (int)code, error = customHttpException.Message, requestId = Activity.Current.Id });
                    break;
            }
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        if (result == string.Empty)
            result = JsonSerializer.Serialize(new
                { error = exception.Message, trace = exception.StackTrace, requestId = Activity.Current.Id });

        return context.Response.WriteAsync(result);
    }
}