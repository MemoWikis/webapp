using System.Web.Mvc;

public class SetController : BaseController
{
    private const string _viewLocation = "~/Views/Sets/Detail/Set.aspx";

    [SetMenu(MenuEntry.QuestionSetDetail)]
    public ActionResult QuestionSet(string text, int id, int elementOnPage)
    {
        return QuestionSetById(id);
    }

    [SetMenu(MenuEntry.QuestionSetDetail)]
    public ActionResult QuestionSetById(int id)
    {
        var set = Resolve<SetRepo>().GetById(id);
        _sessionUiData.VisitedSets.Add(new QuestionSetHistoryItem(set));

        return View(_viewLocation, new SetModel(set));        
    }
}

