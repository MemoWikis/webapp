using System;

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
    public static string GetName(this CategoryType e)
    {
        switch (e)
        {
            case CategoryType.Standard: return "Standard";

            case CategoryType.Book: return "Buch";
            case CategoryType.VolumeChapter: return "Beitrag in Sammelband";
            case CategoryType.Daily: return "Tageszeitung";
            case CategoryType.DailyIssue: return "Ausgabe Tageszeitung";
            case CategoryType.DailyArticle: return "Artikel in Tageszeitung";
            case CategoryType.Magazine: return "Zeitschrift";
            case CategoryType.MagazineIssue: return "Ausgabe Zeitschrift";
            case CategoryType.MagazineArticle: return "Artikel in Zeitschrift";

            case CategoryType.Website: return "Webseite";
            case CategoryType.WebsiteArticle: return "Online-Artikel / Blog-Artikel";
            case CategoryType.WebsiteVideo: return "Youtube-Video";
            case CategoryType.WebsiteOther: return "Webseite: Artikel/Eintrag/Meldung/Kurs...";


            case CategoryType.Movie: return "Film";
            case CategoryType.TvShow: return "Fernsehen";
            case CategoryType.TvShowEpisode: return "Fernsehen: Episode/Ausgabe";

            case CategoryType.FieldOfStudy: return "Studienfach";
            case CategoryType.FieldOfTraining: return "Ausbildungsberuf";
            case CategoryType.SchoolSubject: return "Schulfach";
            case CategoryType.Course: return "Kurs/Seminar";
            case CategoryType.Certification: return "Zertifizierung";
            case CategoryType.EducationProvider: return "Bildungsanbieter";
        }
        throw new Exception("invalid category type");
    }

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

    public static string GetShortName(this CategoryType e)
    {
        if (e == CategoryType.DailyArticle || e == CategoryType.MagazineArticle)
            return "Artikel";
        if (e == CategoryType.WebsiteArticle)
            return "Online-Artikel";
        return e.GetName();
    }

    public static string GetUrlDescription(this CategoryType e)
    {
        switch (e)
        {
            //case CategoryType.Standard: return "Offizielle Webseite";

            case CategoryType.Book: return "Offizielle Webseite zum Buch";
            case CategoryType.VolumeChapter: return "Offizielle Webseite zum Sammelbandbeitrag";
            case CategoryType.Daily: return "Webseite der Tageszeitung";
            case CategoryType.DailyIssue: return "Webseite der Ausgabe";
            case CategoryType.DailyArticle: return "Webseite des Artikel der Tageszeitung";
            case CategoryType.Magazine: return "Webseite der Zeitschrift";
            case CategoryType.MagazineIssue: return "Webseite der Ausgabe";
            case CategoryType.MagazineArticle: return "Webseite des Artikels";

            //case CategoryType.Website: return "Webseite";
            //case CategoryType.WebsiteArticle: return "Online-Artikel / Blog-Artikel";
            case CategoryType.WebsiteVideo: return "Link zum Youtube-Video";
            //case CategoryType.WebsiteOther: return "Webseite: Artikel/Eintrag/Meldung/Kurs...";


            case CategoryType.Movie: return "Offizielle Webseite zum Film";
            case CategoryType.TvShow: return "Offizielle Webseite zur Sendung";
            case CategoryType.TvShowEpisode: return "Offizielle Webseite zur Episode/Ausgabe";

            case CategoryType.FieldOfStudy: return "Offizielle Webseite zum Studienfach";
            case CategoryType.FieldOfTraining: return "Offizielle Webseite zum Ausbildungsberuf";
            case CategoryType.SchoolSubject: return "Offizielle Webseite zum Schulfach";
            case CategoryType.Course: return "Offizielle Webseite zum Kurs/Seminar";
            case CategoryType.Certification: return "Offizielle Webseite zur Zertifizierung";
            case CategoryType.EducationProvider: return "Offizielle Webseite des Bildungsanbieters";
        }
        return "";

    }

}