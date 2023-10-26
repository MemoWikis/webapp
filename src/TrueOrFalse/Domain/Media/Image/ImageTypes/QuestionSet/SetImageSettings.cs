using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

public class SetImageSettings : ImageSettings, IImageSettings
{
    public override int Id { get; set; }
    public ImageType ImageType => ImageType.QuestionSet;
    public IEnumerable<int> SizesSquare => new[] { 206, 20 };
    public IEnumerable<int> SizesFixedWidth => new[] { 500 };
    public override string  BasePath => Path.Combine(ImageFolderPath(), "QuestionSets");
    public string BaseDummyUrl => Path.Combine(ImageFolderPath(), "no-set-");

    public SetImageSettings(IHttpContextAccessor httpContextAccessor) :
        base(httpContextAccessor)  {}

    public SetImageSettings(int setId, 
        IHttpContextAccessor httpContextAccessor) :
        base(httpContextAccessor)
    {
        Init(setId);
    }

    public void Init(int setId){
        Id = setId;
    }
}