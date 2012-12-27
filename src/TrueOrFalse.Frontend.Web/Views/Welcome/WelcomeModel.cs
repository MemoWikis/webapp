using System.Collections.Generic;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Models;


public class WelcomeModel : BaseModel
{
    public IList<Question> MostPopular;

    public WelcomeModel()
    {
        MostPopular = new List<Question>();
    }
}
