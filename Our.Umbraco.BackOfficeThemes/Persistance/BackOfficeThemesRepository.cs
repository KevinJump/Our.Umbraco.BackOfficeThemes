using NPoco;

using Our.Umbraco.BackOfficeThemes.Models;

using System;
using System.Linq;

#if NET6_0_OR_GREATER
using Umbraco.Cms.Infrastructure.Scoping;
#elif NET5_0
using Umbraco.Cms.Core.Scoping;
#endif

#if NETCOREAPP
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Infrastructure.Persistence.SqlSyntax;
using Umbraco.Extensions;
#else
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.SqlSyntax;
using Umbraco.Core.Scoping;
#endif

namespace Our.Umbraco.BackOfficeThemes.Persistance
{
    internal class BackOfficeThemesRepository : IBackOfficeThemesRepository
    {
        const int maxParams = 2000;
        const string TableName = BackOfficeThemes.TableName;

        private readonly IScopeAccessor _scopeAccessor;

        public BackOfficeThemesRepository(IScopeAccessor scopeAccessor)
        {
            _scopeAccessor = scopeAccessor;
        }

        private IScope AmbientScope
        {
            get
            {
                var scope = _scopeAccessor.AmbientScope;
                if (scope == null)
                    throw new InvalidOperationException("Cannot run without an ambient scope");

                return scope;
            }
        }

        private IUmbracoDatabase Database => AmbientScope.Database;
        private ISqlContext SqlContext => AmbientScope.SqlContext;
        private Sql<ISqlContext> Sql() => SqlContext.Sql();
        private ISqlSyntaxProvider SqlSyntax => SqlContext.SqlSyntax;


        private Sql<ISqlContext> GetBaseQuery(bool isCount)
            => isCount
                ? Sql().SelectCount().From<UserThemeSettings>()
                : Sql().Select($"{TableName}.*").From<UserThemeSettings>();

        private string GetBaseWhereClause()
            => $"{TableName}.{nameof(UserThemeSettings.Id)} = @Id";


        public UserThemeSettings GetByUser(int userId)
        {
            var sql = GetBaseQuery(false)
                .Where<UserThemeSettings>(x => x.UserId == userId);

            return Database.FirstOrDefault<UserThemeSettings>(sql);
        }

        public UserThemeSettings Save(UserThemeSettings model)
        {
            using (var transaction = Database.GetTransaction())
            {
                Database.Save(model);
                transaction.Complete();
            }

            return model;
        }

        public void Delete(int id)
        {
            using (var transaction = Database.GetTransaction())
            {
                Database.Delete<UserThemeSettings>(id);
                transaction.Complete();
            }
        }

    }
}
