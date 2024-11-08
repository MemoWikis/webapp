
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
    public bool CanViewCategory(int id) => CanView(EntityCache.GetPage(id));
    public bool CanView(Page page) => CanView(EntityCache.GetPage(page.Id));
    public bool CanView(PageCacheItem? category) => CanView(_userId, category);

    public bool CanView(int userId, PageCacheItem? category)
    {
        if (category == null)
            return false;

        if (category.Visibility == PageVisibility.All)
            return true;

        if (category.Visibility == PageVisibility.Owner && category.CreatorId == userId)
            return true;

        return false;
    }

    public bool CanView(int creatorId, PageVisibility visibility)
    {
        if (visibility == PageVisibility.All)
            return true;

        if (visibility == PageVisibility.Owner && creatorId == _userId)
            return true;

        return false;
    }

    public bool CanEditCategory(int categoryId) => CanEdit(EntityCache.GetPage(categoryId));
    public bool CanEdit(Page page) => CanEdit(EntityCache.GetPage(page.Id));

    public bool CanView(PageChange change)
    {
        return change.Page != null &&
               change.Page.Id > 0 &&
               CanView(change.Page) &&
               CanView(change.Page.Creator.Id, change.GetCategoryChangeData().Visibility);
    }

    public bool CanEdit(PageCacheItem page)
    {
        if (_userId == default)
            return false;

        if (page == null)
            return false;

        if (RootCategory.LockedCategory(page.Id) && !_isInstallationAdmin)
            return false;

        if (!CanView(page))
            return false;

        return true;
    }

    public bool CanDelete(PageCacheItem page)
    {
        if (_userId == default || page == null || page.Id == 0)
            return false;

        if (page.IsStartPage())
            return false;

        if (page.Creator.Id == _userId || _isInstallationAdmin)
            return true;

        return false;
    }

    public bool CanDelete(Page page)
    {
        if (_userId == default || page == null || page.Id == 0)
            return false;

        if (page.Id == RootCategory.RootCategoryId || page.Id == page.Creator.StartTopicId)
            return false;

        if (page.Creator.Id == _userId || _isInstallationAdmin)
            return true;

        return false;
    }

    public bool CanMoveTopic(int topicId, int oldParentId, int newParentId) => CanMoveTopic(
        EntityCache.GetPage(topicId), EntityCache.GetPage(oldParentId), newParentId);

    public bool CanMoveTopic(PageCacheItem? movingTopic, PageCacheItem? oldParent, int newParentId)
    {
        if (_userId == default
            || movingTopic == null
            || movingTopic.Id == 0
            || oldParent == null
            || oldParent.Id == 0)
            return false;

        if (RootCategory.RootCategoryId == newParentId && !_isInstallationAdmin && movingTopic.Visibility == PageVisibility.All)
            return false;

        return _isInstallationAdmin || movingTopic.CreatorId == _userId || oldParent.CreatorId == _userId;
    }

    public bool CanViewQuestion(int id) => CanView(EntityCache.GetQuestion(id));

    public bool CanView(QuestionCacheItem? question)
    {
        if (question == null || question.Id == 0)
            return false;

        if (question.Visibility == QuestionVisibility.All)
            return true;

        if (question.Visibility == QuestionVisibility.Owner && question.CreatorId == _userId)
            return true;

        return false;
    }

    public bool CanEditQuestion(int questionId) => CanEdit(EntityCache.GetQuestion(questionId));

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
    }
}