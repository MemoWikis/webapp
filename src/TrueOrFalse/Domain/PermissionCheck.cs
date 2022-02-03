using System.Runtime.InteropServices.WindowsRuntime;
using System.Web.Razor.Tokenizer;

public class PermissionCheck
{
    public static bool CanViewCategory(int id) => CanView(EntityCache.GetCategoryCacheItem(id));
    public static bool CanView(Category category) => CanView(EntityCache.GetCategoryCacheItem(category.Id));
    public static bool CanView(CategoryCacheItem category) => CanView(Sl.SessionUser.User, category);

    public static bool CanView(User user, CategoryCacheItem category)
    {
        if (category == null)
            return false;

        if (category.Visibility == CategoryVisibility.All)
            return true;

        if (category.Visibility == CategoryVisibility.Owner && category.Creator == user)
            return true;

        return false;
    }

    public static bool CanView(User creator, CategoryVisibility visibility)
    {
        if (visibility == CategoryVisibility.All)
            return true;

        if (visibility == CategoryVisibility.Owner && creator == Sl.SessionUser.User)
            return true;

        return false;
    }

    public static bool CanView(User creator, CategoryVisibility previousVisibility,
        CategoryVisibility selectedVisibility)
    {
        return CanView(creator, previousVisibility) && CanView(creator, selectedVisibility);
    }

    public static bool CanEditCategory(int id) => CanEdit(EntityCache.GetCategoryCacheItem(id));
    public static bool CanEdit(Category category) => CanEdit(EntityCache.GetCategoryCacheItem(category.Id));
    public static bool CanEdit(CategoryCacheItem category) => CanEdit(Sl.SessionUser.User, category);
    public static bool CanEdit(User user, CategoryCacheItem category)
    {
        if (user == null || category == null)
            return false;

        if (RootCategory.LockedCategory(category.Id) && !user.IsInstallationAdmin)
            return false;

        if (Sl.SessionUser.IsLoggedIn)
            return true;

        return false;
    }

    public static bool CanDelete(Category category) => CanEdit(EntityCache.GetCategoryCacheItem(category.Id));
    public static bool CanDelete(CategoryCacheItem category) => CanDelete(Sl.SessionUser.User, category);
    public static bool CanDelete(User user, CategoryCacheItem category)
    {
        if (user == null || category == null)
            return false;

        if (category.IsStartPage())
            return false;

        if (category.Creator == user || user.IsInstallationAdmin)
            return true;

        return false;
    }

    public static bool CanView(Question question) => CanView(Sl.SessionUser.User, question);

    public static bool CanView(User user, Question question)
    {
        if (question == null)
            return false;

        if (question.Visibility == QuestionVisibility.All)
            return true;

        if (question.Visibility == QuestionVisibility.Owner && question.Creator == user)
            return true;

        return false;
    }

    public static bool CanEdit(Question question) => CanEdit(Sl.SessionUser.User, question);

    public static bool CanEdit(User user, Question question)
    {
        if (user == null || question == null)
            return false;

        if (Sl.SessionUser.IsLoggedIn)
            return true;

        return false;
    }

    public static bool CanDelete(Question question) => CanDelete(Sl.SessionUser.User, question);

    public static bool CanDelete(User user, Question question)
    {
        if (user == null || question == null)
            return false;

        if (question.Creator == user || user.IsInstallationAdmin)
            return true;

        return false;
    }
    public static bool IsAuthorOrAdmin(UserTinyModel author)
    {
        if (author == null)
            return IsAuthorOrAdmin((int?)null);
        return IsAuthorOrAdmin(author.Id);
    }
    public static bool IsAuthorOrAdmin(int? creatorId)
    {
        return Sl.SessionUser.IsInstallationAdmin || Sl.SessionUser.UserId == creatorId;
    }
}