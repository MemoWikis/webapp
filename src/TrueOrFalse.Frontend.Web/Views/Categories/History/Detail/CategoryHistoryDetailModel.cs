using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AngleSharp.Html;
using AngleSharp.Html.Parser;
using FluentNHibernate.Conventions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using TrueOrFalse.Frontend.Web.Code;

public class CategoryHistoryDetailModel
{
    private readonly PermissionCheck _permissionCheck;
    private readonly PageChangeRepo _pageChangeRepo;
    private readonly PageRepository _pageRepository;
    public int CategoryId;
    public string CategoryName;
    public string CategoryUrl;

    public bool PrevRevExists;
    public bool NextRevExists;

    public UserTinyModel Author;
    public string AuthorName;
    public string AuthorImageUrl;
    public bool ImageWasUpdated;
    public ImageFrontendData ImageFrontendData;

    public int CurrentId;
    public string CurrentName;
    public DateTime CurrentDateCreated;
    public string CurrentMarkdown;
    public string CurrentContent;
    public string CurrentSegments;
    public string CurrentDescription;
    public string CurrentWikipediaUrl;
    public string CurrentRelations;
    public PageVisibility CurrentVisibility;

    public string PrevName;
    public string PrevMarkdown;
    public string PrevContent;
    public string PrevSegments;
    public string PrevDescription;
    public string PrevWikipediaUrl;
    public string PrevRelations;
    public PageVisibility PrevVisibility;

    public PageChangeType ChangeType;

    public CategoryHistoryDetailModel(
        PageChange currentRevision,
        PageChange previousRevision,
        PageChange nextRevision,
        bool isCategoryDeleted,
        PermissionCheck permissionCheck,
        PageChangeRepo pageChangeRepo,
        PageRepository pageRepository,
        ImageMetaDataReadingRepo imageMetaDataReadingRepo,
        IHttpContextAccessor httpContextAccessor,
        IActionContextAccessor actionContextAccessor,
        QuestionReadingRepo questionReadingRepo
    )
    {
        _permissionCheck = permissionCheck;
        _pageChangeRepo = pageChangeRepo;
        _pageRepository = pageRepository;
        var httpContextAccessor1 = httpContextAccessor;
        ChangeType = currentRevision.Type;
        var currentVersionTypeDelete = currentRevision.Type == PageChangeType.Delete;

        PrevRevExists = previousRevision != null;
        NextRevExists = nextRevision != null;

        var previousRevisionData = !PrevRevExists ? null : previousRevision.GetCategoryChangeData();
        var currentRevisionData = currentRevision.GetCategoryChangeData();
        currentRevisionData = currentVersionTypeDelete
            ? new PageEditData_V2(_pageRepository)
            : currentRevisionData;

        CategoryId = currentRevision.Page == null
            ? _pageChangeRepo.GetCategoryId(currentRevision.Id)
            : currentRevision.Page.Id;

        if (currentVersionTypeDelete) // is currentVersion deleted then is too category deleted
            CategoryName = previousRevisionData.Name;
        // is category deleted  then currentversion type delete is not necessarily
        else if (isCategoryDeleted)
            CategoryName = currentRevisionData.Name;
        else
            CategoryName = currentRevision.Page.Name;

        var author =
            new UserTinyModel(currentRevision.Author());

        Author = author;
        AuthorName = author.Name;
        AuthorImageUrl = new UserImageSettings(author.Id, httpContextAccessor1)
            .GetUrl_85px_square(author).Url;

        CategoryUrl = isCategoryDeleted
            ? ""
            : new Links(actionContextAccessor, httpContextAccessor1)
                .CategoryDetail(CategoryName, CategoryId);

        CurrentId = currentRevision.Id;
        CurrentDateCreated = currentRevision.DateCreated;
        CurrentName = currentVersionTypeDelete
            ? previousRevisionData.Name
            : currentRevisionData.Name;
        CurrentMarkdown = currentRevisionData.TopicMardkown?.Replace("\\r\\n", "\r\n");
        CurrentContent = FormatHtmlString(currentRevisionData.Content);
        CurrentSegments = currentRevisionData.CustomSegments;
        CurrentDescription = currentRevisionData.Description?.Replace("\\r\\n", "\r\n");
        CurrentWikipediaUrl = currentVersionTypeDelete ? "" : currentRevisionData.WikipediaURL;
        CurrentVisibility = currentRevisionData.Visibility;

        if (currentRevision.DataVersion == 2)
        {
            ImageWasUpdated = ((PageEditData_V2)currentRevisionData).ImageWasUpdated;
            var imageMetaData = imageMetaDataReadingRepo.GetBy(CategoryId, ImageType.Page);
            ImageFrontendData = new ImageFrontendData(imageMetaData,
                httpContextAccessor1,
                questionReadingRepo);
        }

        if (PrevRevExists)
        {
            var prevRevisionData = previousRevision.GetCategoryChangeData();
            PrevName = prevRevisionData?.Name;
            PrevMarkdown = prevRevisionData?.TopicMardkown?.Replace("\\r\\n", "\r\n");
            PrevContent = prevRevisionData != null
                ? FormatHtmlString(prevRevisionData?.Content)
                : null;
            PrevSegments = prevRevisionData?.CustomSegments;
            PrevDescription = prevRevisionData?.Description?.Replace("\\r\\n", "\r\n");
            PrevWikipediaUrl = prevRevisionData?.WikipediaURL;
            PrevVisibility = prevRevisionData != null
                ? prevRevisionData.Visibility
                : PageVisibility.Owner;

            if (currentRevision.DataVersion >= 2 && previousRevision.DataVersion >= 2)
            {
                var currentRelationsList = ((PageEditData_V2)currentRevisionData)
                    .CategoryRelations.Where(cr =>
                        CrIsVisibleToCurrentUser(cr.PageId, cr.RelatedPageId)).ToList();
                var prevRelationsList = ((PageEditData_V2)prevRevisionData).CategoryRelations
                    .Where(cr => CrIsVisibleToCurrentUser(cr.PageId, cr.RelatedPageId))
                    .ToList();

                CurrentRelations = SortedListOfRelations(currentRelationsList);
                PrevRelations = SortedListOfRelations(prevRelationsList);
            }
        }
    }

