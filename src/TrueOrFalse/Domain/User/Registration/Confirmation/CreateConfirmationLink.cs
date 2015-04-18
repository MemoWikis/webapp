using System;

public class CreateEmailConfirmationLink : IRegisterAsInstancePerLifetime
{
    public string Run(User user){
        return String.Format("http://memucho.de/EmailConfirmation/x7b" + user.Id);
    }
}