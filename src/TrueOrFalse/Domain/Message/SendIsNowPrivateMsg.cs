using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueOrFalse
{
    public class SendIsNowPrivateMsg : BaseSendMessage, IRegisterAsInstancePerLifetime
    {
        public SendIsNowPrivateMsg(MessageRepository messageRepo, UserRepository userRepo) : base(messageRepo, userRepo)
        {
        }

        public void Run(int receiverId)
        {
            var user = LoadUser(receiverId);

            
        }
    }
}