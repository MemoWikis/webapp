﻿public class CategoryUpdater(
    CategoryRepository _categoryRepository,
    PermissionCheck permissionCheck) : IRegisterAsInstancePerLifetime
{
    public bool HideOrShowTopicText(bool hideText, int categoryId)
    {
        var cacheTopic = EntityCache.GetCategory(categoryId);
        if (cacheTopic == null)
            throw new NullReferenceException($"{nameof(HideOrShowTopicText)}: topicCacheItem is null");

        //if (cacheTopic.Content?.Length > 0)
        //    throw new AccessViolationException($"{nameof(HideOrShowTopicText)}: topicCacheItem has content");

        if (permissionCheck.CanView(cacheTopic) == false)
            throw new AccessViolationException($"{nameof(HideOrShowTopicText)}: No permission for user");

        cacheTopic.TextIsHidden = hideText;
        EntityCache.AddOrUpdate(cacheTopic);

        var dbTopic = _categoryRepository.GetById(categoryId);
        dbTopic.TextIsHidden = hideText;
        _categoryRepository.BaseUpdate(dbTopic);

        return cacheTopic.TextIsHidden;
    }
}