    public static string FormatHtmlString(string unformatted)
    {
        if (String.IsNullOrEmpty(unformatted))
            return "";

        var parser = new HtmlParser();
        var document = parser.ParseDocument("<asplaceholder>" + unformatted + "</asplaceholder>");
        var body = document.QuerySelector("asplaceholder");
        var sw = new StringWriter();
        body.ToHtml(sw, new PrettyMarkupFormatter());

        var formatted = sw.ToString()
            .Replace("<asplaceholder>", "")
            .Replace("</asplaceholder>", "");

        return formatted;
    }

    private bool CrIsVisibleToCurrentUser(int categoryId, int relatedCategoryId)
    {
        PageCacheItem page = null;
        PageCacheItem relatedPage = null;

        try
        {
            page = EntityCache.GetPage(categoryId);
            relatedPage = EntityCache.GetPage(relatedCategoryId);
        }
        catch (Exception e)
        {
            Logg.Error(e);
        }

        if (page != null && relatedPage != null)
            return _permissionCheck.CanView(page) && _permissionCheck.CanView(relatedPage);

        return false;
    }

    private string Relation2String(PageRelation_EditData relation)
    {
        var relatedCategory = _pageRepository.GetById(relation.RelatedPageId);
        var isRelatedCategoryNull = relatedCategory == null;

        string name;
        if (isRelatedCategoryNull) // then is category deleted
        {
            var prevVersion = _pageChangeRepo.GetForCategory(relation.RelatedPageId)
                .Where(cc => cc.Type != PageChangeType.Delete)
                .OrderByDescending(cc => cc.DateCreated)
                .Select(cc => PageEditData_V2.CreateFromJson(cc.Data)).First();
            name = prevVersion.Name;
        }
        else
        {
            name = relatedCategory.Name;
        }

        string res;

        res = $"\"{name}\" (hat undefinierte Beziehung)";
        return res;
    }

    private string SortedListOfRelations(IList<PageRelation_EditData_V2> relations)
    {
        string res = "";
        if (relations != null && relations.IsNotEmpty())
        {
            var parents = relations
                .ToList();

            res += "Übergeordnete Themen\n";
            res += parents.IsEmpty()
                ? "<keine>"
                : string.Join("\n", parents.Select(Relation2String));

            var children = relations
                .ToList();

            res += "\n\nUntergeordnete Themen\n";
            res += children.IsEmpty()
                ? "<keine>"
                : string.Join("\n", children.Select(Relation2String));

            var otherRelations = relations;
            res += "\n\nAndere Beziehungsdaten\n";
            res += otherRelations.IsEmpty()
                ? "<keine>"
                : string.Join("\n", children.Select(Relation2String));
        }

        return res;
    }
}