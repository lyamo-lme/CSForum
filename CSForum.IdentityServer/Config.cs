using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace CSForum.IdentityServer
{
    public  class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource()
                {
                    Name = "UserClaims",
                    UserClaims = new List<string>()
                    {
                        "Id",
                        JwtClaimTypes.Email
                    }
                }
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new[] { new ApiScope("api") };
        
        public static IEnumerable<ApiResource> ApiResources =>
            new[]
            {
                new ApiResource("api")
                {
                    Scopes = new List<string> { "api" },
                    ApiSecrets = new List<Secret> { new Secret("ScopeSecret".Sha256()) },
                    UserClaims = new List<string>
                    {
                        "Id",
                        JwtClaimTypes.Email
                    }
                }
            };

        public static IEnumerable<Client> Clients =>
            new[]
            {
                new Client
                {
                    ClientId = "api.client",
                    ClientName = "Client Credentials Client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("ClientSecret1".Sha256())
                    },
                    AllowedScopes =
                    {
                        "api",
                        "UserClaims",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    },
                    AccessTokenLifetime = 60*2,
                    AllowOfflineAccess = true,
                    AlwaysIncludeUserClaimsInIdToken = true
                },
                new Client
                {
                    ClientId = "mvc.client",
                    ClientName = "Client Credentials Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = new List<string>()
                    {
                        "http://localhost:5161/signin-oidc",
                        "https://localhost:5161/signin-oidc"
                    },
                    PostLogoutRedirectUris = new List<string>()
                    {
                        "http://localhost:5161/signin-oidc",
                        "https://localhost:5161/Home/Index"
                    },
                    AllowedCorsOrigins = new List<string>()
                    {
                        "https://localhost:5161",
                        "https://localhost:5200"
                    },
                    ClientSecrets = { new Secret("ClientSecret_MVC".Sha256()) },
                    AllowedScopes =
                    {
                        "UserClaims",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    },
                    AccessTokenLifetime = 60*2,
                    AllowOfflineAccess = true,
                    AlwaysIncludeUserClaimsInIdToken = true
                },
                new Client
                {
                    ClientId = "interactive",
                    ClientSecrets = { new Secret("ClientSecret1".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris =
                    {
                        "https://localhost:5444/signin-oidc" ,
                    },
                    FrontChannelLogoutUri = "https://localhost:5444/signout-oidc",
                    PostLogoutRedirectUris = { "https://localhost:5444/signout-callback-oidc" },
                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "api"  },
                    RequirePkce = true,
                    RequireConsent = true,
                    AllowPlainTextPkce = false
                },
            };
    }
}
