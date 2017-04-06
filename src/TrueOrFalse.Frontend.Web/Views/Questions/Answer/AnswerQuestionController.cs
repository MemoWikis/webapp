﻿using System;
using System.Linq;
using System.Web.Mvc;
using StackExchange.Profiling;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Search;
using TrueOrFalse.Web;

public class AnswerQuestionController : BaseController
{
    private readonly QuestionRepo _questionRepo;
    private readonly AnswerQuestion _answerQuestion;

    private const string _viewLocation = "~/Views/Questions/Answer/AnswerQuestion.aspx";
    private const string _viewLocationError = "~/Views/Questions/Answer/AnswerQuestionError.aspx";

    public AnswerQuestionController(QuestionRepo questionRepo, AnswerQuestion answerQuestion)
    {
        _questionRepo = questionRepo;
        _answerQuestion = answerQuestion;
    }

    [SetMenu(MenuEntry.QuestionDetail)]
    public ActionResult Answer(string text, int? id, int? elementOnPage, string pager, int? setId, int? questionId,
        string category)
    {
        if (setId != null && questionId != null)
            return AnswerSet((int) setId, (int) questionId);

        return AnswerQuestion(text, id, elementOnPage, pager, category);
    }

    public ActionResult Learn(int learningSessionId, string learningSessionName, int skipStepIdx = -1)
    {
        var learningSession = Sl.Resolve<LearningSessionRepo>().GetById(learningSessionId);

        if (learningSession.User != _sessionUser.User)
            throw new Exception("not logged in or not possessing user");

        if (skipStepIdx != -1 && learningSession.Steps.Any(s => s.Idx == skipStepIdx))
        {
            learningSession.SkipStep(skipStepIdx);
            return RedirectToAction("Learn", Links.AnswerQuestionController,
                new {learningSessionId, learningSessionName = learningSessionName});
        }

        var currentLearningStepIdx = learningSession.CurrentLearningStepIdx();

        if (currentLearningStepIdx == -1) //None of the steps is uncompleted
            return RedirectToAction("LearningSessionResult", Links.LearningSessionResultController,
                new {learningSessionId, learningSessionName = learningSessionName});

        if (learningSession.IsDateSession)
        {
            var trainingDateRepo = Sl.R<TrainingDateRepo>();
            var trainingDate = trainingDateRepo.GetByLearningSessionId(learningSessionId);

            if (trainingDate != null)
            {
                if (trainingDate.IsExpired())
                {
                    return RedirectToAction("StartLearningSession", Links.DatesController,
                        new {dateId = trainingDate.TrainingPlan.Date.Id});
                }

                trainingDate.ExpiresAt =
                    DateTime.Now.AddMinutes(TrainingDate.DateStaysOpenAfterNewBegunLearningStepInMinutes);
                trainingDateRepo.Update(trainingDate);
            }
        }

        var questionViewGuid = Guid.NewGuid();

        Sl.SaveQuestionView.Run(
            questionViewGuid,
            learningSession.Steps[currentLearningStepIdx].Question,
            _sessionUser.User.Id,
            learningSession: learningSession,
            learningSessionStepGuid: learningSession.Steps[currentLearningStepIdx].Guid);

        return View(_viewLocation,
            new AnswerQuestionModel(questionViewGuid, Sl.Resolve<LearningSessionRepo>().GetById(learningSessionId)));
    }

    public ActionResult Test(int testSessionId)
    {
        return TestActionShared(
            testSessionId,
            testSession => RedirectToAction(
                Links.TestSessionResultAction,
                Links.TestSessionResultController,
                new {name = testSession.UriName, testSessionId = testSessionId}
            ),
            (testSession, questionViewGuid, question) => View(
                _viewLocation,
                new AnswerQuestionModel(testSession, questionViewGuid, question))
        );
    }

