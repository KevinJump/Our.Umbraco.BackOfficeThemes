using Our.Umbraco.BackOfficeThemes.Persistance.Migrations;

#if NETCOREAPP
using Umbraco.Cms.Infrastructure.Migrations;
#else
using Umbraco.Core.Migrations;
#endif

namespace Our.Umbraco.BackOfficeThemes.Persistance
{
    public class BackOfficeThemesMigrationPlan : MigrationPlan
    {
        public BackOfficeThemesMigrationPlan() 
            : base("BackOfficeThemes")
        {
            From(string.Empty)
                .To<CreateThemeSettingsTableMigration>("Initalized");
        }
    }
}
