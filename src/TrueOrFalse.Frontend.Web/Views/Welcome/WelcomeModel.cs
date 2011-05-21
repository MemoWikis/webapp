using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrueOrFalse.Core;

namespace TrueOrFalse.Frontend.Web.Models
{
    public class WelcomeModel : ModelBase
    {
        public IList<Question> MostPopular;

        public WelcomeModel()
        {
            ShowRightLogin.Yes = true;
            ShowLeftMenu = false;
        }
    }
}
