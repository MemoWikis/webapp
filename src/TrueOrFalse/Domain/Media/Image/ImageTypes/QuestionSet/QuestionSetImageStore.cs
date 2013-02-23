using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueOrFalse;


public class QuestionSetImageStore : IRegisterAsInstancePerLifetime
{
    private readonly WikiImageMetaLoader _metaLoader;
    private readonly ImageMetaDataRepository _imgMetaRepo;

    public QuestionSetImageStore(
        WikiImageMetaLoader metaLoader,
        ImageMetaDataRepository imgMetaRepo)
    {
        _metaLoader = metaLoader;
        _imgMetaRepo = imgMetaRepo;
    }

    public void RunWikimedia(string imageWikiFileName, int questionSetId, int userId)
    {
        var wikiMetaData = _metaLoader.Run(imageWikiFileName, 1024);

        using (var stream = wikiMetaData.GetStream()){
            StoreImages.Run(stream, new QuestionSetImageSettings(questionSetId));
        }

        var imageMeta = _imgMetaRepo.GetBy(questionSetId, ImageType.QuestionSet);
        if (imageMeta == null){
            _imgMetaRepo.Create(
                new ImageMetaData{
                    TypeId = questionSetId,
                    Type = ImageType.QuestionSet,
                    Source = ImageSource.WikiMedia,
                    SourceUrl = wikiMetaData.ImageUrl,
                    LicenceInfo = wikiMetaData.JSonResult,
                    UserId = userId
                }
            );
        }else{
            imageMeta.SourceUrl = wikiMetaData.ImageUrl;
            imageMeta.LicenceInfo = wikiMetaData.JSonResult;
            imageMeta.UserId = userId;
        }
        
    }
}