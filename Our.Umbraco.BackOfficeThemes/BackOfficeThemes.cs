using Our.Umbraco.BackOfficeThemes.Persistance;
using Our.Umbraco.BackOfficeThemes.Services;

#if NETCOREAPP
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Extensions;
#else
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Logging;
using Umbraco.Core.Migrations;
using Umbraco.Core.Migrations.Upgrade;
using Umbraco.Core.Scoping;
using Umbraco.Core.Services;
#endif

namespace Our.Umbraco.BackOfficeThemes
{
    internal class BackOfficeThemes
    {
        internal const string TableName = "Themes_UserConfig";

        internal const string ThemesFolder = "/App_Plugins/BackOfficeThemes/Themes";

    }

    public class BackOfficeThemesComposer : IComposer
    {
#if NETCOREAPP
        public void Compose(IUmbracoBuilder builder)
        {
            builder.Services.AddUnique<IBackOfficeThemesRepository, BackOfficeThemesRepository>();
            builder.Services.AddUnique<BackOfficeThemeService>();

            builder.AddNotificationHandler<UmbracoApplicationStartingNotification, BackOfficeNotificationHandler>();
            builder.AddNotificationHandler<ServerVariablesParsingNotification, BackOfficeNotificationHandler>();
        }
#else

        public void Compose(Composition composition)
        {
            composition.RegisterUnique<IBackOfficeThemesRepository, BackOfficeThemesRepository>();
            composition.RegisterUnique<BackOfficeThemeService>();
        }

#endif
    }
}
