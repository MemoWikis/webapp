using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Conventions;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;

public class CategoryHistoryDetailModel : BaseModel
{
    public int CategoryId;
    public string CategoryName;
    public CategoryChange CurrentChange;
    public CategoryChange PrevChange;
    public string CurrentName;
    public string PrevName;
    public string CurrentMarkdown;
    public string PrevMarkdown;
    public string CurrentDescription;
    public string PrevDescription;
    public string CurrentWikipediaUrl;
    public string PrevWikipediaUrl;
    public string CurrentRelations;
    public string PrevRelations;
    public string AuthorName;
    public string AuthorImageUrl;

    public CategoryHistoryDetailModel(CategoryChange currentChange, CategoryChange prevChange)
    {
        CurrentChange = currentChange;
        PrevChange = prevChange;

        var currentRevisionData = currentChange.GetCategoryChangeData();
        CurrentName = currentRevisionData.Name;
        CurrentMarkdown = currentRevisionData.TopicMardkown?.Replace("\\r\\n", "\r\n");
        CurrentDescription = currentRevisionData.Description?.Replace("\\r\\n", "\r\n");
        CurrentWikipediaUrl = currentRevisionData.WikipediaURL;

        if (PrevChange != null)
        {
            var prevRevisionData = PrevChange.GetCategoryChangeData();
            PrevName = prevRevisionData?.Name;
            PrevMarkdown = prevRevisionData?.TopicMardkown?.Replace("\\r\\n", "\r\n");
            PrevDescription = prevRevisionData?.Description?.Replace("\\r\\n", "\r\n");
            PrevWikipediaUrl = prevRevisionData?.WikipediaURL;

            if (currentChange.DataVersion >= 2 && prevChange.DataVersion >= 2)
            {
                var currentRelationsList = ((CategoryEditData_V2)currentRevisionData).CategoryRelations;
                var prevRelationsList = ((CategoryEditData_V2)prevRevisionData).CategoryRelations;

                CurrentRelations = SortedListOfRelations(currentRelationsList);
                PrevRelations = SortedListOfRelations(prevRelationsList);
            }
        }

        CategoryId = currentChange.Category.Id;
        CategoryName = currentChange.Category.Name;
        AuthorName = currentChange.Author.Name;
        AuthorImageUrl = new UserImageSettings(currentChange.Author.Id).GetUrl_85px_square(currentChange.Author).Url;
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

