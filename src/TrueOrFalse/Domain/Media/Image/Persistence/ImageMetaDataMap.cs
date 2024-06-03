using FluentNHibernate.Mapping;

public class ImageMetaDataMap : ClassMap<ImageMetaData>
{
    public ImageMetaDataMap()
    {
        Id(x => x.Id);

        Map(x => x.Type).CustomType<ImageType>(); 
        Map(x => x.TypeId);

        Map(x => x.UserId);
            
        Map(x => x.Source).CustomType<ImageSource>();
        Map(x => x.SourceUrl);

        Map(x => x.ApiResult).Length(8000);
        Map(x => x.ApiHost);

        Map(x => x.AuthorParsed).Column("Author");
        Map(x => x.DescriptionParsed).Column("Description");
        Map(x => x.Markup);
        Map(x => x.MarkupDownloadDate);

        Map(x => x.ManualEntries);
        Map(x => x.MainLicenseInfo);
        Map(x => x.AllRegisteredLicenses);

        Map(x => x.Notifications);
        Map(x => x.LicenseState).CustomType<ImageLicenseState>(); ;

        Map(x => x.DateCreated);
        Map(x => x.DateModified);
    }
}