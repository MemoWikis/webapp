using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueOrFalse
{
    public class BaseSendMessage
    {
        protected readonly MessageRepository _messageRepo;
        protected readonly UserRepository _userRepo;

        public BaseSendMessage(MessageRepository messageRepo, UserRepository userRepo)
        {
            _messageRepo = messageRepo;
            _userRepo = userRepo;
        }

        protected User LoadUser(int receiverId)
        {
            var user = _userRepo.GetById(receiverId);

            if (user == null)
                throw new Exception("user '" + receiverId + "' not found");

            return user;
        }
    }
}
