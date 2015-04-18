public class SendIsNowPrivateMsg : BaseSendMessage, IRegisterAsInstancePerLifetime
{
    public SendIsNowPrivateMsg(MessageRepository messageRepo, UserRepository userRepo) : base(messageRepo, userRepo)
    {
    }

    public void Run(int receiverId)
    {
        var user = LoadUser(receiverId);            
    }
}