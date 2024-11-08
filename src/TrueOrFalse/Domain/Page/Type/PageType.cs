public enum PageType
{
    None = 0,
    Standard = 1,

    Book = 5,
    VolumeChapter = 20,
    Daily = 6,
    DailyIssue = 7,
    DailyArticle = 8,
    Magazine = 9,
    MagazineIssue = 10,
    MagazineArticle = 11,

    Website = 2,
    WebsiteArticle = 3,
    WebsiteVideo = 4,
    WebsiteOther = 21,

    Movie = 12,
    TvShow = 13,
    TvShowEpisode = 14,

    FieldOfStudy = 15,
    FieldOfTraining = 16,
    SchoolSubject = 17,
    Course = 18,
    Certification = 19,
    EducationProvider = 22
}

public enum PageTypeGroup
{
    Standard = 1,
    Media = 2,
    Education = 3,
}
public static class CategoryTypeExts
{
    public static PageTypeGroup GetPageTypeGroup(this PageType e)
    {

        if (e == PageType.Standard)
            return PageTypeGroup.Standard;

        if (e == PageType.Book ||
            e == PageType.Daily ||
            e == PageType.DailyArticle ||
            e == PageType.DailyIssue ||
            e == PageType.Magazine ||
            e == PageType.MagazineArticle ||
            e == PageType.MagazineIssue ||
            e == PageType.Movie ||
            e == PageType.TvShow ||
            e == PageType.TvShowEpisode ||
            e == PageType.VolumeChapter ||
            e == PageType.Website ||
            e == PageType.WebsiteArticle ||
            e == PageType.WebsiteOther ||
            e == PageType.WebsiteVideo)
        {
            return PageTypeGroup.Media;
        }

        if (e == PageType.Certification ||
            e == PageType.Course ||
            e == PageType.FieldOfStudy ||
            e == PageType.FieldOfTraining ||
            e == PageType.SchoolSubject ||
            e == PageType.EducationProvider)
        {
            return PageTypeGroup.Education;
        }

        throw new Exception("CategoryType isn't assigned to any CategoryTypeGroup.");
    }
}