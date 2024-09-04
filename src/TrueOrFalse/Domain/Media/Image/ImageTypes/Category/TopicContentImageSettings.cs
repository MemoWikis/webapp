using Microsoft.AspNetCore.Http;

public class TopicContentImageSettings : ImageSettings, IImageSettings
{
    public override int Id { get; set; }
    public ImageType ImageType => ImageType.TopicContent;
    public IEnumerable<int> SizesSquare => [128];
    public IEnumerable<int> SizesFixedWidth => [800];

    public override string BasePath => Settings.TopicContentImageBasePath;
    public string BaseDummyUrl => "no-category-picture-128.png";

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
}