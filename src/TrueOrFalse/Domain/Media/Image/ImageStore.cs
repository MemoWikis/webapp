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
    private readonly WikiImageLicenseLoader _wikiImageLicenseLoader;
    private readonly ImageMetaDataRepository _imgMetaRepo;

    public ImageStore(
        WikiImageMetaLoader metaLoader,
        WikiImageLicenseLoader wikiImageLicenseLoader,
        ImageMetaDataRepository imgMetaRepo)
    {
        _metaLoader = metaLoader;
        _wikiImageLicenseLoader = wikiImageLicenseLoader;
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

        _imgMetaRepo.StoreWiki(typeId, imageType, userId, wikiMetaData);

        return wikiMetaData.ImageWidth;

    }

    public void RunUploaded<T>(TmpImage tmpImage, int typeId, int userId, string licenseGiverName) where T : IImageSettings
    {
        var imageSettings = Activator.CreateInstance<T>();
        imageSettings.Init(typeId);

        using (var stream = tmpImage.GetStream()){
            StoreImages.Run(stream, imageSettings);
        }

        _imgMetaRepo.StoreSetUploaded(typeId, userId, licenseGiverName);
    }

}