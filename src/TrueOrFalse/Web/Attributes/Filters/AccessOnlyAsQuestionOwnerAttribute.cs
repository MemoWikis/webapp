using System;
using System.Web.Mvc;

/// <summary>
/// Allows access only if current user is the owner of the question addressed by the current action.
/// Expects a parameter questionId.
/// </summary>
public class AccessOnlyAsQuestionOwnerAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        var questionId = (int)filterContext.ActionParameters["questionId"];
        var question = Sl.QuestionRepo.GetById(questionId);
        if (question.Visibility != QuestionVisibility.All)
            if (question.Creator.Id != Sl.SessionUser.UserId)
                throw new Exception("AccessOnlyAsQuestionOwner: Invalid access to question with id " + question.Id);
        
        base.OnActionExecuting(filterContext);
    }
}