    public static ActionResult TestActionShared(
        int testSessionId,
        Func<TestSession, ActionResult> redirectToFinalStepFunc,
        Func<TestSession, Guid, Question, ActionResult> resultFunc)
    {
        var sessionUser = Sl.SessionUser;

        var sessionCount = sessionUser.TestSessions.Count(s => s.Id == testSessionId);

        if (sessionCount == 0)
        {
            //Logg.r().Error("SessionCount 0");
            //return View(_viewLocation, AnswerQuestionModel.CreateExpiredTestSession());
            throw new Exception("SessionCount is 0. Shoult be 1");
        }

        if (sessionCount > 1)
            throw new Exception(
                $"SessionCount is {sessionUser.TestSessions.Count(s => s.Id == testSessionId)}. Should be not more then more than 1.");

        var testSession = sessionUser.TestSessions.Find(s => s.Id == testSessionId);

        if (testSession.CurrentStepIndex > testSession.NumberOfSteps)
            return redirectToFinalStepFunc(testSession);

        var question = Sl.R<QuestionRepo>().GetById(testSession.Steps.ElementAt(testSession.CurrentStepIndex - 1).QuestionId);
        var questionViewGuid = Guid.NewGuid();

        Sl.SaveQuestionView.Run(questionViewGuid, question, sessionUser.User);

        return resultFunc(testSession, questionViewGuid, question);
    }

    public ActionResult AnswerSet(int setId, int questionId)
    {
        var set = Resolve<SetRepo>().GetById(setId);
        var question = _questionRepo.GetById(questionId);
        return AnswerSet(set, question);
    }

    public ActionResult AnswerSet(Set set, Question question)
    {
        _sessionUiData
            .VisitedQuestions
            .Add(new QuestionHistoryItem(set, question));

        var questionViewGuid = Guid.NewGuid();
        Sl.SaveQuestionView.Run(questionViewGuid, question, _sessionUser.User);
        return View(_viewLocation, new AnswerQuestionModel(questionViewGuid, set, question));
    }

    public ActionResult AnswerQuestion(string text, int? id, int? elementOnPage, string pager, string category)
    {
        if (String.IsNullOrEmpty(pager) && ((elementOnPage == null) || (elementOnPage == -1)))
        {
            if (id == null)
                throw new Exception("AnswerQuestionController: No id for question provided.");

            var question2 = _questionRepo.GetById((int) id);

            if (question2 == null)
                throw new Exception("question not found");

            _sessionUiData.VisitedQuestions.Add(new QuestionHistoryItem(question2, HistoryItemType.Any));

            var questionViewGuid2 = Guid.NewGuid();
            Sl.SaveQuestionView.Run(questionViewGuid2, question2, _sessionUser.User);

            return View(_viewLocation, new AnswerQuestionModel(question2));
        }

        var activeSearchSpec = Resolve<QuestionSearchSpecSession>().ByKey(pager);

        if (!String.IsNullOrEmpty(category))
        {
            var categoryDb = R<CategoryRepository>().GetByName(category).FirstOrDefault();
            if (categoryDb != null)
            {
                activeSearchSpec.Filter.Categories.Clear();
                activeSearchSpec.Filter.Categories.Add(categoryDb.Id);
                activeSearchSpec.OrderBy.PersonalRelevance.Desc();
                activeSearchSpec.PageSize = 1;

                //set total count
                Sl.R<SearchQuestions>().Run(activeSearchSpec);
            }
        }

        if (text == null && id == null && elementOnPage == null)
            return GetViewBySearchSpec(activeSearchSpec);

        var question = _questionRepo.GetById((int) id);

        activeSearchSpec.PageSize = 1;
        if ((int) elementOnPage != -1)
            activeSearchSpec.CurrentPage = (int) elementOnPage;

        _sessionUiData.VisitedQuestions.Add(new QuestionHistoryItem(question, activeSearchSpec));

        var questionViewGuid = Guid.NewGuid();
        Sl.SaveQuestionView.Run(questionViewGuid, question, _sessionUser.User);

        return View(_viewLocation, new AnswerQuestionModel(questionViewGuid, question, activeSearchSpec));
    }

    public ActionResult Next(string pager)
    {
        var activeSearchSpec = Resolve<QuestionSearchSpecSession>().ByKey(pager);
        activeSearchSpec.NextPage(1);
        return GetViewBySearchSpec(activeSearchSpec);
    }

    public ActionResult Previous(string pager)
    {
        var activeSearchSpec = Resolve<QuestionSearchSpecSession>().ByKey(pager);
        activeSearchSpec.PreviousPage(1);
        return GetViewBySearchSpec(activeSearchSpec);
    }

