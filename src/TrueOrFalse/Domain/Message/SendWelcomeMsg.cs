using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueOrFalse
{
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
<p>wir begrüßen Dich herzlich bei MEMuchO. Solltest Du irgendwelche Fragen haben, helfen wir Dir gerne.</p>

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
}