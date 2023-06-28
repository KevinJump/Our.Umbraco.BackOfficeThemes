using Our.Umbraco.BackOfficeThemes.Models;
using Our.Umbraco.BackOfficeThemes.Persistance;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Configuration;

#if NETCOREAPP
using Umbraco.Cms.Core.Hosting;
using Umbraco.Cms.Core.Scoping;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
#else
using Umbraco.Core;
using Newtonsoft.Json;
using Umbraco.Core.IO;
using Umbraco.Core.Scoping;
#endif

namespace Our.Umbraco.BackOfficeThemes.Services
{
    public class BackOfficeThemeService
    {
#if NET6_0_OR_GREATER
        private ICoreScopeProvider _scopeProvider;
#else
        private IScopeProvider _scopeProvider;
#endif
        private IBackOfficeThemesRepository _themeRepository;
        
        private string _themesFolder = BackOfficeThemes.ThemesFolder;

        public string ThemesFolder => _themesFolder;
        public bool Enabled { get; private set; } = true;
        public string DefaultTheme { get; private set; } = "";

#if NETCOREAPP
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;

        public BackOfficeThemeService(
#if NET6_0_OR_GREATER
            ICoreScopeProvider scopeProvider,
#else
            IScopeProvider scopeProvider,
#endif
            IHostingEnvironment hostingEnvironment,
            IBackOfficeThemesRepository themesRepository,
            IConfiguration configuration)
        {
            _configuration = configuration;

            _hostingEnvironment = hostingEnvironment;
            _scopeProvider = scopeProvider;
            _themeRepository = themesRepository;

            LoadSettings();
        }
#else

        public BackOfficeThemeService(IScopeProvider scopeProvider,
            IBackOfficeThemesRepository themesRepository)
        {
            _scopeProvider = scopeProvider;
            _themeRepository = themesRepository;

            LoadSettings();
        }
#endif

        private void LoadSettings()
        {
            _themesFolder = GetConfigSetting("BackOfficeThemes:Folder", BackOfficeThemes.ThemesFolder);
            Enabled = GetConfigSetting("BackOfficeThemes:Enabled", true);
            DefaultTheme = GetConfigSetting("BackOfficeThemes:Default", "");
        }

        public UserThemeSettings GetSettings(int userId)
        {
#if NET6_0_OR_GREATER
            using (_scopeProvider.CreateCoreScope(autoComplete: true))
#else
            using (_scopeProvider.CreateScope(autoComplete: true))
#endif
            {
                return _themeRepository.GetByUser(userId);
            }
        }

        public UserThemeSettings SaveSettings(int userId, string themeAlias)
        {
#if NET6_0_OR_GREATER
            using (_scopeProvider.CreateCoreScope(autoComplete: true))
#else
            using (_scopeProvider.CreateScope(autoComplete: true))
#endif
            {

                var themeSettings = _themeRepository.GetByUser(userId);
                if (themeSettings == null)
                {
                    themeSettings = new UserThemeSettings
                    {
                        UserId = userId,
                        ThemeSettings = "" // reserved for future use
                    };
                }

                themeSettings.ThemeAlias = themeAlias;

                return _themeRepository.Save(themeSettings);
            }
        }

        public void Reset(int userId)
        {
#if NET6_0_OR_GREATER
            using (_scopeProvider.CreateCoreScope(autoComplete: true))
#else
            using (_scopeProvider.CreateScope(autoComplete: true))
#endif
            {
                var settings = _themeRepository.GetByUser(userId);
                if (settings != null)
                {
                    _themeRepository.Delete(settings.Id);
                }
            }
        }


        public IEnumerable<ThemeInfo> GetThemes()
        {
            var folder = GetThemesFolder();

            var themes = new List<ThemeInfo>();

            if (Directory.Exists(folder))
            {
                var folderInfo = new DirectoryInfo(folder);

                foreach (var themeConfigFile in folderInfo.GetFiles("theme.json", SearchOption.AllDirectories))
                {
                    var themeConfig = File.ReadAllText(themeConfigFile.FullName);

                    var config = LoadConfig(themeConfig);

                    config.Alias = Path.GetFileName(themeConfigFile.DirectoryName);
                    config.Image = GetRelativeUrl(themeConfigFile, config.Image);
                    themes.Add(config);
                }
            }

            return themes;
        }

        public ThemeInfo GetTheme(string alias)
        {
            if (string.IsNullOrWhiteSpace(alias)) return null;
            return GetThemes()?.FirstOrDefault(x => x.Alias == alias);
        }

        public string GetThemesFolder()
#if NETCOREAPP
            => _hostingEnvironment.MapPathContentRoot(_themesFolder);
#else
            => IOHelper.MapPath(_themesFolder);
#endif

        public ThemeInfo LoadConfig(string themeConfig)
#if NETCOREAPP
            => JsonSerializer.Deserialize<ThemeInfo>(themeConfig, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
#else
            => JsonConvert.DeserializeObject<ThemeInfo>(themeConfig);
#endif

        private string GetRelativeUrl(FileInfo folder, string file)
          => $"{BackOfficeThemes.ThemesFolder}/{Path.GetFileName(folder.DirectoryName)}/{file}";


        private TResult GetConfigSetting<TResult>(string key, TResult defaultValue)
        {
#if NETCOREAPP
            return _configuration.GetValue(key, defaultValue);
#else
            var value = ConfigurationManager.AppSettings[key];
            if (value != null)
            {
                var attempt = value.TryConvertTo<TResult>();
                if (attempt.Success)
                {
                    return attempt.Result;
                }
            }
            return defaultValue;
#endif
        }

    }
}
