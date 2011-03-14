using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrueOrFalse.Core.Infrastructure;

namespace TrueOrFalse.View.Web
{
    /// <summary>
    /// LogicModule: Answers, if the left menu should be visible on the Page
    /// </summary>
    public class ShowLeftMenu : IRegisterAsInstancePerLifetime
    {
        public bool Yes()
        {
            return true;
        }
    }
}