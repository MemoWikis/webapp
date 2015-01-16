using System.Collections.Generic;
using System.Web;
using TrueOrFalse;

public class SetImageSettings : ImageSettingsBase, IImageSettings
{
    public override int Id { get; set; }
    public ImageType ImageType
    {
        get { return ImageType.QuestionSet; }
    }
    public IEnumerable<int> SizesSquare { get { return new[] { 206, 20 }; } }
    public IEnumerable<int> SizesFixedWidth { get { return new[] { 500 }; } }
    public override string BasePath { get { return "/Images/QuestionSets/"; } }
    public string BaseDummyUrl { get { return "/Images/no-set-"; } }

    public static SetImageSettings Create(int questionSetId)
    {
        var result =  new SetImageSettings();
        result.Init(questionSetId);
        return result;
    }

    public void Init(int questionSetId)
    {
        Id = questionSetId;
    }

    public ImageUrl GetUrl_128px_square() { return GetUrl(128, isSquare: true); }
    public ImageUrl GetUrl_206px_square() { return GetUrl(206, isSquare:true); }
    public ImageUrl GetUrl_350px_square() { return GetUrl(350, isSquare: true); }

    private ImageUrl GetUrl(int width, bool isSquare = false)
    {
        var imageMetaRepo = ServiceLocator.Resolve<ImageMetaDataRepository>();
        var imageMeta = imageMetaRepo.GetBy(Id, ImageType.QuestionSet);

        return ImageUrl.Get(
            this,
            width, 
            isSquare,
            arg => BaseDummyUrl + width + ".png"
        ).SetSuffix(imageMeta);
    }
}

