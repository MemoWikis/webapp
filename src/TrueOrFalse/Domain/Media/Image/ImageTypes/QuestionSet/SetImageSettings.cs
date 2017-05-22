using System.Collections.Generic;
using static System.String;

public class SetImageSettings : ImageSettings, IImageSettings
{
    public override int Id { get; set; }
    public ImageType ImageType => ImageType.QuestionSet;
    public IEnumerable<int> SizesSquare => new[] { 206, 20 };
    public IEnumerable<int> SizesFixedWidth => new[] { 500 };
    public override string BasePath => "/Images/QuestionSets/";
    public string BaseDummyUrl => "/Images/no-set-";

    public SetImageSettings() {}

    public SetImageSettings(int setId){
        Init(setId);
    }

    public void Init(int setId){
        Id = setId;
    }

    public ImageUrl GetUrl_50px_square() { return GetUrl(50, isSquare: true); }
    public ImageUrl GetUrl_128px_square() { return GetUrl(128, isSquare: true); }
    public ImageUrl GetUrl_206px_square() { return GetUrl(206, isSquare:true); }
    public ImageUrl GetUrl_350px_square() { return GetUrl(350, isSquare: true); }

    public ImageUrl GetUrl(int width, bool isSquare = false)
    {
        var imageMetaRepo = ServiceLocator.Resolve<ImageMetaDataRepo>();
        var imageMeta = imageMetaRepo.GetBy(Id, ImageType.QuestionSet);

        return ImageUrl.Get(
            this,
            width, 
            isSquare,
            arg =>
            {
                var youtubUrl = Sl.SetRepo.GetYoutbeUrl(Id);

                if (!IsNullOrEmpty(youtubUrl))
                {
                    var youtubeKey = YoutubeVideo.GetVideoKeyFromUrl(youtubUrl);
                    if(!IsNullOrEmpty(youtubeKey))
                        return YoutubeVideo.GetPreviewImage(youtubeKey);
                }

                return BaseDummyUrl + width + ".png";
            }
        ).SetSuffix(imageMeta);
    }
}