    private ActionResult GetViewBySearchSpec(QuestionSearchSpec searchSpec)
    {

        using (MiniProfiler.Current.Step("GetViewBySearchSpec"))
        {
            var question = AnswerQuestionControllerSearch.Run(searchSpec);

            if (searchSpec.HistoryItem != null)
            {
                if (searchSpec.HistoryItem.Question != null)
                {
                    if (searchSpec.HistoryItem.Question.Id != question.Id)
                    {
                        question = Resolve<QuestionRepo>().GetById(searchSpec.HistoryItem.Question.Id);
                    }
                }

                searchSpec.HistoryItem = null;
            }

            return RedirectToAction("Answer", Links.AnswerQuestionController,
                new
                {
                    text = UriSegmentFriendlyQuestion.Run(question.Text),
                    id = question.Id,
                    elementOnPage = searchSpec.CurrentPage,
                    pager = searchSpec.Key,
                    category = ""
                });
        }
    }

    [HttpPost]
    public JsonResult SendAnswer(int id, string answer, Guid questionViewGuid, int interactionNumber,
        int millisecondsSinceQuestionView)
    {
        var result = _answerQuestion.Run(id, answer, UserId, questionViewGuid, interactionNumber,
            millisecondsSinceQuestionView);
        var question = _questionRepo.GetById(id);
        var solution = GetQuestionSolution.Run(question);
        return new JsonResult
        {
            Data = new
            {
                correct = result.IsCorrect,
                correctAnswer = result.CorrectAnswer,
                choices = solution.GetType() == typeof(QuestionSolutionMultipleChoice_SingleSolution)
                    ? ((QuestionSolutionMultipleChoice_SingleSolution) solution).Choices
                    : null
            }
        };
    }

    [HttpPost]
    public JsonResult SendAnswerLearningSession(int id,
        int learningSessionId,
        Guid questionViewGuid,
        int interactionNumber,
        Guid stepGuid,
        string answer,
        int millisecondsSinceQuestionView)
    {
        //var timeOfAnswer = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(timeOfAnswerString));

        var result = _answerQuestion.Run(id, answer, UserId, questionViewGuid, interactionNumber,
            millisecondsSinceQuestionView, learningSessionId, stepGuid);
        var question = _questionRepo.GetById(id);
        var solution = GetQuestionSolution.Run(question);

        return new JsonResult
        {
            Data = new
            {
                correct = result.IsCorrect,
                correctAnswer = result.CorrectAnswer,
                choices = solution.GetType() == typeof(QuestionSolutionMultipleChoice_SingleSolution)
                    ? ((QuestionSolutionMultipleChoice_SingleSolution) solution).Choices
                    : null,
                newStepAdded = result.NewStepAdded,
                numberSteps = result.NumberSteps,
            }
        };
    }

    [HttpPost]
    public JsonResult AmendAfterShowSolution(int learningSessionId, Guid stepGuid)
    {
        var learningSessionStep = LearningSession.GetStep(learningSessionId, stepGuid);
        var learningSession = Sl.R<LearningSessionRepo>().GetById(learningSessionId);

        learningSessionStep.AnswerState = StepAnswerState.ShowedSolutionOnly;

        learningSession.UpdateAfterWrongAnswerOrShowSolution(learningSessionStep);

        return new JsonResult
        {
            Data = new
            {
                numberSteps = learningSession.Steps.Count
            }
        };
    }

    [HttpPost]
    public JsonResult GetSolution(int id, string answer, Guid questionViewGuid, int interactionNumber, int? roundId,
        int millisecondsSinceQuestionView = -1)
    {
        var question = _questionRepo.GetById(id);
        var solution = GetQuestionSolution.Run(question);

        if (IsLoggedIn)
            if (roundId == null)
                R<AnswerLog>()
                    .LogAnswerView(question, this.UserId, questionViewGuid, interactionNumber,
                        millisecondsSinceQuestionView);
            else
                R<AnswerLog>()
                    .LogAnswerView(question, this.UserId, questionViewGuid, interactionNumber,
                        millisecondsSinceQuestionView, roundId);

        return new JsonResult
        {
            Data = new
            {
                correctAnswerAsHTML = solution.GetCorrectAnswerAsHtml(),
                correctAnswer = solution.GetCorrectAnswerAsHtml(),
                correctAnswerDesc = MarkdownInit.Run().Transform(question.Description),
                correctAnswerReferences = question.References.Select(r => new
                {
                    referenceId = r.Id,
                    categoryId = r.Category?.Id ?? -1,
                    referenceType = r.ReferenceType.GetName(),
                    additionalInfo = r.AdditionalInfo ?? "",
                    referenceText = r.ReferenceText ?? ""
                })
            }
        };
    }

