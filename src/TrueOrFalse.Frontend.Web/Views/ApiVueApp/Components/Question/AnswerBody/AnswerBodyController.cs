using System.Linq;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using TrueOrFalse.Web;

[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
public class AnswerBodyController: BaseController
{
    [HttpGet]
    public JsonResult Get(int id)
    {
        var question = EntityCache.GetQuestion(id);

        if (!PermissionCheck.CanView(question))
        {
            return Json(new
            {
                success = false
            }, JsonRequestBehavior.AllowGet);
        }
        var learningSession = LearningSessionCache.GetLearningSession();
        var model = new
        {
            questionViewGuid = learningSession.QuestionViewGuid,
        };
        return Json(new
        {

        }, JsonRequestBehavior.AllowGet);
    }
}