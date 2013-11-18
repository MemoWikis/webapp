using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Hql.Ast.ANTLR;
using TrueOrFalse;


public class ImageStore : IRegisterAsInstancePerLifetime
{
    private readonly WikiImageMetaLoader _metaLoader;
    private readonly ImageMetaDataRepository _imgMetaRepo;

    public ImageStore(
        WikiImageMetaLoader metaLoader,
        ImageMetaDataRepository imgMetaRepo)
    {
        _metaLoader = metaLoader;
        _imgMetaRepo = imgMetaRepo;
    }

    public void RunWikimedia<T>(string imageWikiFileName, int typeId, int userId) where T : IImageSettings
    {
        var wikiMetaData = _metaLoader.Run(imageWikiFileName, 1024);

        var imageSettings = Activator.CreateInstance<T>();
        imageSettings.Init(typeId);

        using (var stream = wikiMetaData.GetStream()){
            StoreImages.Run(stream, imageSettings);
        }

        _imgMetaRepo.StoreSetWiki(typeId, userId, wikiMetaData);
    }

    public void RunUploaded<T>(TmpImage tmpImage, int typeId, int userId, string licenceGiverName) where T : IImageSettings
    {
        var imageSettings = Activator.CreateInstance<T>();
        imageSettings.Init(typeId);

        using (var stream = tmpImage.GetStream()){
            StoreImages.Run(stream, imageSettings);    
        }

        _imgMetaRepo.StoreSetUploaded(typeId, userId, licenceGiverName);
    }

}