using System.Net;

namespace CSForum.Services.Extensions;

public class AuthHttpException:HttpRequestException
{
    public HttpStatusCode StatusCode { get; set; }

    public AuthHttpException(string m, Exception e,  HttpStatusCode statusCode):base(m,e,statusCode)
    {
        StatusCode = statusCode;
    }
}