using System.Web.Mvc;
using System.Web.Script.Serialization;

public class CategoryGraphModel : BaseModel
{
    public Category Category;
    public JsonResult GraphData;
    public string GraphDataString;

    public CategoryGraphModel(Category category)
    {
        Category = category;
        GraphData = GetCategoryGraph.AsJson(category);
        GraphDataString = new JavaScriptSerializer().Serialize(GraphData.Data);
    }
}