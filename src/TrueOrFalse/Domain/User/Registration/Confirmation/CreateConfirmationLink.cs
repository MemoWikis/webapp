﻿public class CreateEmailConfirmationLink
{
    public static string Run(User user)
    {
        return String.Format(Settings.BaseUrl + "/EmailBestaetigen/" + EmailConfirmationService.CreateEmailConfirmationToken(user));
    }
}