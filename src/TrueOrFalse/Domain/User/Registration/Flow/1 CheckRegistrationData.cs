public class IsEmailAddressAvailable
{
    public static bool Yes(string emailAddress){
        return Sl.R<UserRepo>().GetByEmail(emailAddress.TrimAndReplaceWhitespacesWithSingleSpace()) == null;
    }
}

public class IsUserNameAvailable
{
    public static bool Yes(string userName)
    {
        return Sl.R<UserRepo>().GetByName(userName.TrimAndReplaceWhitespacesWithSingleSpace()) == null;
    }
}