public class KnowledgeWheelController : BaseController
{
    public string Get(int setId)
    {
        var set = Sl.SetRepo.GetById(setId);
        var knowledgeSummary = KnowledgeSummaryLoader.Run(UserId, set);
        return ViewRenderer.RenderPartialView("/Views/Knowledge/Wheel/KnowledgeWheel.ascx", knowledgeSummary, ControllerContext);
    }
}