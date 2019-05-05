using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using System.Collections.Generic;

namespace Our.Umbraco.BackOfficeThemes.Models
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class ThemeConfig
    {
        public List<ThemeUserConfig> Users { get; set; }
        public List<ThemeGroupConfig> Groups { get; set; }
        public List<ThemeServerConfig> Servers { get; set; }
    }



    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class ThemeUserConfig
    {
        public int UserId { get; set; }
        public string Theme { get; set; }
    }

    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class ThemeGroupConfig
    {
        public string Group { get; set; }
        public string Theme { get; set; }
    }

    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class ThemeServerConfig
    {
        public string Pattern { get; set; }
        public string Theme { get; set; }
    }
}