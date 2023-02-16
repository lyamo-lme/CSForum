using System.Security.Claims;
using CSForum.Data.Context;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace CSForum.WebUI;

public static class WebUiExtensions
{
    public static IdentityBuilder AddAppIdentity<TUser>(this IServiceCollection services,
        Action<IdentityOptions> configureOptions) where TUser : class
    {
        services.AddAuthentication(config =>
            {
                config.DefaultScheme = "Cookie";
                config.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookie")
            .AddOpenIdConnect("oidc", config =>
            {
                config.Authority = "https://localhost:5444/";
                config.SignedOutCallbackPath = "/Home/Index";
                config.ClientId = "mvc.client";
                config.ClientSecret = "ClientSecret_MVC";
                config.SaveTokens = true;
                config.ResponseType = "code";
                config.GetClaimsFromUserInfoEndpoint = true;
                
                config.ClaimActions.MapUniqueJsonKey(ClaimTypes.NameIdentifier, ClaimTypes.NameIdentifier);
                
                config.Scope.Add("UserClaims");
                config.Scope.Add("offline_access");
            });


        return services.AddIdentityCore<TUser>(o =>
            {
                o.Stores.MaxLengthForKeys = 128;
                configureOptions?.Invoke(o);
            })
            .AddDefaultUI()
            .AddEntityFrameworkStores<ForumDbContext>();
    }
}