using System.Net;
using CSForum.Services.Http;

namespace CSForum.WebUI.Services.HttpClients;

public static class HttpExtensions
{
    public static async Task<HttpResponseMessage> SendAuthAsync(this HttpClient client,
        HttpRequestMessage requestMessage)
    {
        var response = await client.SendAsync(requestMessage);
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new HttpRequestException("non auth", new Exception(response.RequestMessage.ToString()),
                response.StatusCode);
        }

        return response;
    }
}