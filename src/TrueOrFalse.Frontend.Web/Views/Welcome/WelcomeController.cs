﻿using System;
using System.Net.Mail;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Web;

public class WelcomeController : BaseController
{
    [SetMainMenu(MainMenuEntry.None)]
    [SetThemeMenu]
    public ActionResult Welcome()
    {
        CategoryCacheItem category;
        category = IsLoggedIn ? EntityCache.GetCategoryCacheItem(_sessionUser.User.StartTopicId, getDataFromEntityCache: true) : RootCategory.Get; 

        return Redirect(Links.CategoryDetail(category)); ;
    }

    public ActionResult Team()
    {
        return View(new BaseModel());
    }

    public ActionResult Promoter()
    {
        return View(new BaseModel());
    }

    public ActionResult Contact()
    {
        return View(new BaseModel());
    }

    public ActionResult LogOn() => View();

    public ActionResult Logout()
    {
        RemovePersistentLoginFromCookie.Run();
        _sessionUser.Logout();
        return View(new BaseModel());
    }


    public ActionResult PasswordRecovery()
    {
        return View(new PasswordRecoveryModel());
    }

    [HttpPost]
    public ActionResult PasswordRecovery(PasswordRecoveryModel model)
    {
        var result = Sl.Resolve<PasswordRecovery>().Run(model.Email);
        if (result.TheEmailDoesNotExist)
            model.Message = new ErrorMessage("Diese E-Mail-Adresse ist uns unbekannt.");
        else if (result.Success)
            model.Message = new SuccessMessage("Wir haben an " + model.Email + " eine E-Mail verschickt (überprüfe gegebenenfalls auch deinen Spam-Ordner). Klicke dort auf den Link, um ein neues Passwort zu setzen.");

        return View(model);
    }

    [HttpGet]
    public ActionResult PasswordReset(string id)
    {
        var result = PasswordResetPrepare.Run(id);
        var model = new PasswordResetModel { TokenFound = result.Success, Token = id };

        if (result.TokenOlderThan72h)
        {
            model.Message = new ErrorMessage("Der Link ist abgelaufen.");
            return View(model);
        }

        if (!result.Success)
            model.Message = new ErrorMessage("Der Link ist leider ungültig. Wenn du Probleme hast, schreibe uns einfach eine E-Mail an team@memucho.de.");

        return View(model);
    }

    [HttpPost]
    public ActionResult PasswordReset(PasswordResetModel model)
    {
        if (!ModelState.IsValid)
        {
            model.TokenFound = true;
            model.Message = new ErrorMessage("Bitte überprüfe deine Eingaben.");
            return View(model);
        }

        var result = PasswordResetPrepare.Run(model.Token);

        var userRepo = Sl.Resolve<UserRepo>();
        var user = userRepo.GetByEmail(result.Email);

        if (user == null)
            throw new Exception();

        SetUserPassword.Run(model.NewPassword1, user);
        userRepo.Update(user);

        _sessionUser.Login(user);

        return RedirectToAction(Links.KnowledgeAction, Links.KnowledgeController, new {passwordSet = "true"});
    }

    public void SendNewsletterRequest(string requesterEmail)
    {
        if (String.IsNullOrEmpty(requesterEmail))
            return;

        SendEmail.Run(new MailMessage(
            Settings.EmailFrom,
            Settings.EmailToMemucho,
            "Newsletter sign up request",
            requesterEmail + " asked to sign up for newsletter."), MailMessagePriority.Medium);
    }
}