﻿using Google.Apis.Auth;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace VueApp;

public class GoogleController : Controller
{
    private readonly VueSessionUser _vueSessionUser;
    private readonly RegisterUser _registerUser;
    private readonly UserRepo _userRepo;
    private readonly SessionUser _sessionUser;

    public GoogleController(SessionUser sessionUser,
        UserRepo userRepo, 
        VueSessionUser vueSessionUser,
        RegisterUser registerUser)
    {
        _vueSessionUser = vueSessionUser;
        _registerUser = registerUser;
        _userRepo = userRepo;
        _sessionUser = sessionUser;
    }

    [HttpPost]
    public async Task<JsonResult> Login(string token)
    {
        var googleUser = await GetGoogleUser(token);
        if (googleUser != null)
        {
            var user = _userRepo.UserGetByGoogleId(googleUser.Subject);

            if (user == null)
            {
                var newUser = new GoogleUserCreateParameter
                {
                    Email = googleUser.Email,
                    UserName = googleUser.Name,
                    GoogleId = googleUser.Subject
                };
                return CreateAndLogin(newUser);
            }

            _sessionUser.Login(user);
            return Json(new
            {
                success = true,
                currentUser = _vueSessionUser.GetCurrentUserData()
            });
        }

        return Json(new
        {
            success = false,
        });
       
    }

    [HttpPost]
    public JsonResult UserExists(string googleId)
    {
        return Json(Sl.UserRepo.GoogleUserExists(googleId));
    }

    [HttpPost]
    public JsonResult CreateAndLogin(GoogleUserCreateParameter googleUser)
    {
        var registerResult = _registerUser.Run(googleUser);

        if (registerResult.Success)
        {
            var user = Sl.UserRepo.UserGetByGoogleId(googleUser.GoogleId);
            SendRegistrationEmail.Run(user);
            WelcomeMsg.Send(user);
            _sessionUser.Login(user);
            var category = PersonalTopic.GetPersonalCategory(user);
            user.StartTopicId = category.Id;
            Sl.CategoryRepo.Create(category);
            _sessionUser.User.StartTopicId = category.Id;
        }

        return Json(new
        {
            success = true,
            CurrentUser = _vueSessionUser.GetCurrentUserData()
        });
    }

    public async Task<GoogleJsonWebSignature.Payload> GetGoogleUser(string token)
    {
        var settings = new GoogleJsonWebSignature.ValidationSettings()
        {
            Audience = new List<string>() { "290065015753-gftdec8p1rl8v6ojlk4kr13l4ldpabc8.apps.googleusercontent.com" }
        };

        try
        {
            return await GoogleJsonWebSignature.ValidateAsync(token, settings);
        }
        catch (InvalidJwtException e)
        {
            return null;
        }
    }
}