using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum CategoryType
{
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
    FieldStudyTrade = 16,
    SchoolSubject = 17,
    Course = 18,
    Certification = 19,
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
            case CategoryType.WebsiteArticle: return "Online-Artikel";
            case CategoryType.WebsiteVideo: return "Youtube-Video";
            case CategoryType.WebsiteOther: return "Webseite: Artikel/Eintrag/Meldung/Kurs...";


            case CategoryType.Movie: return "Film";
            case CategoryType.TvShow: return "Fernsehen";
            case CategoryType.TvShowEpisode: return "Fernsehen: Episode/Ausgabe";

            case CategoryType.FieldOfStudy: return "Studienfach";
            case CategoryType.FieldStudyTrade: return "Ausbildungsberuf";
            case CategoryType.SchoolSubject: return "Schulfach";
            case CategoryType.Course: return "Kurs/Seminar";
            case CategoryType.Certification: return "Zertifizierung";
        }
        throw new Exception("invalid type");
    }    
}