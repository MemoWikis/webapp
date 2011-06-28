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
            RightMenu.Yes = true;
            ShowLeftMenu_TopUsers();
            MostPopular = new List<Question>();
        }
    }
}
