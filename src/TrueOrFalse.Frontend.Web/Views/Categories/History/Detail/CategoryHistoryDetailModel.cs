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
        
        CurrentData = currentChange.Data;
        PrevData = (prevChange != null) ? prevChange.Data : "";


        dynamic currentCatRevision = JsonConvert.DeserializeObject(currentChange.Data);
        dynamic prevCatRevision = JsonConvert.DeserializeObject(prevChange.Data);
        CurrentData = ((string) currentCatRevision.TopicMardkown).Replace("\\r\\n", "\r\n");
        PrevData = (prevChange != null) ? ((string) prevCatRevision.TopicMardkown).Replace("\\r\\n", "\r\n") : "";

        CategoryId = currentChange.Category.Id;
        CategoryName = currentChange.Category.Name;
        AuthorName = currentChange.Author.Name;
        AuthorImageUrl = new UserImageSettings(currentChange.Author.Id).GetUrl_85px_square(currentChange.Author).Url;
    }
}

