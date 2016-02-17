public class CustomMsg
{
    public static void Send(int receiverId, string subject, string body)
    {
        MessageUtils.LoadUser(receiverId);

        Sl.R<MessageRepo>().Create(new Message
        {
            ReceiverId = receiverId,
            Subject = subject,
            Body = body,
            MessageType = MessageTypes.Custom
        });
    }
}