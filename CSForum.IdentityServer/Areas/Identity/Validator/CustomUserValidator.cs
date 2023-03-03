using CSForum.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace CSForum.IdentityServer.Areas.Identity.Validator;


public sealed class CustomUserValidator<TUser> : UserValidator<TUser>
    where TUser : User
{
    public override  Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user)
    {                
        if (user.UserName.Any(x=>x ==':' || x == ';' || x == ' ' || x == ','))
        {
            return Task.FromResult(IdentityResult.Failed(new IdentityError
            {
                Code = "InvalidCharactersUsername",
                Description = "Username can not contain ':', ';', ' ' or ','"
            }));
        }
        return Task.FromResult(IdentityResult.Success);
    }        
}