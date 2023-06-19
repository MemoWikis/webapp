using System.Linq;

public class PermissionCheck : IRegisterAsInstancePerLifetime
{
    //private readonly SessionUser _sessionUser;

    private readonly int _userId;
    private readonly bool _isInstallationAdmin;

    public PermissionCheck(SessionUser sessionUser)
    {
        _userId = sessionUser.UserId;
        _isInstallationAdmin = sessionUser.IsInstallationAdmin;
    }

    public PermissionCheck(UserCacheItem userCacheItem)
    {
        _userId = userCacheItem.Id;
        _isInstallationAdmin = userCacheItem.IsInstallationAdmin;
    }

    public static PermissionCheck Instance(int userId)
    {
        return new PermissionCheck(EntityCache.GetUserById(userId));
    }

    //setter is for tests
    public bool CanViewCategory(int id) => CanView(EntityCache.GetCategory(id));
    public  bool CanView(Category category) => CanView(EntityCache.GetCategory(category.Id));
    public bool CanView(CategoryCacheItem category) => CanView(_userId, category);

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

    public  bool CanEditCategory(int categoryId) => CanEdit(EntityCache.GetCategory(categoryId));
    public  bool CanEdit(Category category) => CanEdit(EntityCache.GetCategory(category.Id));

    public  bool CanEdit(CategoryCacheItem category)
    {
        if (category == null)
            return false;

        if (RootCategory.LockedCategory(category.Id) && !_isInstallationAdmin)
            return false;

        if (!CanView(category))
            return false;

        return true;
    }

    public bool CanDelete(CategoryCacheItem category)
    {
        if (category == null)
            return false;

        if (category.IsStartPage())
            return false;

        if (category.Creator.Id == _userId || _isInstallationAdmin)
            return true;

        return false;
    }

    public bool CanViewQuestion(int id) => CanView(EntityCache.GetQuestion(id));

    public bool CanView(QuestionCacheItem question)
    {
        if (question == null)
            return false;

        if (question.Visibility == QuestionVisibility.All)
            return true;

        if (question.Visibility == QuestionVisibility.Owner && question.Creator.Id == _userId)
            return true;

        return false;
    }

    public bool CanEdit(Question question)
    {
        if (question == null)
            return false;

        if (question.IsCreator(_userId) || _isInstallationAdmin)
            return false;

        return false;
    }

    public bool CanEdit(QuestionCacheItem question)
    {
        if (question == null)
            return false;

        if (question.IsCreator(_userId) || _isInstallationAdmin)
            return false;

        return false;
    }

    public bool CanDelete(Question question)
    {
        if (question == null)
            return false;

        if (question.IsCreator(_userId) || _isInstallationAdmin)
            return true;

        return false;
    }
}