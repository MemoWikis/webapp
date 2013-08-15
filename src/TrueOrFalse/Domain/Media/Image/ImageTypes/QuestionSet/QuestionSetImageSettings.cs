using System.Collections.Generic;
using System.Web;
using TrueOrFalse;

public class QuestionSetImageSettings : IImageSettings
{
    public int Id { get; private set; }

    public IEnumerable<int> SizesSquare { get { return new[] { 206, 20 }; } }
    public IEnumerable<int> SizesFixedWidth { get { return new[] { 500 }; } }
    public string BasePath { get { return "/Images/QuestionSets/"; } }
    
    public string ServerPathAndId(){
        return HttpContext.Current.Server.MapPath(BasePath + Id);
    }

    public QuestionSetImageSettings(int questionSetId){
        Id = questionSetId;
    }

    public ImageUrl GetUrl_206px_square() { return GetUrl(206, isSquare:true); }

    private ImageUrl GetUrl(int width, bool isSquare = false)
    {
        var imageMetaRepo = ServiceLocator.Resolve<ImageMetaDataRepository>();
        var imageMeta = imageMetaRepo.GetBy(Id, ImageType.QuestionSet);

        return ImageUrl.Get(
            this,
            width, 
            isSquare,
            arg => "/Images/no-question-set-" + width + ImageUrl.SquareSuffix(isSquare) + ".png"
        ).SetSuffix(imageMeta);
    }
}

