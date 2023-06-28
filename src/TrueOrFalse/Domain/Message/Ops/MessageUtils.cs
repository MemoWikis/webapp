public class MessageUtils
{
    public static User LoadUser(int receiverId)
    {
        var user = Sl.R<UserRepo>().GetById(receiverId);

        if (user == null)
            throw new Exception("user '" + receiverId + "' not found");

        return user;
    }
}