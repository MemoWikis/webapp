using System;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;
using static System.String;

public class SeoUtils
{
    public static string ReplaceDoubleQuotes(string value) => 
        IsNullOrEmpty(value) 
            ? "" 
            : value.Replace("\"", "'").Replace("„", "'").Replace("“", "'");


    public static bool HasUnderscores(string term) => term.Contains("_");

    public static ActionResult RedirectToHyphendVersion(Func<string, RedirectResult> redirect, int setIdValue, int questionId) 
        => redirect(Links.AnswerQuestion(Sl.QuestionRepo.GetById(questionId), Sl.SetRepo.GetById(setIdValue)));

    public static ActionResult RedirectToHyphendVersion(Func<string, RedirectResult> redirect, int questionId)
        => redirect(Links.AnswerQuestion(Sl.QuestionRepo.GetById(questionId)));

    public static ActionResult RedirectToHyphendVersion_Set(Func<string, RedirectResult> redirect, string text, int setId)
        => redirect(Links.SetDetail(text, setId));

    public static ActionResult RedirectToHyphendVersion_Category(Func<string, RedirectResult> redirect, string text, int categoryId)
        => redirect(Links.CategoryDetail(text, categoryId));

    public static ActionResult RedirectToNewCategory(Func<string, RedirectResult> redirect, string text, int categoryId)
        => redirect(Links.CategoryDetailRedirect(text, categoryId));
}

