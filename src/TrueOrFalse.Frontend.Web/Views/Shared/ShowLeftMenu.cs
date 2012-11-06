using System.Web.Mvc;
using TrueOrFalse;
using TrueOrFalse.Infrastructure;

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
