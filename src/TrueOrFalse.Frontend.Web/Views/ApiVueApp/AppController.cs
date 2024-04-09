﻿using System.Linq;
using Antlr.Runtime;
using Microsoft.AspNetCore.Mvc;
using NHibernate.Linq.Functions;
using VueApp;
using static AppController;

public class AppController : BaseController
{
    private readonly VueSessionUser _vueSessionUser;

    public AppController(SessionUser sessionUser, VueSessionUser vueSessionUser) : base(sessionUser)
    {
        _vueSessionUser = vueSessionUser;
    }


    //todo: (Jun)
    public readonly record struct CurrentUserJson(VueSessionUser CurrentSessionUser);
    [HttpGet]
    public CurrentUserJson GetCurrentUser()
    {
        return new(_vueSessionUser.GetCurrentUserData());
    }

    //todo: (Jun)
    public readonly record struct FooterTopicsJson(
        TinyTopicItem RootWiki,
        TinyTopicItem[] MainTopics,
        TinyTopicItem MemoWiki,
        TinyTopicItem[] MemoTopics,
        TinyTopicItem[] HelpTopics,
        TinyTopicItem[] PopularTopics,
        TinyTopicItem Documentation);

    public readonly record struct TinyTopicItem(int Id, string Name);

    [HttpGet]
    public FooterTopicsJson GetFooterTopics()
    {
        var footerTopics = new FooterTopicsJson
        (
            RootWiki: new TinyTopicItem
            (
                Id: RootCategory.RootCategoryId,
                Name: EntityCache.GetCategory(RootCategory.RootCategoryId)?.Name
            ),
            MainTopics: RootCategory.MainCategoryIds.Select(id => new TinyTopicItem(
                Id: id,
                Name: EntityCache.GetCategory(id).Name
            )).ToArray(),
            MemoWiki: new TinyTopicItem
            (
                Id: RootCategory.MemuchoWikiId,
                Name: EntityCache.GetCategory(RootCategory.MemuchoWikiId).Name
            ),
            MemoTopics: RootCategory.MemuchoCategoryIds.Select(id => new TinyTopicItem(
                Id: id,
                Name: EntityCache.GetCategory(id).Name
            )).ToArray(),
            HelpTopics: RootCategory.MemuchoHelpIds.Select(id => new TinyTopicItem(
            
                Id: id,
                Name: EntityCache.GetCategory(id).Name
            )).ToArray(),
            PopularTopics: RootCategory.PopularCategoryIds.Select(id => new TinyTopicItem(
                Id: id,
                Name: EntityCache.GetCategory(id).Name
            )).ToArray(),
            Documentation: new TinyTopicItem(
            
                Id: RootCategory.IntroCategoryId,
                Name: EntityCache.GetCategory(RootCategory.IntroCategoryId).Name
            )
        );
        return footerTopics;
    }
}