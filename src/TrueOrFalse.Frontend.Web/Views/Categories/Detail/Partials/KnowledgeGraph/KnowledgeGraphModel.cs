using System;
using System.Web.Mvc;
using System.Web.Script.Serialization;

public class KnowledgeGraphModel : BaseModel
{
    public CategoryCacheItem CategoryCahCacheItem;
    public JsonResult GraphData;
    public string GraphDataString;

    public KnowledgeGraphModel(CategoryCacheItem categoryCahCacheItem)
    {
        CategoryCahCacheItem = categoryCahCacheItem;
        GraphData = GetCategoryGraph.AsJson(categoryCahCacheItem);
        GraphDataString = EscapeChars(new JavaScriptSerializer().Serialize(GraphData.Data));
    }

    private string EscapeChars(string objectString)
    {
        return objectString.Replace(Environment.NewLine, String.Empty).Replace("\'", @"\'").Replace("\"", "\\\"").Replace(@"\t", " ").Replace("\\r\\n", "");
    }
}