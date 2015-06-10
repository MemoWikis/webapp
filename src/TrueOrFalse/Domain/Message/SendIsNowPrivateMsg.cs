public class SendIsNowPrivateMsg : BaseSendMessage, IRegisterAsInstancePerLifetime
{
    public SendIsNowPrivateMsg(MessageRepository messageRepo, UserRepo userRepo) : base(messageRepo, userRepo)
    {
    }

    public void Run(int receiverId)
    {
        var user = LoadUser(receiverId);            
    }
}