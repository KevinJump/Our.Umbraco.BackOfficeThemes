#if NETCOREAPP
using Our.Umbraco.BackOfficeThemes.Persistance;

using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Migrations;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Migrations.Upgrade;

namespace Our.Umbraco.BackOfficeThemes
{
    public class AppStartingNotification
        : INotificationHandler<UmbracoApplicationStartingNotification>
    {
        private readonly IScopeProvider _scopeProvider;
        private readonly IKeyValueService _keyValueService;
        private readonly IMigrationPlanExecutor _migrationPlanExecutor;

        private readonly IRuntimeState _runtimeState;

        public AppStartingNotification(IScopeProvider scopeProvider,
            IKeyValueService keyValueService,
            IMigrationPlanExecutor migrationPlanExecutor,
            IRuntimeState runtimeState)
        {
            _scopeProvider = scopeProvider;
            _keyValueService = keyValueService;
            _migrationPlanExecutor = migrationPlanExecutor;
            _runtimeState = runtimeState;
        }

        public void Handle(UmbracoApplicationStartingNotification notification)
        {
            if (_runtimeState.Level == RuntimeLevel.Run)
            {
                var upgrader = new Upgrader(new BackOfficeThemesMigrationPlan());
                upgrader.Execute(_migrationPlanExecutor, _scopeProvider, _keyValueService);
            }
        }
    }
}
#endif