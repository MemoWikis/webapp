using Microsoft.AspNetCore.Http;

public class PageContentImageSettings : ImageSettings, IImageSettings
{
    public override int Id { get; set; }
    public ImageType ImageType => ImageType.PageContent;
    public IEnumerable<int> SizesSquare => [128];
    public IEnumerable<int> SizesFixedWidth => [800];

    public override string BasePath => Settings.PageContentImageBasePath;
    public string BaseDummyUrl => "no-category-picture-128.png";

    public PageContentImageSettings(
        int pageId,
        IHttpContextAccessor contextAccessor) :
        base(contextAccessor)
    {
        Id = pageId;
    }

    public void Init(int pageId)
    {
        Id = pageId;
    }
}