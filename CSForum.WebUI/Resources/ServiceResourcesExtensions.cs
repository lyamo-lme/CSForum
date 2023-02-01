using System.Globalization;
using CSForum.Data.Context;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;

namespace CSForum.WebUI.Resources
{
    public static class ServiceResourcesExtensions
    {
        public static IServiceCollection AddLocalizationApp(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });
            
            const string defaultCulture = "uk-UA";
            var supportedCultures = new[]
            {
                new CultureInfo(defaultCulture),
                new CultureInfo("en-US")
            };
            serviceCollection.Configure<RequestLocalizationOptions>(options => {
                options.DefaultRequestCulture = new RequestCulture(defaultCulture);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            // serviceCollection.Configure<RequestLocalizationOptions>(options => { 
            //
            //     options.SetDefaultCulture("en-US");
            //     options.AddSupportedUICultures("uk-UA");
            //     options.FallBackToParentCultures = true;
            //     options.RequestCultureProviders = new[] { new CookieRequestCultureProvider() };
            // });
            return serviceCollection;
        }
    }
}
