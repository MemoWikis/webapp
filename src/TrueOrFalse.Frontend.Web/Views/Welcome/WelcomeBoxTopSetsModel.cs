using System.Collections.Generic;
using System.Linq;

public class WelcomeBoxTopSetsModel : BaseModel
{
    public IEnumerable<TopSetResult> TopSets;

    public WelcomeBoxTopSetsModel(IEnumerable<Set> getMostRecent)
    {
        TopSets = getMostRecent.Select(set => new TopSetResult
        {
            Name = set.Name,
            QCount = set.QuestionsInSet.Count,
            SetId = set.Id,
            Text = set.Text
        });
    }

    private WelcomeBoxTopSetsModel(IEnumerable<TopSetResult> getMostQuestions)
    {
        TopSets = getMostQuestions;
    }

    public static WelcomeBoxTopSetsModel CreateMostRecent(int amount)
    {
        var setRepo = Sl.R<SetRepo>();
        return new WelcomeBoxTopSetsModel(setRepo.GetMostRecent(amount));
    }

    public static WelcomeBoxTopSetsModel CreateMostQuestions(int amount)
    {
        var setRepo = Sl.R<SetRepo>();
        return new WelcomeBoxTopSetsModel(setRepo.GetMostQuestions(amount));
    }
}
