using System.Runtime.InteropServices.WindowsRuntime;
using System.Web.Razor.Tokenizer;

public class PermissionCheck
{
    //public static bool CanEdit(CategoryCacheItem category) => CanEdit(Sl.R<SessionUser>().User, category);
    public static bool CanEdit(Category category) => CheckAccess(Sl.R<SessionUser>().User, category);

    public static bool CanDelete(Category category) => CheckAccess(Sl.R<SessionUser>().User, category);

    private static bool CheckAccess(User user, ICreator entity)
    {
        var t = entity.GetType().FullName;
        if (user == null || entity == null)
            return false;

        if (user.IsInstallationAdmin)
            return true;

        if (entity.GetType().FullName == "Category")
            return true;

        if (user.Id == entity.Creator.Id)
            return true;

        return false;
    }

    private static bool CheckAccess(User user, CategoryCacheItem entity)
    {
        var t = entity.GetType().FullName;
        if (user == null || entity == null)
            return false;

        if (user.IsInstallationAdmin)
            return true;

        if (entity.GetType().FullName == "Category")
            return true;

        if (user.Id == entity.Creator.Id)
            return true;

        return false;
    }

    public static bool CanViewCategory(int id) => CanView(EntityCache.GetCategoryCacheItem(id));
    public static bool CanView(Category category) => CanView(Sl.R<SessionUser>().User, CategoryCacheItem.ToCacheCategory(category));
    public static bool CanView(CategoryCacheItem category) => CanView(Sl.R<SessionUser>().User, category);
    public static bool CanView(User user, CategoryCacheItem category)
    {
        if (category.Visibility == CategoryVisibility.All)
            return true;
        if (category.Visibility == CategoryVisibility.Owner && category.Creator == user)
            return true;
        return false;
    }

    public static bool CanEditCategory(int id) => CanEdit(EntityCache.GetCategoryCacheItem(id));
    public static bool CanEdit(CategoryCacheItem category) => CanEdit(Sl.R<SessionUser>().User, category);
    public static bool CanEdit(User user, CategoryCacheItem category)
    {
        if (user == null)
            return false;
        if (RootCategory.LockedCategory(category.Id) && !user.IsInstallationAdmin)
            return false;
        if (Sl.SessionUser.IsLoggedIn)
            return true;
        return false;
    }

    public static bool CanDelete(CategoryCacheItem category) => CanDelete(Sl.R<SessionUser>().User, category);
    public static bool CanDelete(User user, CategoryCacheItem category)
    {
        if (user == null)
            return false;
        if (category.IsWiki())
            return false;
        if (category.Creator == user || user.IsInstallationAdmin)
            return true;
        return false;
    }

    public static bool CanView(Question question) => CanView(Sl.R<SessionUser>().User, question);
    public static bool CanView(User user, Question question)
    {
        if (question.Visibility == QuestionVisibility.All)
            return true;
        if (question.Visibility == QuestionVisibility.Owner && question.Creator == user)
            return true;
        return false;
    }

    public static bool CanEdit(Question question) => CanEdit(Sl.R<SessionUser>().User, question);

    public static bool CanEdit(User user, Question question)
    {
        if (user == null)
            return false;
        if (Sl.SessionUser.IsLoggedIn)
            return true;
        return false;
    }

    public static bool CanDelete(Question question) => CanDelete(Sl.R<SessionUser>().User, question);

    public static bool CanDelete(User user, Question question)
    {
        if (user == null)
            return false;
        if (question.Creator == user || user.IsInstallationAdmin)
            return true;
        return false;
    }
}