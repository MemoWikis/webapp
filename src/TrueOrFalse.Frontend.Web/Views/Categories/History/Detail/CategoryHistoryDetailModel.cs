﻿using System;
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
    private readonly CategoryChangeRepo _categoryChangeRepo;
    private readonly CategoryRepository _categoryRepository;
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
    public CategoryVisibility CurrentVisibility;

    public string PrevName;
    public string PrevMarkdown;
    public string PrevContent;
    public string PrevSegments;
    public string PrevDescription;
    public string PrevWikipediaUrl;
    public string PrevRelations;
    public CategoryVisibility PrevVisibility;

    public CategoryChangeType ChangeType;

    public CategoryHistoryDetailModel(
        CategoryChange currentRevision,
        CategoryChange previousRevision,
        CategoryChange nextRevision,
        bool isCategoryDeleted,
        PermissionCheck permissionCheck,
        CategoryChangeRepo categoryChangeRepo,
        CategoryRepository categoryRepository,
        ImageMetaDataReadingRepo imageMetaDataReadingRepo,
        IHttpContextAccessor httpContextAccessor,
        IActionContextAccessor actionContextAccessor,
        QuestionReadingRepo questionReadingRepo
    )
    {
        _permissionCheck = permissionCheck;
        _categoryChangeRepo = categoryChangeRepo;
        _categoryRepository = categoryRepository;
        var httpContextAccessor1 = httpContextAccessor;
        ChangeType = currentRevision.Type;
        var currentVersionTypeDelete = currentRevision.Type == CategoryChangeType.Delete;

        PrevRevExists = previousRevision != null;
        NextRevExists = nextRevision != null;

        var previousRevisionData = !PrevRevExists ? null : previousRevision.GetCategoryChangeData();
        var currentRevisionData = currentRevision.GetCategoryChangeData();
        currentRevisionData = currentVersionTypeDelete
            ? new CategoryEditData_V2(_categoryRepository)
            : currentRevisionData;

        CategoryId = currentRevision.Category == null
            ? _categoryChangeRepo.GetCategoryId(currentRevision.Id)
            : currentRevision.Category.Id;

        if (currentVersionTypeDelete) // is currentVersion deleted then is too category deleted
            CategoryName = previousRevisionData.Name;
        // is category deleted  then currentversion type delete is not necessarily
        else if (isCategoryDeleted)
            CategoryName = currentRevisionData.Name;
        else
            CategoryName = currentRevision.Category.Name;

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
            ImageWasUpdated = ((CategoryEditData_V2)currentRevisionData).ImageWasUpdated;
            var imageMetaData = imageMetaDataReadingRepo.GetBy(CategoryId, ImageType.Category);
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
                : CategoryVisibility.Owner;

            if (currentRevision.DataVersion >= 2 && previousRevision.DataVersion >= 2)
            {
                var currentRelationsList = ((CategoryEditData_V2)currentRevisionData)
                    .CategoryRelations.Where(cr =>
                        CrIsVisibleToCurrentUser(cr.CategoryId, cr.RelatedCategoryId)).ToList();
                var prevRelationsList = ((CategoryEditData_V2)prevRevisionData).CategoryRelations
                    .Where(cr => CrIsVisibleToCurrentUser(cr.CategoryId, cr.RelatedCategoryId))
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
        CategoryCacheItem category = null;
        CategoryCacheItem relatedCategory = null;

        try
        {
            category = EntityCache.GetCategory(categoryId);
            relatedCategory = EntityCache.GetCategory(relatedCategoryId);
        }
        catch (Exception e)
        {
            Logg.Error(e);
        }

        if (category != null && relatedCategory != null)
            return _permissionCheck.CanView(category) && _permissionCheck.CanView(relatedCategory);

        return false;
    }

    private string Relation2String(CategoryRelation_EditData relation)
    {
        var relatedCategory = _categoryRepository.GetById(relation.RelatedCategoryId);
        var isRelatedCategoryNull = relatedCategory == null;

        string name;
        if (isRelatedCategoryNull) // then is category deleted
        {
            var prevVersion = _categoryChangeRepo.GetForCategory(relation.RelatedCategoryId)
                .Where(cc => cc.Type != CategoryChangeType.Delete)
                .OrderByDescending(cc => cc.DateCreated)
                .Select(cc => CategoryEditData_V2.CreateFromJson(cc.Data)).First();
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

    private string SortedListOfRelations(IList<CategoryRelation_EditData_V2> relations)
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