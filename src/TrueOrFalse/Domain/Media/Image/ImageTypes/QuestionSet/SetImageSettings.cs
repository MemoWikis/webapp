using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using static System.String;

public class SetImageSettings : ImageSettings, IImageSettings
{
    public int Id { get; set; }
    public ImageType ImageType => ImageType.QuestionSet;
    public IEnumerable<int> SizesSquare => new[] { 206, 20 };
    public IEnumerable<int> SizesFixedWidth => new[] { 500 };
    public string BasePath => "/Images/QuestionSets/";
    public string BaseDummyUrl => "/Images/no-set-";

    public SetImageSettings(IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment) :
        base(httpContextAccessor, webHostEnvironment)  {}

    public SetImageSettings(int setId, 
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment) :
        base(httpContextAccessor, webHostEnvironment)
    {
        Init(setId);
    }

    public void Init(int setId){
        Id = setId;
    }
}