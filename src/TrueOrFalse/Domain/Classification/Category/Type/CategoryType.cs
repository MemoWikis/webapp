using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum CategoryType
{
    Standard = 1,
    Website = 2,
    WebsiteArticle = 3,
    WebsiteVideo = 4,
    Book = 5,
    Daily = 6,
    DailyIssue = 7,
    DailyArticle = 8,
    Magazine = 9,
    MagazineIssue = 10,
    MagazineArticle = 11,
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
            case CategoryType.Website: return "Webseite";
            case CategoryType.WebsiteArticle: return "Webseite -> Artikel/Eintrag/Meldung/..";
            case CategoryType.WebsiteVideo: return "Youtube";
            case CategoryType.Book: return "Buch (auch eBooks)";
            case CategoryType.Daily: return "Tageszeitung";
            case CategoryType.DailyIssue: return "Tageszeitung -> Ausgabe";
            case CategoryType.DailyArticle: return "Tageszeitung -> Artikel";
            case CategoryType.Magazine: return "Zeitschrift/Magazin";
            case CategoryType.MagazineIssue: return "Zeitschrift/Magazin -> Ausgabe";
            case CategoryType.MagazineArticle: return "Zeitschrift/Magazin -> Artikel";
            case CategoryType.Movie: return "Film";
            case CategoryType.TvShow: return "Fernsehen";
            case CategoryType.TvShowEpisode: return "Fernsehen - Episode/Ausgabe";
            case CategoryType.FieldOfStudy: return "Studienfach";
            case CategoryType.FieldStudyTrade: return "Ausbildungsberuf";
            case CategoryType.SchoolSubject: return "Schulfach";
            case CategoryType.Course: return "Kurs/Seminar";
            case CategoryType.Certification: return "Zertifizierung";
        }
        throw new Exception("invalid type");
    }    
}