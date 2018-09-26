using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

public class CategoryHistoryDetailModel : BaseModel
{
    public int CategoryId;
    public string CategoryName;
    public CategoryChange CurrentChange;
    public CategoryChange PrevChange;
    public string CurrentData;
    public string PrevData;
    public string AuthorName;
    public string AuthorImageUrl;

    public CategoryHistoryDetailModel(CategoryChange currentChange, CategoryChange prevChange)
    {
        CurrentChange = currentChange;
        PrevChange = prevChange;

        var currentCatRevision = currentChange.GetCategoryChangeData();
        var prevCatRevision = prevChange?.GetCategoryChangeData();
        CurrentData = currentCatRevision.TopicMardkown?.Replace("\\r\\n", "\r\n");
        PrevData = prevCatRevision?.TopicMardkown?.Replace("\\r\\n", "\r\n");

        CategoryId = currentChange.Category.Id;
        CategoryName = currentChange.Category.Name;
        AuthorName = currentChange.Author.Name;
        AuthorImageUrl = new UserImageSettings(currentChange.Author.Id).GetUrl_85px_square(currentChange.Author).Url;
    }
}

