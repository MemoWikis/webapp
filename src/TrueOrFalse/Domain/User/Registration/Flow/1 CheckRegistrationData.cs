public class IsEmailAddressAvailable
{
    public static bool Yes(string emailAddress, UserRepo userRepo){
        return userRepo.GetByEmail(emailAddress.TrimAndReplaceWhitespacesWithSingleSpace()) == null;
    }
}

public class IsUserNameAvailable
{
    public static bool Yes(string userName, UserRepo userRepo)
    {
        return userRepo.GetByName(userName.TrimAndReplaceWhitespacesWithSingleSpace()) == null;
    }
}