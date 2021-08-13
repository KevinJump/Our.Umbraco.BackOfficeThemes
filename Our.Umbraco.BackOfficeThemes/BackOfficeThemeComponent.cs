#if NETFRAMEWORK
using Our.Umbraco.BackOfficeThemes.Persistance;

using Umbraco.Core.Composing;
using Umbraco.Core.Logging;
using Umbraco.Core.Migrations;
using Umbraco.Core.Migrations.Upgrade;
using Umbraco.Core.Scoping;
using Umbraco.Core.Services;


namespace Our.Umbraco.BackOfficeThemes
{
    public class BackOfficeThemeComponent : IComponent
    {
        private readonly IScopeProvider _scopeProvider;
        private readonly IKeyValueService _keyValueService;
        private readonly IMigrationBuilder _migrationBuilder;
        private readonly ILogger _logger;

        public BackOfficeThemeComponent(IScopeProvider scopeProvider,
            IKeyValueService keyValueService,
            IMigrationBuilder migrationBuilder,
            ILogger logger)
        {
            _scopeProvider = scopeProvider;
            _keyValueService = keyValueService;
            _migrationBuilder = migrationBuilder;
            _logger = logger;
        }

        public void Initialize()
        {
            var upgrader = new Upgrader(new BackOfficeThemesMigrationPlan());
            upgrader.Execute(_scopeProvider, _migrationBuilder, _keyValueService, _logger);
        }

        public void Terminate()
        {
            
        }
    }
}

#endif