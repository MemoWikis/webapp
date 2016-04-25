using System;

public class WelcomeMsg
{
    public static void Send(int receiverId)
    {
        var user = MessageUtils.LoadUser(receiverId);
        Send(user);
    }

    public static void Send(User user)
    {
        string body = String.Format(@"
<p>Hallo {0}, </p>
<p>wir begrüßen dich herzlich bei memucho. Solltest du irgendwelche Fragen haben, helfen wir dir gerne.</p>

<p>Viele Grüße<br>
Jule & Robert</p>
", user.Name);

        Sl.R<MessageRepo>().Create(new Message{
            ReceiverId = user.Id,
            Subject = "Willkommen bei memucho",
            Body = body,
            MessageType = MessageTypes.Welcome
        });
    }
}