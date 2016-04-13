public class IsNowPrivateMsg
{
    public static void Send(int receiverId)
    {
        var user = MessageUtils.LoadUser(receiverId);            
    }
}