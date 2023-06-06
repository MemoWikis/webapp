using System.Linq;

public class PermissionCheck
{
    //setter is for tests
    public static bool CanViewCategory(int id) => CanView(EntityCache.GetCategory(id));
    public static bool CanView(Category category) => CanView(EntityCache.GetCategory(category.Id));
    public static bool CanView(CategoryCacheItem category) => CanView(SessionUserLegacy.UserId, category);

    public static bool CanView(int userId, CategoryCacheItem category)
    {
        if (category == null)
            return false;

        if (category.Visibility == CategoryVisibility.All)
            return true;

        if (category.Visibility == CategoryVisibility.Owner && category.CreatorId == userId)
            return true;

        return false;
    }

    public static bool CanView(int creatorId, CategoryVisibility visibility)
    {
        if (visibility == CategoryVisibility.All)
            return true;

        if (visibility == CategoryVisibility.Owner && creatorId == SessionUserLegacy.UserId)
            return true;

        return false;
    }

    public static bool CanView(int creatorId, CategoryVisibility previousVisibility,
        CategoryVisibility selectedVisibility)
    {
        return CanView(creatorId, previousVisibility) && CanView(creatorId, selectedVisibility);
    }

    public static bool CanEditCategory(int id) => CanEdit(EntityCache.GetCategory(id));
    public static bool CanEdit(Category category) => CanEdit(EntityCache.GetCategory(category.Id));
    public static bool CanEdit(CategoryCacheItem category) => CanEdit(SessionUserLegacy.User, category);
    public static bool CanEdit(SessionUserCacheItem user, CategoryCacheItem category)
    {
        if (user == null || category == null)
            return false;

        if (RootCategory.LockedCategory(category.Id) && !user.IsInstallationAdmin)
            return false;

        if (!CanView(category))
            return false;

        return SessionUserLegacy.IsLoggedIn;
    }

    public static bool CanDelete(Category category) => CanEdit(EntityCache.GetCategory(category.Id));
    public static bool CanDelete(CategoryCacheItem category) => CanDelete(SessionUserLegacy.User, category);
    public static bool CanDelete(SessionUserCacheItem user, CategoryCacheItem category)
    {
        if (user == null || category == null)
            return false;

        if (category.IsStartPage())
            return false;

        if (category.Creator.Id == user.Id || user.IsInstallationAdmin)
            return true;

        return false;
    }

    public static bool CanViewQuestion(int id) => CanView(EntityCache.GetQuestion(id));

    public static bool CanView(QuestionCacheItem question) => CanView(SessionUserLegacy.UserId, question);

    public static bool CanView(int userId, QuestionCacheItem question)
    {
        if (question == null)
            return false;

        if (question.Visibility == QuestionVisibility.All)
            return true;

        if (question.Visibility == QuestionVisibility.Owner && question.Creator.Id == userId)
            return true;

        return false;
    }



    public static bool CanEdit(Question question) => CanEdit(SessionUserLegacy.User, question);

    public static bool CanEdit(SessionUserCacheItem user, Question question)
    {
        if (user == null || question == null)
            return false;

        return SessionUserLegacy.IsLoggedIn;
    }
    public static bool CanEdit(QuestionCacheItem question) => CanEdit(SessionUserLegacy.User, question);

    public static bool CanEdit(SessionUserCacheItem user, QuestionCacheItem question)
    {
        if (user == null || question == null)
            return false;

        return SessionUserLegacy.IsLoggedIn;
    }

    public static bool CanDelete(Question question) => CanDelete(SessionUserLegacy.User, question);

    public static bool CanDelete(SessionUserCacheItem user, Question question)
    {
        if (user == null || question == null)
            return false;

        if (question.Creator?.Id == user.Id || user.IsInstallationAdmin)
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
        return SessionUserLegacy.IsInstallationAdmin || SessionUserLegacy.UserId == creatorId;
    }
}