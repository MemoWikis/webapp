using System;
using System.Linq;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;

[SetMenu(MenuEntry.None)]
public class WidgetController : BaseController
{
    public ActionResult Question(int questionId)
    {
        var questionViewGuid = Guid.NewGuid();
        var question = R<QuestionRepo>().GetById(questionId);

        var answerQuestionModel = new AnswerQuestionModel(
            questionViewGuid, 
            question,
            new QuestionSearchSpec()
        );

        answerQuestionModel.DisableCommentLink = true;
        answerQuestionModel.QuestionViewGuid = questionViewGuid;

        Sl.SaveQuestionView.Run(questionViewGuid, question, _sessionUser.User);

        return View(
            "~/Views/Widgets/WidgetQuestion.aspx",
            new WidgetQuestionModel(answerQuestionModel));
    }

    public ActionResult Set(int setId)
    {
        var set = Sl.SetRepo.GetById(setId);
        var testSession = new TestSession(set);

        Sl.SessionUser.AddTestSession(testSession);

        return RedirectToAction("SetTestStep", "Widget", new {testSessionId = testSession.Id});
    }

    public ActionResult SetTestStep(int testSessionId)
    {
        return AnswerQuestionController.TestActionShared(testSessionId,
            testSession => RedirectToAction(
                Links.TestSessionResultAction,
                Links.TestSessionResultController,
                new {name = testSession.UriName, testSessionId = testSessionId}
            ),
            (testSession, questionViewGuid, question) => {
                var answerModel = new AnswerQuestionModel(testSession, questionViewGuid, question);
                answerModel.NextUrl = url => url.Action("SetTestStep", "Widget", new { testSessionId = testSession.Id });
                return View("~/Views/Widgets/WidgetSet.aspx", new WidgetSetModel(answerModel));
            }
        );
    }

    public ActionResult SetVideo(int setId)
    {
        var set = Sl.SetRepo.GetById(setId);

        return View("~/Views/Widgets/WidgetSetVideo.aspx", new WidgetSetVideoModel(set));
    }
}