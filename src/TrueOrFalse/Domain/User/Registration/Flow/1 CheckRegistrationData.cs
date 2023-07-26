public class IsEmailAddressAvailable
{
    public static bool Yes(string emailAddress, UserReadingRepo userReadingRepo){
        return userReadingRepo.GetByEmail(emailAddress.TrimAndReplaceWhitespacesWithSingleSpace()) == null;
    }
}

public class IsUserNameAvailable
{
    public static bool Yes(string userName, UserReadingRepo userReadingRepo)
    {
        return userReadingRepo.GetByName(userName.TrimAndReplaceWhitespacesWithSingleSpace()) == null;
    }
}