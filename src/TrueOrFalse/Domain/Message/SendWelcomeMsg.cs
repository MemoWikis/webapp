using System;

public class SendWelcomeMsg : BaseSendMessage, IRegisterAsInstancePerLifetime
{
    public SendWelcomeMsg(MessageRepository messageRepo, UserRepository userRepo)
        : base(messageRepo, userRepo)
    {
    }

    public void Run(int receiverId)
    {
        var user = LoadUser(receiverId);
        Run(user);
    }

    public void Run(User user)
    {
        string body = String.Format(@"
<p>Hallo {0}, </p>
<p>wir begrüßen dich herzlich bei MEMuchO. Solltest du irgendwelche Fragen haben, helfen wir dir gerne.</p>

<p>Viele Grüße<br>
Jule & Robert</p>
", user.Name);

        _messageRepo.Create(new Message{
            ReceiverId = user.Id,
            Subject = "Willkommen bei MEMuchO",
            Body = body,
            MessageType = "Welcome"
        });
    }
}