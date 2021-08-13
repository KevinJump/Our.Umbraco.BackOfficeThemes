using NPoco;

#if NETCOREAPP
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;
#else
using Umbraco.Core.Persistence.DatabaseAnnotations;
#endif

namespace Our.Umbraco.BackOfficeThemes.Models
{
    [TableName(BackOfficeThemes.TableName)]
    [PrimaryKey("id")]
    [ExplicitColumns]
    public class UserThemeSettings
    {
        [Column("Id")]
        [PrimaryKeyColumn]
        public int Id { get; set; }

        [Column("UserId")]
        public int UserId { get; set; }

        [Column("Alias")]
        public string ThemeAlias { get; set; }

        [Column("Settings")]
        [SpecialDbType(SpecialDbTypes.NTEXT)]
        public string ThemeSettings { get; set; }
    }
}
