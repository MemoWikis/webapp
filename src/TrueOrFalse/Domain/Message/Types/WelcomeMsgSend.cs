using System;

public class WelcomeMsgSend
{
    public static void Run(int receiverId)
    {
        var user = MessageUtils.LoadUser(receiverId);
        Run(user);
    }

    public static void Run(User user)
    {
        string body = String.Format(@"
<p>Hallo {0}, </p>
<p>wir begrüßen dich herzlich bei MEMuchO. Solltest du irgendwelche Fragen haben, helfen wir dir gerne.</p>

<p>Viele Grüße<br>
Jule & Robert</p>
", user.Name);

        Sl.R<MessageRepo>().Create(new Message{
            ReceiverId = user.Id,
            Subject = "Willkommen bei MEMuchO",
            Body = body,
            MessageType = MessageTypes.Welcome
        });
    }
}