    [HttpPost]
    public void LogTimeForQuestionView(Guid questionViewGuid, int millisecondsSinceQuestionView) => 
        Sl.SaveQuestionView.LogOverallTime(questionViewGuid, millisecondsSinceQuestionView);

    [HttpPost]
    public void CountLastAnswerAsCorrect(int id, Guid questionViewGuid, int interactionNumber, int? testSessionId) => 
        _answerQuestion.Run(id, _sessionUser.UserId, questionViewGuid, interactionNumber, testSessionId, countLastAnswerAsCorrect: true);

    [HttpPost]
    public void CountUnansweredAsCorrect(int id, Guid questionViewGuid, int interactionNumber, int millisecondsSinceQuestionView, int? testSessionId) => 
        _answerQuestion.Run(id, _sessionUser.UserId, questionViewGuid, interactionNumber, testSessionId, millisecondsSinceQuestionView, countUnansweredAsCorrect: true);

    public ActionResult PartialAnswerHistory(int questionId)
    {
        var question = _questionRepo.GetById(questionId);

        var questionValuationForUser =
            NotNull.Run(Resolve<QuestionValuationRepo>().GetBy(question.Id, _sessionUser.UserId));
        var valuationForUser = Resolve<TotalsPersUserLoader>().Run(_sessionUser.UserId, question.Id);

        return View("HistoryAndProbability",
            new HistoryAndProbabilityModel
            {
                LoadJs = true,
                AnswerHistory = new AnswerHistoryModel(question, valuationForUser),
                CorrectnessProbability = new CorrectnessProbabilityModel(question, questionValuationForUser),
                QuestionValuation = questionValuationForUser
            }
        );
    }

    public string RenderAnswerBody(int questionId, string pager, bool? isMobileDevice, int? testSessionId = null, int? learningSessionId = null)
    {
        if (learningSessionId != null)
        {
            var learningSession = Sl.Resolve<LearningSessionRepo>().GetById((int) learningSessionId);
            ControllerContext.RouteData.Values.Add("learningSessionId", learningSessionId);
            ControllerContext.RouteData.Values.Add("learningSessionName", learningSession.UrlName);
            var learningSessionQuestionViewGuid = Guid.NewGuid();
            return ViewRenderer.RenderPartialView(
                "~/Views/Questions/Answer/AnswerBodyControl/AnswerBody.ascx",
                new AnswerBodyModel(new AnswerQuestionModel(learningSessionQuestionViewGuid, learningSession, isMobileDevice)),
                ControllerContext
            );
        }
        if (testSessionId != null)
        {
            var sessionUser = Sl.SessionUser;
            var testSession = sessionUser.TestSessions.Find(s => s.Id == testSessionId);
            var testSessionQuestion = Sl.QuestionRepo.GetById(testSession.Steps.ElementAt(testSession.CurrentStepIndex - 1).QuestionId);
            var testSessionQuestionViewGuid = Guid.NewGuid();
            var testSessionName = testSession.UriName;

            ControllerContext.RouteData.Values.Add("testSessionId", testSessionId);
            ControllerContext.RouteData.Values.Add("name", testSessionName);

            return ViewRenderer.RenderPartialView(
                "~/Views/Questions/Answer/AnswerBodyControl/AnswerBody.ascx",
                new AnswerBodyModel(new AnswerQuestionModel(testSession, testSessionQuestionViewGuid, testSessionQuestion, isMobileDevice)),
                ControllerContext
            );
        }
        //for normal questions
        var question = Sl.QuestionRepo.GetById(questionId);
        var activeSearchSpec = Resolve<QuestionSearchSpecSession>().ByKey(pager);
        var questionViewGuid = Guid.NewGuid();
        return ViewRenderer.RenderPartialView(
            "~/Views/Questions/Answer/AnswerBodyControl/AnswerBody.ascx",
            new AnswerBodyModel(new AnswerQuestionModel(questionViewGuid, question, activeSearchSpec, isMobileDevice)),
            ControllerContext
        );
    }

    public EmptyResult ClearHistory()
    {
        _sessionUiData.VisitedQuestions = new QuestionHistory();
        return new EmptyResult();
    }

    public string ShareQuestionModal(int questionId) =>
        ViewRenderer.RenderPartialView("~/Views/Questions/Answer/ShareQuestionModal.ascx", new ShareQuestionModalModel(questionId), ControllerContext);
}