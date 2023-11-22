public class CustomMsg
{
    public static void Send(int receiverId,
        string subject, 
        string body,
        MessageRepo messageRepo,
        UserReadingRepo userReadingRepo)
    {
        MessageUtils.LoadUser(receiverId, userReadingRepo);

        messageRepo.Create(new Message
        {
            ReceiverId = receiverId,
            Subject = subject,
            Body = body,
            MessageType = MessageTypes.Custom
        });
    }
}