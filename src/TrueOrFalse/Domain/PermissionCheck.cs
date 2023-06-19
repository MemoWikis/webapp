using System.Linq;

public class PermissionCheck : IRegisterAsInstancePerLifetime
{
    private readonly SessionUser _sessionUser;

    public PermissionCheck(SessionUser sessionUser)
    {
        _sessionUser = sessionUser;
    }
    //setter is for tests
    public bool CanViewCategory(int id) => CanView(EntityCache.GetCategory(id));
    public  bool CanView(Category category) => CanView(EntityCache.GetCategory(category.Id));
    public bool CanView(CategoryCacheItem category) => CanView(_sessionUser.UserId, category);

    public bool CanView(int userId, CategoryCacheItem category)
    {
        if (category == null)
            return false;

        if (category.Visibility == CategoryVisibility.All)
            return true;

        if (category.Visibility == CategoryVisibility.Owner && category.CreatorId == userId)
            return true;

        return false;
    }

    public  bool CanEditCategory(int id) => CanEdit(EntityCache.GetCategory(id));
    public  bool CanEdit(Category category) => CanEdit(EntityCache.GetCategory(category.Id));
    public  bool CanEdit(CategoryCacheItem category) => CanEdit(_sessionUser, category);
    public  bool CanEdit(SessionUser user, CategoryCacheItem category)
    {
        if (user == null || category == null)
            return false;

        if (RootCategory.LockedCategory(category.Id) && !user.IsInstallationAdmin)
            return false;

        if (!CanView(category))
            return false;

        return _sessionUser.IsLoggedIn;
    }

    public bool CanDelete(CategoryCacheItem category) => CanDelete(_sessionUser.User, category);
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

    public bool CanViewQuestion(int id) => CanView(EntityCache.GetQuestion(id));

    public bool CanView(QuestionCacheItem question) => CanView(_sessionUser.UserId, question);

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

    public bool CanEdit(Question question) => CanEdit(_sessionUser.User, question);

    public  bool CanEdit(SessionUserCacheItem user, Question question)
    {
        if (user == null || question == null)
            return false;

        return _sessionUser.IsLoggedIn;
    }
    public  bool CanEdit(QuestionCacheItem question) => CanEdit(_sessionUser.User, question);

    public bool CanEdit(SessionUserCacheItem user, QuestionCacheItem question)
    {
        if (user == null || question == null)
            return false;

        return _sessionUser.IsLoggedIn;
    }

    public bool CanDelete(Question question) => CanDelete(_sessionUser.User, question);

    public static bool CanDelete(SessionUserCacheItem user, Question question)
    {
        if (user == null || question == null)
            return false;

        if (question.Creator?.Id == user.Id || user.IsInstallationAdmin)
            return true;

        return false;
    }
}