using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Conventions;

public class CategoryHistoryDetailModel : BaseModel
{
    public int CategoryId;
    public string CategoryName;
    public User CurrentAuthor;
    public string AuthorName;
    public string AuthorImageUrl;
    public bool PrevRevisionExists;
    public bool NextRevExists;
    public int CurrentId;
    public string CurrentName;
    public string PrevName;
    public DateTime CurrentDateCreated;
    public DateTime PrevDateCreated;
    public string CurrentMarkdown;
    public string PrevMarkdown;
    public string CurrentDescription;
    public string PrevDescription;
    public string CurrentWikipediaUrl;
    public string PrevWikipediaUrl;
    public string CurrentRelations;
    public string PrevRelations;
    
    public CategoryHistoryDetailModel(CategoryChange currentRevision, CategoryChange previousRevision, CategoryChange nextRevision)
    {
        PrevRevisionExists = previousRevision != null;
        NextRevExists = nextRevision != null;

        var currentRevisionData = currentRevision.GetCategoryChangeData();
        CurrentId = currentRevision.Id;
        CurrentDateCreated = currentRevision.DateCreated;
        CurrentName = currentRevisionData.Name;
        CurrentMarkdown = currentRevisionData.TopicMardkown?.Replace("\\r\\n", "\r\n");
        CurrentDescription = currentRevisionData.Description?.Replace("\\r\\n", "\r\n");
        CurrentWikipediaUrl = currentRevisionData.WikipediaURL;

        if (PrevRevisionExists)
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

        CategoryId = currentRevision.Category.Id;
        CategoryName = currentRevision.Category.Name;
        CurrentAuthor = currentRevision.Author;
        AuthorName = currentRevision.Author.Name;
        AuthorImageUrl = new UserImageSettings(currentRevision.Author.Id).GetUrl_85px_square(currentRevision.Author).Url;
    }

    private string Relation2String(CategoryRelation_EditData_V2 relation)
    {
        var relatedCategory = Sl.CategoryRepo.GetById(relation.RelatedCategoryId);
        string res;
        switch (relation.RelationType)
        {
            case 1:
                res = $"\"{relatedCategory.Name}\" (ist übergeordnet)";
                break;
            case 2:
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
        if (relations.IsNotEmpty())
        {
            var parents = relations.Where(r => r.RelationType == 1);
            res += "Übergeordnete Themen\n";
            res += (parents.IsEmpty())
                ? "<keine>"
                : string.Join("\n", parents.Select(Relation2String));

            var children = relations.Where(r => r.RelationType == 2);
            res += "\n\nUntergeordnete Themen\n";
            res += (children.IsEmpty())
                ? "<keine>"
                : string.Join("\n", children.Select(Relation2String));

            var otherRelations = relations.Where(r => r.RelationType != 1 && r.RelationType != 2);
            res += "\n\nAndere Beziehungsdaten\n";
            res += (otherRelations.IsEmpty())
                ? "<keine>"
                : string.Join("\n", children.Select(Relation2String));
        }

        return res;
    }
}

