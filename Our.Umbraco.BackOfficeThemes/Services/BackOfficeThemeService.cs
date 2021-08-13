using Our.Umbraco.BackOfficeThemes.Models;
using Our.Umbraco.BackOfficeThemes.Persistance;

using System.Collections.Generic;
using System.IO;

#if NETCOREAPP
using Umbraco.Cms.Core.Hosting;
using Umbraco.Cms.Core.Scoping;
using System.Text.Json;
#else
using Newtonsoft.Json;
using Umbraco.Core.IO;
using Umbraco.Core.Scoping;
#endif

namespace Our.Umbraco.BackOfficeThemes.Services
{
    public class BackOfficeThemeService
    {
        private IScopeProvider _scopeProvider;
        private IBackOfficeThemesRepository _themeRepository;

#if NETCOREAPP
        private readonly IHostingEnvironment _hostingEnvironment;

        public BackOfficeThemeService(
            IScopeProvider scopeProvider,
            IHostingEnvironment hostingEnvironment,
            IBackOfficeThemesRepository themesRepository)
        {
            _hostingEnvironment = hostingEnvironment;
            _scopeProvider = scopeProvider;
            _themeRepository = themesRepository;
        }
#else 

        public BackOfficeThemeService(IScopeProvider scopeProvider,
            IBackOfficeThemesRepository themesRepository)
        {
            _scopeProvider = scopeProvider;
            _themeRepository = themesRepository;
        }
#endif

        public UserThemeSettings GetSettings(int userId)
        {
            using (_scopeProvider.CreateScope(autoComplete: true))
            {
                return _themeRepository.GetByUser(userId);
            }
        }

        public UserThemeSettings SaveSettings(int userId, string themeAlias)
        {
            using (_scopeProvider.CreateScope(autoComplete: true))
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
            using (_scopeProvider.CreateScope(autoComplete: true))
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

        public string GetThemesFolder()
#if NETCOREAPP
            => _hostingEnvironment.MapPathContentRoot(BackOfficeThemes.ThemesFolder);
#else
            => IOHelper.MapPath(BackOfficeThemes.ThemesFolder);
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

    }
}
