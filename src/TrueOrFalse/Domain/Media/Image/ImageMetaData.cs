using System;
using Seedworks.Lib.Persistence;

public class ImageMetaData : DomainEntity
{
    public virtual ImageType Type { get; set; }
    /// <summary>E.g questionId, questionSetId, ... </summary>
    public virtual int TypeId { get; set; }
    public virtual ImageSource Source { get; set; }
    public virtual string SourceUrl { get; set; }
    public virtual string ApiResult { get; set; }
    public virtual string ApiHost { get; set; }
    public virtual int UserId { get; set; }
    public virtual string AuthorParsed { get; set; }
    public virtual string DescriptionParsed { get; set; }
    public virtual string Markup { get; set; }
    public virtual DateTime MarkupDownloadDate { get; set; }
    public virtual string ManualEntries { get; set; }
    public virtual string MainLicenseInfo { get; set; }
    public virtual string AllRegisteredLicenses { get; set; }
    public virtual string Notifications { get; set; }
    public virtual ImageLicenseState LicenseState { get; set; }
        
    public virtual ManualImageData ManualEntriesFromJson() => ManualImageData.FromJson(ManualEntries);
    public virtual ImageParsingNotifications NotificationsFromJson() => ImageParsingNotifications.FromJson(Notifications);

    public virtual IImageSettings GetSettings() => ImageSettings.InitByType(this);

    public virtual bool IsYoutubePreviewImage { get; set; }
    public virtual string YoutubeKey { get; set; }
}