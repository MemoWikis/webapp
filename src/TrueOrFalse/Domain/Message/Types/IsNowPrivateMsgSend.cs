public class IsNowPrivateMsgSend
{
    public static void Run(int receiverId)
    {
        var user = MessageUtils.LoadUser(receiverId);            
    }
}