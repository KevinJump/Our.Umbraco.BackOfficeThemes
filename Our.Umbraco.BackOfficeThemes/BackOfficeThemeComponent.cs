#if NETFRAMEWORK
using Our.Umbraco.BackOfficeThemes.Persistance;

using System.Collections.Generic;
using System.Web.Routing;
using System.Web;
using System;

using Umbraco.Core.Composing;
using Umbraco.Core.Logging;
using Umbraco.Core.Migrations;
using Umbraco.Core.Migrations.Upgrade;
using Umbraco.Core.Scoping;
using Umbraco.Core.Services;
using System.Web.Mvc;
using Umbraco.Web.JavaScript;
using Our.Umbraco.BackOfficeThemes.Services;

namespace Our.Umbraco.BackOfficeThemes
{
    public class BackOfficeThemeComponent : IComponent
    {
        private readonly IScopeProvider _scopeProvider;
        private readonly IKeyValueService _keyValueService;
        private readonly IMigrationBuilder _migrationBuilder;
        private readonly ILogger _logger;

        private readonly BackOfficeThemeService _themeService;

        public BackOfficeThemeComponent(IScopeProvider scopeProvider,
            IKeyValueService keyValueService,
            IMigrationBuilder migrationBuilder,
            ILogger logger,
            BackOfficeThemeService themeService)
        {
            _scopeProvider = scopeProvider;
            _keyValueService = keyValueService;
            _migrationBuilder = migrationBuilder;
            _logger = logger;

            _themeService = themeService;
        }

        public void Initialize()
        {
            var upgrader = new Upgrader(new BackOfficeThemesMigrationPlan());
            upgrader.Execute(_scopeProvider, _migrationBuilder, _keyValueService, _logger);

            ServerVariablesParser.Parsing += ServerVariablesParser_Parsing;

        }

        public void Terminate()
        {
            
        }

        private void ServerVariablesParser_Parsing(object sender, Dictionary<string, object> e)
        {
            if (HttpContext.Current == null) throw new InvalidOperationException("This method requires that an HttpContext be active");

            var urlHelper = new UrlHelper(new RequestContext(new HttpContextWrapper(HttpContext.Current), new RouteData()));

            e.Add("backOfficeThemes", new Dictionary<string, object>
                {
                    { "themeFolder", _themeService.ThemesFolder },
                    { "enabled", _themeService.Enabled },
                    { "default", _themeService.DefaultTheme }
                });
        }
    }
}

#endif