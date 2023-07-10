public class MessageUtils
{
    public static User LoadUser(int receiverId, UserRepo userRepo)
    {
        var user = userRepo.GetById(receiverId);

        if (user == null)
            throw new Exception("user '" + receiverId + "' not found");

        return user;
    }
}