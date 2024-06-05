public class CategoryUpdater(
    CategoryRepository _categoryRepository,
    PermissionCheck permissionCheck) : IRegisterAsInstancePerLifetime
{
    public bool HideOrShowTopicText(bool isHideText, int categoryId)
    {
        var cacheTopic = EntityCache.GetCategory(categoryId);
        if (permissionCheck.CanView(cacheTopic) == false)
            return false;

        cacheTopic.IsHideText = isHideText;
        EntityCache.AddOrUpdate(cacheTopic);

        var DbTopic = _categoryRepository.GetById(categoryId);
        DbTopic.IsHideText = isHideText;
        _categoryRepository.BaseUpdate(DbTopic);

        return true;
    }
}

