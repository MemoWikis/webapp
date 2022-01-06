﻿using System.Runtime.InteropServices.WindowsRuntime;
using System.Web.Razor.Tokenizer;

public class PermissionCheck
{
    public static bool CanViewCategory(int id) => CanView(EntityCache.GetCategoryCacheItem(id));
    public static bool CanView(Category category) => CanView(CategoryCacheItem.ToCacheCategory(category));
    public static bool CanView(CategoryCacheItem category) => CanView(Sl.R<SessionUser>().User, category);

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

    public static bool CanEditCategory(int id) => CanEdit(EntityCache.GetCategoryCacheItem(id));
    public static bool CanEdit(Category category) => CanEdit(CategoryCacheItem.ToCacheCategory(category));
    public static bool CanEdit(CategoryCacheItem category) => CanEdit(Sl.R<SessionUser>().User, category);
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

    public static bool CanDelete(Category category) => CanEdit(CategoryCacheItem.ToCacheCategory(category));
    public static bool CanDelete(CategoryCacheItem category) => CanDelete(Sl.R<SessionUser>().User, category);
    public static bool CanDelete(User user, CategoryCacheItem category)
    {
        if (user == null || category == null)
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
        if (question == null)
            return false;

        if (question.Visibility == QuestionVisibility.All)
            return true;

        if (question.Visibility == QuestionVisibility.Owner && question.Creator == user)
            return true;

        return false;
    }

    public static bool CanEdit(Question question) => CanEdit(Sl.R<SessionUser>().User, question);

    public static bool CanEdit(User user, Question question)
    {
        if (user == null || question == null)
            return false;

        if (Sl.SessionUser.IsLoggedIn)
            return true;

        return false;
    }

    public static bool CanDelete(Question question) => CanDelete(Sl.R<SessionUser>().User, question);

    public static bool CanDelete(User user, Question question)
    {
        if (user == null || question == null)
            return false;

        if (question.Creator == user || user.IsInstallationAdmin)
            return true;

        return false;
    }
}