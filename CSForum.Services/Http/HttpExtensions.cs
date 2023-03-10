using System.Dynamic;
using System.Net;
using CSForum.Services.Extensions;

namespace CSForum.Services.Http;

public static class HttpExtensions
{
    public static async Task<HttpResponseMessage> SendAuthAsync(this HttpClient client,
        HttpRequestMessage requestMessage)
    {
        var response = await client.SendAsync(requestMessage);
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new AuthHttpException("non auth",
                new Exception(response.RequestMessage?.ToString()),
                response.StatusCode);
        }

        return response;
    }
}