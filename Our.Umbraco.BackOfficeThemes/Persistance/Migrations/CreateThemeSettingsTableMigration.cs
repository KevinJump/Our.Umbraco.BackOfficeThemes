using Our.Umbraco.BackOfficeThemes.Models;


#if NETCOREAPP
using Umbraco.Cms.Infrastructure.Migrations;
#else
using Umbraco.Core.Migrations;
#endif

namespace Our.Umbraco.BackOfficeThemes.Persistance.Migrations
{
    public class CreateThemeSettingsTableMigration : MigrationBase
    {
        public CreateThemeSettingsTableMigration(IMigrationContext context) 
            : base(context)
        {
        }

#if NETCOREAPP
        protected override void Migrate()
#else 
        public override void Migrate()
#endif
        {
            if (!TableExists(BackOfficeThemes.TableName))
                Create.Table<UserThemeSettings>().Do();
        }
    }
}
