using System.Web;
using System.Web.Mvc;
using TrueOrFalse.Core;

public class GetCategoryImageUrl : GetImageUrl<Category>
{
    protected override string GetFallbackImage(Category category)
    {
        return "/Images/no-category-picture-{0}.png";
    }

    protected override string RelativePath
    {
        get { return "/Images/Categories/"; }
    }
}