public class WelcomeMsg
{
    public static void Send(int receiverId, UserReadingRepo userReadingRepo, MessageRepo messageRepo)
    {
        var user = MessageUtils.LoadUser(receiverId, userReadingRepo);
        Send(user, messageRepo);
    }

    public static void Send(User user, MessageRepo messageRepo)
    {
        string body = String.Format(@"
            <p>Hallo {0}, </p>
            <p>wir begrüßen dich herzlich bei memoWikis!</p>

            <p>memoWikis ist in der Beta-Phase, es gibt noch viel zu tun und wir haben viel vor. Du kannst uns unterstützen, indem du deinen Freunden von uns erzählst und Fehler und Verbesserungsvorschläge an uns weiterleitest. Solltest du irgendwelche Fragen haben, helfen wir Dir gerne.</p>

            <p>Viele Grüße,<br>
            Robert</p>
            <p style='font-size: 12px; margin-top: 20px'>E-Mail: {1} | Telefon: +49-178 186 68 48 (Robert)</p>
            ", user.Name, Settings.EmailToMemoWikis);

        messageRepo.Create(new Message
        {
            ReceiverId = user.Id,
            Subject = "Willkommen bei memoWikis",
            Body = body,
            MessageType = MessageTypes.Welcome
        });
    }
}