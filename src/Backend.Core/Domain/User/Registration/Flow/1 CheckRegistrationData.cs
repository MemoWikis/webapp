using System.Net.Mail;

public class IsEmailAddressAvailable
{
    public static bool Yes(string emailAddress, UserReadingRepo userReadingRepo)
    {
        return userReadingRepo.GetByEmail(emailAddress.TrimAndReplaceWhitespacesWithSingleSpace()) == null;
    }

    public static bool No(string emailAddress, UserReadingRepo userReadingRepo)
    {
        return !Yes(emailAddress, userReadingRepo);
    }
}

public class IsUserNameAvailable
{
    public static bool Yes(string userName, UserReadingRepo userReadingRepo)
    {
        return userReadingRepo.GetByName(userName.TrimAndReplaceWhitespacesWithSingleSpace()) == null;
    }

    public static bool No(string userName, UserReadingRepo userReadingRepo)
    {
        return !Yes(userName, userReadingRepo);
    }
}

public class IsEmailAdressFormat
{
    public static bool Valid(string email)
    {
        try
        {
            var addr = new MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    public static bool NotValid(string email)
    {
        return !Valid(email); 
    }
}

