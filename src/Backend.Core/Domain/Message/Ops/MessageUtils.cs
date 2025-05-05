public class MessageUtils
{
    public static User LoadUser(int receiverId, UserReadingRepo userReadingRepo)
    {
        var user = userReadingRepo.GetById(receiverId);

        if (user == null)
            throw new Exception("user '" + receiverId + "' not found");

        return user;
    }
}