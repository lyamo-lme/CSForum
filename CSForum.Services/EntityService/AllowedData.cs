using CSForum.Core.Models;

namespace CSForum.Services.EntityService;

public static class AllowedData
{
    public static List<string> AllowedUserData = new List<string>
    {
        nameof(User.Answers),
        nameof(User.Posts),
        "null"
    };
}