﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Models;

public class UserModel : BaseModel
{
    public string Name { get; private set; }
    public int AmountCreatedQuestions;

    public string ImageUrl_200;
    public bool ImageIsCustom;

    public bool IsCurrentUser;

    public UserModel(User user)
    {
        Name = user.Name;

        AmountCreatedQuestions = Resolve<UserSummary>().AmountCreatedQuestions(user.Id);
    }
}
