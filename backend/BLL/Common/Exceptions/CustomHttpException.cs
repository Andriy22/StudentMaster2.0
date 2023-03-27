using System.Net;

namespace backend.BLL.Common.Exceptions;

public class CustomHttpException : Exception
{
    public CustomHttpException(string message, HttpStatusCode code = HttpStatusCode.BadRequest) : base(message)
    {
        StatusCode = code;
    }

    public HttpStatusCode StatusCode { get; set; }
}