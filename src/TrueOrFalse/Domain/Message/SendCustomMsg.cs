using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueOrFalse
{
    public class SendCustomMsg : BaseSendMessage, IRegisterAsInstancePerLifetime
    {
        public SendCustomMsg(MessageRepository messageRepo, UserRepository userRepo) : base(messageRepo, userRepo){}

        public void Run(int receiverId, string subject, string body)
        {
            var user = LoadUser(receiverId);

            _messageRepo.Create(new Message
            {
                ReceiverId = receiverId,
                Subject = subject,
                Body = body,
                MessageType = ""
            });
        }
    }
}
