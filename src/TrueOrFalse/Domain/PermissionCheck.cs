
public class PermissionCheck : IRegisterAsInstancePerLifetime
{
    private readonly int _userId;
    private readonly bool _isInstallationAdmin;

    public PermissionCheck(SessionUser sessionUser)
    {
        _userId = sessionUser.SessionIsActive() ? sessionUser.UserId : default;
        _isInstallationAdmin = sessionUser.SessionIsActive() && sessionUser.IsInstallationAdmin;
    }

    public PermissionCheck(UserCacheItem userCacheItem)
    {
        _userId = userCacheItem.Id;
        _isInstallationAdmin = userCacheItem.IsInstallationAdmin;
    }

        public PermissionCheck(int userId)
    {
        var userCacheItem = EntityCache.GetUserById(userId); 
        _userId = userCacheItem.Id;
        _isInstallationAdmin = userCacheItem.IsInstallationAdmin;
    }

    //setter is for tests
    public bool CanViewCategory(int id) => CanView(EntityCache.GetCategory(id));
    public bool CanView(Category category) => CanView(EntityCache.GetCategory(category.Id));
    public bool CanView(CategoryCacheItem? category) => CanView(_userId, category);

    public bool CanView(int userId, CategoryCacheItem? category)
    {
        if (category == null)
            return false;

        if (category.Visibility == CategoryVisibility.All)
            return true;

        if (category.Visibility == CategoryVisibility.Owner && category.CreatorId == userId)
            return true;

        return false;
    }

    public bool CanView(int creatorId, CategoryVisibility visibility)
    {
        if (visibility == CategoryVisibility.All)
            return true;

        if (visibility == CategoryVisibility.Owner && creatorId == _userId)
            return true;

        return false;
    }

    public bool CanEditCategory(int categoryId) => CanEdit(EntityCache.GetCategory(categoryId));
    public bool CanEdit(Category category) => CanEdit(EntityCache.GetCategory(category.Id));

    public bool CanView(CategoryChange change)
    {
        return change.Category != null &&
               change.Category.Id > 0 &&
               CanView(change.Category) &&
               CanView(change.Category.Creator.Id, change.GetCategoryChangeData().Visibility);
    }

    public bool CanEdit(CategoryCacheItem category)
    {
        if (_userId == default)
            return false;

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
        if (_userId == default || category == null || category.Id == 0)
            return false;

        if (category.IsStartPage())
            return false;

        if (category.Creator.Id == _userId || _isInstallationAdmin)
            return true;

        return false;
    }

    public bool CanDelete(Category category)
    {
        if (_userId == default || category == null || category.Id == 0)
            return false;

        if (category.Id == RootCategory.RootCategoryId || category.Id == category.Creator.StartTopicId)
            return false;

        if (category.Creator.Id == _userId || _isInstallationAdmin)
            return true;

        return false;
    }

    public bool CanMoveTopic(int topicId, int oldParentId, int newParentId) => CanMoveTopic(
        EntityCache.GetCategory(topicId), EntityCache.GetCategory(oldParentId), newParentId);

    public bool CanMoveTopic(CategoryCacheItem? movingTopic, CategoryCacheItem? oldParent, int newParentId)
    {   
        if (_userId == default
            || movingTopic == null
            || movingTopic.Id == 0
            || oldParent == null
            || oldParent.Id == 0)
            return false;

        if (RootCategory.RootCategoryId == newParentId && !_isInstallationAdmin && movingTopic.Visibility == CategoryVisibility.All)
            return false;

        return _isInstallationAdmin || movingTopic.CreatorId == _userId || oldParent.CreatorId == _userId;
    }

    public bool CanViewQuestion(int id) => CanView(EntityCache.GetQuestion(id));

    public bool CanView(QuestionCacheItem question)
    {
        if (question == null || question.Id == 0)
            return false;

        if (question.Visibility == QuestionVisibility.All)
            return true;

        if (question.Visibility == QuestionVisibility.Owner && question.CreatorId == _userId)
            return true;

        return false;
    }

    public bool CanEdit(QuestionCacheItem question)
    {
        if (_userId == default)
            return false;

        if (question == null)
            return false;

        if (question.IsCreator(_userId) || _isInstallationAdmin)
            return false;

        return false;
    }

    public bool CanDelete(Question question)
    {
        if (_userId == default)
            return false;

        if (question == null)
            return false;

        if (question.IsCreator(_userId) || _isInstallationAdmin)
            return true;

        return false;
    } }