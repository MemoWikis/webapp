using System.Collections.Generic;

public class WelcomeBoxTopSetsModel : BaseModel
{
    public IEnumerable<Set> Sets;

    public WelcomeBoxTopSetsModel()
    {
    }

    public static WelcomeBoxTopSetsModel CreateMostRecent(int amount)
    {
        var result = new WelcomeBoxTopSetsModel();
        var setRepo = Sl.R<SetRepo>();
        result.Sets = setRepo.GetMostRecent(amount);

        return result;
    }

    public static WelcomeBoxTopSetsModel CreateMostQuestions(int amount)
    {
        var result = new WelcomeBoxTopSetsModel();
        //var setRepo = Sl.R<SetRepo>();
        //result.Sets = setRepo.GetMostQuestions(amount);

        return result;
    }
}
