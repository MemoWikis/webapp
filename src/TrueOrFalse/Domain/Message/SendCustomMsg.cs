public class SendCustomMsg : BaseSendMessage, IRegisterAsInstancePerLifetime
{
    public SendCustomMsg(MessageRepository messageRepo, UserRepository userRepo) : base(messageRepo, userRepo){}

    public void Run(int receiverId, string subject, string body)
    {
        LoadUser(receiverId);

        _messageRepo.Create(new Message
        {
            ReceiverId = receiverId,
            Subject = subject,
            Body = body,
            MessageType = ""
        });
    }
}