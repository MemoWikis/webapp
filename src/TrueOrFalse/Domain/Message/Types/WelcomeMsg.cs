﻿public class WelcomeMsg
{
    public static void Send(int receiverId, UserRepo userRepo, MessageRepo messageRepo)
    {
        var user = MessageUtils.LoadUser(receiverId, userRepo);
        Send(user, messageRepo);
    }

    public static void Send(User user, MessageRepo messageRepo)
    {
        string body = String.Format(@"
            <p>Hallo {0}, </p>
            <p>wir begrüßen dich herzlich bei memucho!</p>

            <p>memucho ist in der Beta-Phase, es gibt noch viel zu tun und wir haben viel vor. Du kannst uns unterstützen, indem du deinen Freunden von uns erzählst und Fehler und Verbesserungsvorschläge an uns weiterleitest (nutze dazu am besten das Feedback-Werkzeug in der Mitte des rechten Bildschirm-Rands). Solltest du irgendwelche Fragen haben, helfen wir dir gerne.</p>

            <p>Viele Grüße,<br>
            Jule & Robert</p>
            <p style='font-size: 12px; margin-top: 20px'>E-Mail: team@memucho.de | Telefon: +49-178 186 68 48 (Robert)</p>
            ", user.Name);

        messageRepo.Create(new Message
        {
            ReceiverId = user.Id,
            Subject = "Willkommen bei memucho",
            Body = body,
            MessageType = MessageTypes.Welcome
        });
    }
}