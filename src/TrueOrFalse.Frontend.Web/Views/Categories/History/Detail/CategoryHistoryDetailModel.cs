﻿using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Conventions;
using TrueOrFalse.Frontend.Web.Code;

public class CategoryHistoryDetailModel : BaseModel
{
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
    public string CurrentDescription;
    public string CurrentWikipediaUrl;
    public string CurrentRelations;

    public string PrevName;
    public DateTime PrevDateCreated;
    public string PrevMarkdown;
    public string PrevDescription;
    public string PrevWikipediaUrl;
    public string PrevRelations;

    
    public CategoryHistoryDetailModel(CategoryChange currentRevision, CategoryChange previousRevision, CategoryChange nextRevision, bool isCategoryDeleted)
    {
        var currentVersionIsDeleted = currentRevision.Type == CategoryChangeType.Delete; 

        PrevRevExists = previousRevision != null;
        NextRevExists = nextRevision != null;

        var previouisRevisionData =  previousRevision.GetCategoryChangeData();
        var currentRevisionData = currentRevision.GetCategoryChangeData();
        currentRevisionData = currentVersionIsDeleted ? new CategoryEditData_V2() : currentRevisionData;

        CategoryId = currentRevision.Category == null ? Sl.CategoryChangeRepo.GetCategoryId(currentRevision.Id):  currentRevision.Category.Id;
        CategoryName = isCategoryDeleted ? previouisRevisionData.Name :  currentRevision.Category.Name;
        Author = new UserTinyModel(currentRevision.Author);
        AuthorName = new UserTinyModel(currentRevision.Author).Name;
        AuthorImageUrl = new UserImageSettings(new UserTinyModel(currentRevision.Author).Id).GetUrl_85px_square(new UserTinyModel(currentRevision.Author)).Url;
        CategoryUrl = isCategoryDeleted ? "" : Links.CategoryDetail(CategoryName, CategoryId);

       
        CurrentId = currentRevision.Id;
        CurrentDateCreated = currentRevision.DateCreated;
        CurrentName = currentVersionIsDeleted ? previouisRevisionData.Name :  currentRevisionData.Name;
        CurrentMarkdown = currentRevisionData.TopicMardkown?.Replace("\\r\\n", "\r\n");
        CurrentDescription = currentRevisionData.Description?.Replace("\\r\\n", "\r\n");
        CurrentWikipediaUrl = currentVersionIsDeleted ? ""  : currentRevisionData.WikipediaURL;

        if (currentRevision.DataVersion == 2)
        {
            ImageWasUpdated = ((CategoryEditData_V2)currentRevisionData).ImageWasUpdated;
            var imageMetaData = Sl.ImageMetaDataRepo.GetBy(CategoryId, ImageType.Category);
            ImageFrontendData = new ImageFrontendData(imageMetaData);
        }

        if (PrevRevExists)
        {
            var prevRevisionData = previousRevision.GetCategoryChangeData();
            PrevName = prevRevisionData?.Name;
            PrevMarkdown = prevRevisionData?.TopicMardkown?.Replace("\\r\\n", "\r\n");
            PrevDescription = prevRevisionData?.Description?.Replace("\\r\\n", "\r\n");
            PrevWikipediaUrl = prevRevisionData?.WikipediaURL;

            if (currentRevision.DataVersion >= 2 && previousRevision.DataVersion >= 2)
            {
                var currentRelationsList = ((CategoryEditData_V2)currentRevisionData).CategoryRelations;
                var prevRelationsList = ((CategoryEditData_V2)prevRevisionData).CategoryRelations;

                CurrentRelations = SortedListOfRelations(currentRelationsList);
                PrevRelations = SortedListOfRelations(prevRelationsList);
            }
        }
    }

    private string Relation2String(CategoryRelation_EditData relation)
    {
        var relatedCategory = Sl.CategoryRepo.GetById(relation.RelatedCategoryId);
        string res;
        switch (relation.RelationType)
        {
            case CategoryRelationType.IsChildCategoryOf:
                res = $"\"{relatedCategory.Name}\" (ist übergeordnet)";
                break;
            case CategoryRelationType.IncludesContentOf:
                res = $"\"{relatedCategory.Name}\" (ist untergeordnet)";
                break;
            default:
                res = $"\"{relatedCategory.Name}\" (hat undefinierte Beziehung)";
                break;
        }

        return res;
    }

    private string SortedListOfRelations(IList<CategoryRelation_EditData_V2> relations)
    {
        string res = "";
        if (relations != null && relations.IsNotEmpty())
        {
            var parents = relations.Where(r => r.RelationType == CategoryRelationType.IsChildCategoryOf);
            res += "Übergeordnete Themen\n";
            res += (parents.IsEmpty())
                ? "<keine>"
                : string.Join("\n", parents.Select(Relation2String));

            var children = relations.Where(r => r.RelationType == CategoryRelationType.IncludesContentOf);
            res += "\n\nUntergeordnete Themen\n";
            res += (children.IsEmpty())
                ? "<keine>"
                : string.Join("\n", children.Select(Relation2String));

            var otherRelations = relations.Where(r => r.RelationType != CategoryRelationType.IsChildCategoryOf && r.RelationType != CategoryRelationType.IncludesContentOf);
            res += "\n\nAndere Beziehungsdaten\n";
            res += (otherRelations.IsEmpty())
                ? "<keine>"
                : string.Join("\n", children.Select(Relation2String));
        }

        return res;
    }
}

