using Microsoft.AspNetCore.Http;

public class PageImageSettings : ImageSettings, IImageSettings
{
    public override int Id { get; set; }
    public ImageType ImageType => ImageType.Page;
    public IEnumerable<int> SizesSquare => new[] { 206, 50 };
    public IEnumerable<int> SizesFixedWidth => new[] { 500 };

    public override string BasePath => Settings.TopicImageBasePath;
    public string BaseDummyUrl => "no-category-picture-";

    public PageImageSettings(
        int categoryId,
        IHttpContextAccessor contextAccessor) :
        base(contextAccessor)
    {
        Id = categoryId;
    }

    public void Init(int categoryId)
    {
        Id = categoryId;
    }

    public ImageUrl GetUrl_128px(bool asSquare = false)
    {
        return GetUrl(128, asSquare);
    }

    public ImageUrl GetUrl(int width, bool isSquare = false)
    {
        return new ImageUrl(_contextAccessor)
            .Get(this, width, isSquare, GetFallbackImage);
    }

    private string GetFallbackImage(int width)
    {
        return BaseDummyUrl + width + ".png";
    }
}