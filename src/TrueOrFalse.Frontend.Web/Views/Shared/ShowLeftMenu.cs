using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse.Core.Infrastructure;
using TrueOrFalse.Frontend.Web.Controllers;

namespace TrueOrFalse.View.Web
{
    /// <summary>
    /// LogicModule: Answers, if the left menu should be visible on the Page
    /// </summary>
    public class ShowLeftMenu : IRegisterAsInstancePerLifetime
    {
        public bool Yes(Controller controller)
        {
            return controller.GetType() != typeof (WelcomeController);
        }
    }
}