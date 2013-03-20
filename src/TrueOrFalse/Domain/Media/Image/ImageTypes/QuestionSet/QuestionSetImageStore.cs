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

        _imgMetaRepo.StoreSetWiki(questionSetId, userId, wikiMetaData);
    }

    public void RunUploaded(TmpImage tmpImage, int questionSetId, int userId, string licenceGiverName)
    {
        using (var stream = tmpImage.GetStream()){
            StoreImages.Run(stream, new QuestionSetImageSettings(questionSetId));    
        }

        _imgMetaRepo.StoreSetUploaded(questionSetId, userId, licenceGiverName);
    }

}