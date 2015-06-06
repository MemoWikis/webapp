using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs042
    {
        public static void Run()
        {
            var sendWelcomMsg = Sl.Resolve<SendWelcomeMsg>();

            foreach (var user in Sl.Resolve<UserRepository>().GetAll())
                sendWelcomMsg.Run(user);
        }
    }
}
