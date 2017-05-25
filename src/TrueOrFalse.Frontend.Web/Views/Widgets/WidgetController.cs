using System;
using System.Web.Mvc;

[SetMenu(MenuEntry.None)]
public class WidgetController : BaseController
{
    public ActionResult Question(int questionId, bool? hideAddToKnowledge, string host)
    {
        var questionViewGuid = Guid.NewGuid();
        var question = R<QuestionRepo>().GetById(questionId);

        var answerQuestionModel = new AnswerQuestionModel(
            questionViewGuid, 
            question,
            new QuestionSearchSpec()
        );

        answerQuestionModel.DisableCommentLink = true;
        answerQuestionModel.IsInWidget = true;

        if (hideAddToKnowledge.HasValue)
            answerQuestionModel.DisableAddKnowledgeButton = hideAddToKnowledge.Value;

        answerQuestionModel.QuestionViewGuid = questionViewGuid;

        Sl.SaveQuestionView.Run(questionViewGuid, question, _sessionUser.User);

        return View(
            "~/Views/Widgets/WidgetQuestion.aspx",
            new WidgetQuestionModel(answerQuestionModel, host));
    }

    public ActionResult Set(int setId, bool? hideAddToKnowledge)
    {
        return View(
            "~/Views/Widgets/WidgetSetStart.aspx",
            new WidgetSetStartModel(setId, Convert.ToBoolean(hideAddToKnowledge)));
    }

    public ActionResult SetStart(int setId, bool? hideAddToKnowledge)
    {
        var set = Sl.SetRepo.GetById(setId);
        var testSession = new TestSession(set);

        if(hideAddToKnowledge.HasValue)
            testSession.HideAddKnowledge = hideAddToKnowledge.Value;

        Sl.SessionUser.AddTestSession(testSession);

        return RedirectToAction(
            "SetTestStep", 
            "Widget", 
            new {testSessionId = testSession.Id}
        );
    }

    public ActionResult SetTestStep(int testSessionId)
    {
        return AnswerQuestionController.TestActionShared(testSessionId,
            testSession => RedirectToAction("SetTestResult", "Widget", new {testSessionId = testSessionId}
            ),
            (testSession, questionViewGuid, question) => {
                var answerModel = new AnswerQuestionModel(testSession, questionViewGuid, question);
                answerModel.NextUrl = url => url.Action("SetTestStep", "Widget", new { testSessionId = testSession.Id });
                answerModel.IsInWidget = true;
                answerModel.DisableAddKnowledgeButton = testSession.HideAddKnowledge;

                return View("~/Views/Widgets/WidgetSet.aspx", new WidgetSetModel(answerModel));
            }
        );
    }

    public ActionResult SetTestResult(int testSessionId)
    {
        var testSession = TestSessionResultController.GetTestSession(testSessionId);
        var testSessionResultModel = new TestSessionResultModel(testSession);
        var setModel = new WidgetSetResultModel(testSessionResultModel);

        return View("~/Views/Widgets/WidgetSetResult.aspx", setModel);
    }


    public ActionResult SetVideo(int setId, bool? hideAddToKnowledge)
    {
        var set = Sl.SetRepo.GetById(setId);

        return View("~/Views/Widgets/WidgetSetVideo.aspx", new WidgetSetVideoModel(set, hideAddToKnowledge ?? false));
    }
}