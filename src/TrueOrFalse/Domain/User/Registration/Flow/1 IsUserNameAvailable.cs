public class IsEmailAddressAvailable
{
    public static bool Yes(string emailAddress){
        return Sl.R<UserRepo>().GetByEmail(emailAddress) == null;
    }
}