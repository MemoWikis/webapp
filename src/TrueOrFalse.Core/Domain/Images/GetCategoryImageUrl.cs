using TrueOrFalse.Core;

public class GetCategoryImageUrl : GetImageUrl
{
    public string Run(Category category)
    {
        return Run(category.Id);
    }

    protected override string PlaceholderImage
    {
        get { return "/Images/no-category-picture-{0}.png"; }
    }

    protected override string RelativePath
    {
        get { return "/Images/Categories/"; }
    }
}