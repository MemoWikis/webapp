using Microsoft.AspNetCore.Http;

public class TopicContentImageSettings : ImageSettings, IImageSettings
{
    public override int Id { get; set; }
    public ImageType ImageType => ImageType.Category;
    public IEnumerable<int> SizesSquare => new[] { 206, 50 };
    public IEnumerable<int> SizesFixedWidth => new[] { 500, 800 };

    public override string BasePath => "TopicContent";
    public string BaseDummyUrl => "no-category-picture-";

    public TopicContentImageSettings(
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