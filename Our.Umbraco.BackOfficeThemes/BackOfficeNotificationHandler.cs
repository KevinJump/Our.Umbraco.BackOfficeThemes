#if NETCOREAPP
using Our.Umbraco.BackOfficeThemes.Persistance;
using Our.Umbraco.BackOfficeThemes.Services;

using System.Collections.Generic;

using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Migrations;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Migrations.Upgrade;

namespace Our.Umbraco.BackOfficeThemes
{
    public class BackOfficeNotificationHandler
        : INotificationHandler<UmbracoApplicationStartingNotification>,
        INotificationHandler<ServerVariablesParsingNotification>
    {

#if NET6_0_OR_GREATER
        private readonly ICoreScopeProvider _scopeProvider;
#else
        private readonly IScopeProvider _scopeProvider;
#endif
        private readonly IKeyValueService _keyValueService;
        private readonly IMigrationPlanExecutor _migrationPlanExecutor;

        private readonly BackOfficeThemeService _themeService;

        public BackOfficeNotificationHandler(
#if NET6_0_OR_GREATER
            ICoreScopeProvider scopeProvider,
#else
            IScopeProvider scopeProvider,
           
#endif
            IKeyValueService keyValueService, 
            IMigrationPlanExecutor migrationPlanExecutor,
            BackOfficeThemeService themeService)
        {
            _scopeProvider = scopeProvider;
            _keyValueService = keyValueService;
            _migrationPlanExecutor = migrationPlanExecutor;

            _themeService = themeService;
        }

        public void Handle(UmbracoApplicationStartingNotification notification)
        {
            if (notification.RuntimeLevel == RuntimeLevel.Run)
            {
                var upgrader = new Upgrader(new BackOfficeThemesMigrationPlan());
                upgrader.Execute(_migrationPlanExecutor, _scopeProvider, _keyValueService);
            }
        }

        public void Handle(ServerVariablesParsingNotification notification)
        {
            notification.ServerVariables.Add("backOfficeThemes",
                new Dictionary<string, object>
                { 
                    { "themeFolder", _themeService.ThemesFolder },
                    { "enabled", _themeService.Enabled },
                    { "default", _themeService.DefaultTheme }
                });
        }
    }
}
#endif