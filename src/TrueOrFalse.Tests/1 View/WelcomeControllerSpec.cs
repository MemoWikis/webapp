using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Machine.Specifications;
using TrueOrFalse.Core;
using TrueOrFalse.Frontend.Web.Controllers;
using TrueOrFalse.View.Web;

namespace TrueOrFalse.Tests 
{

    public class When_showing_the_welcome_page : BaseTest
    {
        Because of = () => _welcomeController = new WelcomeController(null);
        It should_be_not_visible_the_left_menu = () => Resolve<ShowLeftMenu>().Yes(_welcomeController).ShouldBeFalse();

        static WelcomeController _welcomeController;
        static ActionResult _result;
    }
}
