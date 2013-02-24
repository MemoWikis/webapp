using System.Collections.Generic;
using System.Web;
using TrueOrFalse;

public class QuestionSetImageSettings : IImageSettings
{
    private readonly int _questionSetId;
    public IEnumerable<int> SizesSquare { get { return new[] { 206, 20 }; } }
    public IEnumerable<int> SizesFixedWidth { get { return new[] { 500 }; } }
    public string BasePath { get { return "/Images/QuestionSets/"; } }
    
    public string BasePathAndId(){
        return HttpContext.Current.Server.MapPath(BasePath + _questionSetId);
    }

    public QuestionSetImageSettings(int questionSetId){
        _questionSetId = questionSetId;
    }

    public ImageUrl GetUrl_206px_square() { return GetUrl(206, isSquare:true); }

    private ImageUrl GetUrl(int width, bool isSquare = false)
    {
        var imageMetaRepo = ServiceLocator.Resolve<ImageMetaDataRepository>();
        var imageMeta = imageMetaRepo.GetBy(_questionSetId, ImageType.QuestionSet);

        return ImageUrl.Get(
            _questionSetId, 
            width, 
            isSquare,
            BasePath,
            arg => "/Images/no-question-set-" + width + ImageUrl.SquareSuffix(isSquare) + ".png"
        ).SetSuffix(imageMeta);
    }
}

