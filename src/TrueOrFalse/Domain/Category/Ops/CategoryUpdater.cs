public class CategoryUpdater(
    CategoryRepository _categoryRepository,
    PermissionCheck permissionCheck) : IRegisterAsInstancePerLifetime
{
    public bool HideOrShowTopicText(bool hideText, int categoryId)
    {
        var cacheTopic = EntityCache.GetCategory(categoryId);
        if (cacheTopic == null)
            throw new NullReferenceException($"{nameof(HideOrShowTopicText)}: topicCacheItem is null");

        if (permissionCheck.CanView(cacheTopic) == false)
            throw new AccessViolationException($"{nameof(HideOrShowTopicText)}: No permission for user");

        cacheTopic.TextIsHidden = hideText;
        EntityCache.AddOrUpdate(cacheTopic);

        var DbTopic = _categoryRepository.GetById(categoryId);
        DbTopic.TextIsHidden = hideText;
        _categoryRepository.BaseUpdate(DbTopic);

        return cacheTopic.TextIsHidden;
    }
}

