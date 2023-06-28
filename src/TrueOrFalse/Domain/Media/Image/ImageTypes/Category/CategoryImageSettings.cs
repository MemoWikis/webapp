using System.Collections.Generic;

public class CategoryImageSettings : ImageSettings, IImageSettings
{
    public override int Id { get; set; }
    public ImageType ImageType => ImageType.Category;
    public IEnumerable<int> SizesSquare => new[] { 206, 50 };
    public IEnumerable<int> SizesFixedWidth => new[] { 500 };

    public override string BasePath => "/Images/Categories/";
    public string BaseDummyUrl => "/Images/no-category-picture-";

    public CategoryImageSettings(){}

    public CategoryImageSettings(int categoryId){
        Id = categoryId;
    }

    public void Init(int categoryId){
        Id = categoryId;
    }

    public ImageUrl GetUrl_128px(bool asSquare = false) { return GetUrl(128, asSquare); }
    public ImageUrl GetUrl(int width, bool isSquare = false)
    {
        return ImageUrl.Get(this, width, isSquare, GetFallbackImage);
    }

    private string GetFallbackImage(int width){
        return BaseDummyUrl + width + ".png";
    }
}