﻿using Newtonsoft.Json;
using NHibernate;

public class PageEditDataV1 : PageEditData
{
    private readonly ISession _nhibernateSession;
    private readonly PageRepository _pageRepository;
    public IList<PageRelationEditDataV1> PageRelations;

    public PageEditDataV1(
        Page page,
        ISession nhibernateSession,
        PageRepository pageRepository)
    {
        _nhibernateSession = nhibernateSession;
        _pageRepository = pageRepository;
        Name = page.Name;
        Description = page.Description;
        PageMardkown = page.Markdown;
        Content = page.Content;
        CustomSegments = page.CustomSegments;
        WikipediaURL = page.WikipediaURL;
        DisableLearningFunctions = page.DisableLearningFunctions;
        PageRelations = null;
        Visibility = page.Visibility;
    }

    public PageEditDataV1()
    {
    }

    public override Page ToPage(int pageId)
    {
        var page = _pageRepository.GetById(pageId);

        _nhibernateSession.Evict(page);
        var pageIsNull = page == null;
        page = pageIsNull ? new Page() : page;

        page.IsHistoric = true;
        page.Name = this.Name;
        page.Description = this.Description;
        page.Markdown = this.PageMardkown;
        page.CustomSegments = this.CustomSegments;
        page.Content = this.Content;
        page.WikipediaURL = this.WikipediaURL;
        page.DisableLearningFunctions = this.DisableLearningFunctions;
        page.Visibility = this.Visibility;

        // Historic PageRelations cannot be loaded for DataVersion 1 because there
        // was a bug where data didn't get written properly so correct relation data
        // simply do not exist for V1.
        // Also they cannot be loaded because we do not have archive data and
        // loading them leads to nasty conflicts and nuisance with NHibernate.

        return page;
    }

    public override string ToJson() => JsonConvert.SerializeObject(this);

    public static PageEditDataV1 CreateFromJson(string json) =>
        JsonConvert.DeserializeObject<PageEditDataV1>(json);

    //placeholder
    public override PageCacheItem ToCachePage(int pageId) => new PageCacheItem();
}