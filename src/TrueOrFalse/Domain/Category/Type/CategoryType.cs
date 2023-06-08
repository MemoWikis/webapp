public enum CategoryType
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

public enum CategoryTypeGroup
{
    Standard = 1,
    Media = 2,
    Education = 3,
}
public static class CategoryTypeExts
{
    public static CategoryTypeGroup GetCategoryTypeGroup(this CategoryType e)
    {

        if (e == CategoryType.Standard)
            return CategoryTypeGroup.Standard;
        
        if (e == CategoryType.Book ||
            e == CategoryType.Daily ||
            e == CategoryType.DailyArticle ||
            e == CategoryType.DailyIssue ||
            e == CategoryType.Magazine ||
            e == CategoryType.MagazineArticle ||
            e == CategoryType.MagazineIssue ||
            e == CategoryType.Movie ||
            e == CategoryType.TvShow ||
            e == CategoryType.TvShowEpisode ||
            e == CategoryType.VolumeChapter ||
            e == CategoryType.Website ||
            e == CategoryType.WebsiteArticle ||
            e == CategoryType.WebsiteOther ||
            e == CategoryType.WebsiteVideo)
        {
            return CategoryTypeGroup.Media;
        }

        if (e == CategoryType.Certification ||
            e == CategoryType.Course ||
            e == CategoryType.FieldOfStudy ||
            e == CategoryType.FieldOfTraining ||
            e == CategoryType.SchoolSubject ||
            e == CategoryType.EducationProvider)
        {
            return CategoryTypeGroup.Education;
        }

        throw new Exception("CategoryType isn't assigned to any CategoryTypeGroup.");
    }
}