﻿using System;
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
}

