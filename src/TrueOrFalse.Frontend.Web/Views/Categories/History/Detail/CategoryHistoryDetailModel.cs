using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
    //public string RelationsDiff;
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

            //var prevRelations = prevRevisionData.CategoryRelations;
            //RelationsDiff = string.Join(", ", currentRevisionData.CategoryRelations.Except(prevRelations)
            //    .Select(c => $"Bezugsthema: {c.RelatedCategoryId}, Beziehung: {c.RelationType}"));
        }

        CategoryId = currentChange.Category.Id;
        CategoryName = currentChange.Category.Name;
        AuthorName = currentChange.Author.Name;
        AuthorImageUrl = new UserImageSettings(currentChange.Author.Id).GetUrl_85px_square(currentChange.Author).Url;
    }
}

