using Newtonsoft.Json;

using Our.Umbraco.BackOfficeThemes.Models;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http;

using Umbraco.Web;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

using UmbracoIO = Umbraco.Core.IO;

namespace Our.Umbraco.BackOfficeThemes.Controllers
{
    [PluginController("Themes")]
    public class BackOfficeThemeApiController : UmbracoAuthorizedApiController
    {
        [HttpGet]
        public bool GetApi()
        {
            return true;
        }

        [HttpGet]
        public ThemeConfig GetConfig()
        {
            return LoadConfig();
        }

        public IEnumerable<ThemeInfo> GetThemes()
        {
            var configs = new List<ThemeInfo>();

            var themeFolder = UmbracoIO.IOHelper.MapPath("~/App_Plugins/BackOfficeThemes/Themes");
            if (Directory.Exists(themeFolder))
            {
                var folder = new DirectoryInfo(themeFolder);

                foreach (var themeConfigFile in folder.GetFiles("theme.json", SearchOption.AllDirectories))
                {
                    var themeConfig = File.ReadAllText(themeConfigFile.FullName);
                    var config = JsonConvert.DeserializeObject<ThemeInfo>(themeConfig);

                    config.Alias = Path.GetFileName(themeConfigFile.DirectoryName);
                    config.Image = GetRelativeUrl(themeConfigFile, config.Image);
                    configs.Add(config);
                }
            }

            return configs;
        }


        private static object _lock = new object();

        [HttpPut]
        public void ApplyTheme(string alias)
        {
            lock (_lock)
            {
                var config = LoadConfig();

                var existing = config.Users.FirstOrDefault(x => x.UserId == Security.CurrentUser.Id);

                if (existing != null)
                {
                    existing.Theme = alias;
                }
                else
                {
                    config.Users.Add(new ThemeUserConfig
                    {
                        Theme = alias,
                        UserId = Security.CurrentUser.Id
                    });
                }
                SaveConfig(config);
            }
        }

        [HttpPut]
        public void ResetTheme()
        {
            lock (_lock)
            {
                var config = LoadConfig();
                var existing = config.Users.FirstOrDefault(x => x.UserId == Security.CurrentUser.Id);

                if (existing != null)
                {
                    config.Users.Remove(existing);
                }

                SaveConfig(config);
            }
        }

        private ThemeConfig LoadConfig()
        {
            var configFile = UmbracoIO.IOHelper.MapPath("~/config/themes.config.json");
            if (File.Exists(configFile))
            {
                var content = File.ReadAllText(configFile);
                return JsonConvert.DeserializeObject<ThemeConfig>(content);
            }

            return new ThemeConfig();
        }

        [HttpPost]
        public void SaveConfig(ThemeConfig config)
        {
            var configFile = UmbracoIO.IOHelper.MapPath("~/config/themes.config.json");
            var content = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(configFile, content);
        }

        private string GetRelativeUrl(FileInfo folder, string file)
        {
            return UriUtility.ToAbsolute($"/App_Plugins/BackOfficeThemes/Themes/{Path.GetFileName(folder.DirectoryName)}/{file}");
        }
    }

}
