using System.Security.Claims;
using CSForum.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace CSForum.Services.Extensions;

public static class UserManagerExtensions
{
    public static int GetId(this UserManager<User> userManager, ClaimsPrincipal user)
    {
        try
        {
            return int.Parse(userManager.GetUserId(user));
        }
        catch (Exception e)
        {
            throw new Exception(e.Message, e);
        }
    }
}