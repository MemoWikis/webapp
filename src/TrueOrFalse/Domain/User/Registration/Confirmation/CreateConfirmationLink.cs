﻿using System;

public class CreateEmailConfirmationLink
{
    public static string Run(User user){
        return String.Format("https://memucho.de/EmailBestaetigen/x7b" + user.Id);
    }
}