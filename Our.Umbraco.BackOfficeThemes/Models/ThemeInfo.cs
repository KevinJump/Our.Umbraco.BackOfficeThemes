using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Our.Umbraco.BackOfficeThemes.Models
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class ThemeInfo
    {
        public string Alias { get; set; }
        public string Name { get; set; }

        public string Image { get; set; }
        public string Description { get; set; }
    
    }
}
