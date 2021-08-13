using Our.Umbraco.BackOfficeThemes.Models;

namespace Our.Umbraco.BackOfficeThemes.Persistance
{
    public interface IBackOfficeThemesRepository
    {
        void Delete(int id);
        UserThemeSettings GetByUser(int userId);
        UserThemeSettings Save(UserThemeSettings model);
    }
}