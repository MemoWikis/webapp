﻿using Microsoft.AspNetCore.Http;

public class PageImageSettings : ImageSettings, IImageSettings
{
    public override int Id { get; set; }
    public ImageType ImageType => ImageType.Page;
    public IEnumerable<int> SizesSquare => new[] { 206, 50 };
    public IEnumerable<int> SizesFixedWidth => new[] { 500 };

    public override string BasePath => Settings.PageImageBasePath;
    public string BaseDummyUrl => "Placeholders/placeholder-page-";

    public PageImageSettings(
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

    public ImageUrl GetUrl_128px(bool asSquare = false)
    {
        return GetUrl(128, asSquare);
    }

    public ImageUrl GetUrl(int width, bool isSquare = false)
    {
        return new ImageUrl(_contextAccessor).Get(this, width, isSquare, GetFallbackImage);
    }

    private string GetFallbackImage(int width)
    {
        return BaseDummyUrl + width + ".png";
    }
}