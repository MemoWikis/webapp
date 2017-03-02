using System.Web.Mvc;

public class ShowLeftMenu 
{
    public static bool Yes(Controller controller) => 
        controller.GetType() != typeof (WelcomeController);
}
