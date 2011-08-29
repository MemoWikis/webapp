using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrueOrFalse.Frontend.Web.Code;

namespace TrueOrFalse.Frontend.Web.Models
{
    public class ModelBase
    {
        public readonly RightMenu RightMenu = new RightMenu(false);

        public string CurrentLeftMenu;

        public ModelBase ShowLeftMenu_Nav() { CurrentLeftMenu = UserControls.MenuLeft; return this; }
        public ModelBase ShowLeftMenu_TopUsers() { CurrentLeftMenu = UserControls.TopUsers; return this; }
        public ModelBase ShowLeftMenu_Empty() { CurrentLeftMenu = null; return this; }

        public ModelBase ShowRightLogin(){ RightMenu.Yes = true; return this; }

        public bool MainFullWidth;
    }
}