using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

public class CategoryImageSettings : ImageSettings, IImageSettings
{
    public int Id { get; set; }
    public ImageType ImageType => ImageType.Category;
    public IEnumerable<int> SizesSquare => new[] { 206, 50 };
    public IEnumerable<int> SizesFixedWidth => new[] { 500 };

    public string BasePath => "/Images/Categories/";
    public string BaseDummyUrl => "/Images/no-category-picture-";

    public CategoryImageSettings(int categoryId,
        IHttpContextAccessor contextAccessor, 
        IWebHostEnvironment webHostEnvironment) :
        base(contextAccessor, webHostEnvironment)
    {
        Id = categoryId;
    }

    public CategoryImageSettings(
        IHttpContextAccessor contextAccessor,
        IWebHostEnvironment webHostEnvironment) :
        base(contextAccessor, webHostEnvironment)
    {
     
    }

    public void Init(int categoryId){
        Id = categoryId;
    }

    public ImageUrl GetUrl_128px(bool asSquare = false) { return GetUrl(128, asSquare); }
    public ImageUrl GetUrl(int width, bool isSquare = false)
    {
        return new ImageUrl(_contextAccessor, _webHostEnvironment)
            .Get(this, width, isSquare, GetFallbackImage);
    }

    private string GetFallbackImage(int width){
        return BaseDummyUrl + width + ".png";
    }
}