public class CustomMsg
{
    public static void Send(int receiverId,
        string subject, 
        string body,
        MessageRepo messageRepo,
        UserRepo userRepo)
    {
        MessageUtils.LoadUser(receiverId, userRepo);

        messageRepo.Create(new Message
        {
            ReceiverId = receiverId,
            Subject = subject,
            Body = body,
            MessageType = MessageTypes.Custom
        });
    }
}