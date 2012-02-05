using System.Collections.Generic;
using TrueOrFalse.Core;
using TrueOrFalse.Frontend.Web.Models;


public class WelcomeModel : ModelBase
{
    public IList<Question> MostPopular;

    public WelcomeModel()
    {
        ShowLeftMenu_TopUsers();
        MostPopular = new List<Question>();
    }
}
