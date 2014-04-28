using System.Collections.Generic;
using System.Web;

public class CategoryImageSettings : IImageSettings
{
    public int Id { get; private set; }

    public IEnumerable<int> SizesSquare{ get { return new[] { 206, 50 }; } }
    public IEnumerable<int> SizesFixedWidth { get { return new[] { 500 }; } }

    public string BasePath { get { return "/Images/Categories/"; } }

    public CategoryImageSettings(){}

    public CategoryImageSettings(int categoryId){
        Id = categoryId;
    }

    public string ServerPathAndId(){
        return HttpContext.Current.Server.MapPath(BasePath + Id);
    }

    public void Init(int categoryId){
        Id = categoryId;
    }

    public ImageUrl GetUrl_50px(bool asSquare = false) { return GetUrl(50, asSquare); }
    public ImageUrl GetUrl_128px(bool asSquare = false) { return GetUrl(128, asSquare); }
    public ImageUrl GetUrl_85px_square() { return GetUrl(85, isSquare: true); }

    public ImageUrl GetUrl_206px_square() { return GetUrl(206, isSquare: true); }

    public ImageUrl GetUrl_350px_square() { return GetUrl(350, isSquare: true); }

    private ImageUrl GetUrl(int width, bool isSquare = false)
    {
        return ImageUrl.Get(this, width, isSquare, GetFallbackImage);
    }

    private string GetFallbackImage(int width){
        return "/Images/no-category-picture-" + width + ".png";
    }
}