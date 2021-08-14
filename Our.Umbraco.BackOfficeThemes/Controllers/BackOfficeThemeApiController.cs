
using Our.Umbraco.BackOfficeThemes.Models;
using Our.Umbraco.BackOfficeThemes.Services;

using System.Collections.Generic;
using System.Linq;
#if NETCOREAPP
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common.Attributes;
#else
using System.Web.Http;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;
#endif

namespace Our.Umbraco.BackOfficeThemes.Controllers
{
    [PluginController("Themes")]
    public class BackOfficeThemeApiController : UmbracoAuthorizedApiController
    {

        private readonly BackOfficeThemeService _themeService;

#if NETCOREAPP
        private readonly IBackOfficeSecurityAccessor _backOfficeSecurityAccessor;

        public BackOfficeThemeApiController(
            IBackOfficeSecurityAccessor backOfficeSecurityAccessor,
            BackOfficeThemeService themeService)
        {
            _backOfficeSecurityAccessor = backOfficeSecurityAccessor;
            _themeService = themeService;
        }
#else

        public BackOfficeThemeApiController(
            BackOfficeThemeService themeService)
        {
            _themeService = themeService;
        }
#endif


        [HttpGet]
        public bool GetApi() => true;

        [HttpGet]
        public IEnumerable<ThemeInfo> GetThemes()
            => _themeService.GetThemes().OrderBy(x => x.Name);


        [HttpPut]
        public void ApplyTheme(string alias)
        {
            var userId = GetCurrentUserId();
            _themeService.SaveSettings(userId, alias);
        }

        [HttpGet]
        public string GetCurrentTheme()
        {
            if (!_themeService.Enabled)
                return _themeService.DefaultTheme;

            var userId = GetCurrentUserId();
            var theme = _themeService.GetSettings(userId);

            return theme?.ThemeAlias ?? _themeService.DefaultTheme;
        }

        [HttpGet]
        public  ThemeInfo GetCurrentThemeInfo()
        {
            if (!_themeService.Enabled)
                return _themeService.GetTheme(_themeService.DefaultTheme);

            var userId = GetCurrentUserId();
            var theme = _themeService.GetSettings(userId);

            if (!string.IsNullOrWhiteSpace(theme?.ThemeAlias))
                return _themeService.GetTheme(theme?.ThemeAlias);

            return _themeService.GetTheme(_themeService.DefaultTheme);

        }

        [HttpPut]
        public void ResetTheme()
        {
            var userId = GetCurrentUserId();
            _themeService.Reset(userId);
        }


        private int GetCurrentUserId()
#if NETCOREAPP
            => _backOfficeSecurityAccessor.BackOfficeSecurity.CurrentUser.Id;
#else 
            => Security.CurrentUser.Id;
#endif
    }
}
