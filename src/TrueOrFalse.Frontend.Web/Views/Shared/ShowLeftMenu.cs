using System.Web.Mvc;

public class ShowLeftMenu 
{
    public static bool Yes(Controller controller)
    {
        return controller.GetType() != typeof (WelcomeController);
    }
}
