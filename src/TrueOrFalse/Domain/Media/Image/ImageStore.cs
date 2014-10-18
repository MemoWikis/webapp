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
    private readonly WikiImageLicenceLoader _wikiImageLicenceLoader;
    private readonly ImageMetaDataRepository _imgMetaRepo;

    public ImageStore(
        WikiImageMetaLoader metaLoader,
        WikiImageLicenceLoader wikiImageLicenceLoader,
        ImageMetaDataRepository imgMetaRepo)
    {
        _metaLoader = metaLoader;
        _wikiImageLicenceLoader = wikiImageLicenceLoader;
        _imgMetaRepo = imgMetaRepo;
    }

    public int RunWikimedia<T>(
        string imageWikiFileName, 
        int typeId, 
        ImageType imageType,
        int userId) where T : IImageSettings
    {
        var wikiMetaData = _metaLoader.Run(imageWikiFileName, 1024);

        var imageSettings = Activator.CreateInstance<T>();
        imageSettings.Init(typeId);

        using (var stream = wikiMetaData.GetStream()){
            StoreImages.Run(stream, imageSettings);//$temp: Bildbreite uebergeben und abhaengig davon versch. Groessen speichern?
        }

        var licenceInfo = _wikiImageLicenceLoader.Run(wikiMetaData.ImageTitle, wikiMetaData.ApiHost);

        _imgMetaRepo.StoreWiki(typeId, imageType, userId, wikiMetaData, licenceInfo);

        return wikiMetaData.ImageWidth;

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