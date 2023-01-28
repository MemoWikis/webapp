using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using TrueOrFalse;
using TrueOrFalse.Web;

[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
public class AnswerBodyController: BaseController
{
    [HttpGet]
    public JsonResult Get(int index)
    {
        var learningSession = LearningSessionCache.GetLearningSession();
        var step = learningSession.Steps[index];

        var q = step.Question;
        var primaryCategory = q.Categories.LastOrDefault();
        var model = new
        {
            id = q.Id,
            text = q.Text,
            title = Regex.Replace(q.Text, "<.*?>", String.Empty),
            solutionType = q.SolutionType,
            renderedQuestionTextExtended = q.TextExtended != null ? MarkdownMarkdig.ToHtml(q.TextExtended) : "",
            description = q.Description,
            hasTopics = q.Categories.Any(),
            primaryTopicUrl = primaryCategory?.Url,
            primaryTopicName = primaryCategory?.Name,
            solution = q.Solution,

            isCreator = q.Creator.Id = SessionUser.UserId,
            isInWishKnowledge = SessionUser.IsLoggedIn && q.IsInWishknowledge(),

            questionViewGuid = Guid.NewGuid(),
            isLastStep = learningSession.Steps.Last() == step


        };
        return Json(model, JsonRequestBehavior.AllowGet);
    }
}