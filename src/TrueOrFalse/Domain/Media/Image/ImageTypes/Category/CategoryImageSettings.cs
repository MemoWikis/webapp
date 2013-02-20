using System.Collections.Generic;
using System.Web;

public class CategoryImageSettings : IImageSettings
{
    private readonly int _categoryId;

    public IEnumerable<int> SizesSquare{ get { return new[] { 512, 128, 50, 20 }; } }
    public IEnumerable<int> SizesFixedWidth { get { return new[] { 100, 500 }; } }

    public string BasePath { get { return "/Images/Categories/"; } }

    public CategoryImageSettings(int categoryId){
        _categoryId = categoryId;
    }

    public string BasePathAndId(){
        return HttpContext.Current.Server.MapPath(BasePath + _categoryId);
    }

    public ImageUrl GetUrl_50px() { return GetUrl(50); }
    public ImageUrl GetUrl_128px() { return GetUrl(128); }

    private ImageUrl GetUrl(int width){
        return ImageUrl.Get(_categoryId, width, BasePath, GetFallbackImage);
    }

    private string GetFallbackImage(int width){
        return "/Images/no-category-picture-" + width + ".png";
    }


}

