using System;
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
    private readonly SaveQuestionView _saveQuestionView;
    private const string _viewLocation = "~/Views/Questions/Answer/AnswerQuestion.aspx";

    public AnswerQuestionController(QuestionRepo questionRepo,
                                    AnswerQuestion answerQuestion,
                                    SaveQuestionView saveQuestionView)
    {
        _questionRepo = questionRepo;
        _answerQuestion = answerQuestion;
        _saveQuestionView = saveQuestionView;
    }


    [SetMenu(MenuEntry.QuestionDetail)]
    public ActionResult Answer(string text, int? id, int? elementOnPage, string pager, int? setId, int? questionId, string category)
    {
        if (setId != null && questionId != null)
            return AnswerSet((int)setId, (int)questionId);

        return AnswerQuestion(text, id, elementOnPage, pager, category);
    }

    public ActionResult Learn(int learningSessionId, string learningSessionName, int skipStepIdx = -1)
    {
        var learningSession = Sl.Resolve<LearningSessionRepo>().GetById(learningSessionId);

        if(learningSession.User != _sessionUser.User)
            throw new Exception("not logged in or not possessing user");

        if (skipStepIdx != -1 && learningSession.Steps.Any(s => s.Idx == skipStepIdx))
        {
            learningSession.SkipStep(skipStepIdx);
            return RedirectToAction("Learn", Links.AnswerQuestionController, 
                new { learningSessionId, learningSessionName = learningSessionName });
        }

        var currentLearningStepIdx = learningSession.CurrentLearningStepIdx();

        if (currentLearningStepIdx == -1) //None of the steps is uncompleted
            return RedirectToAction("LearningSessionResult", Links.LearningSessionResultController,
                new { learningSessionId, learningSessionName = learningSessionName });

        if (learningSession.IsDateSession)
        {
            var trainingDateRepo = Sl.R<TrainingDateRepo>();
            var trainingDate = trainingDateRepo.GetByLearningSessionId(learningSessionId);

            if (trainingDate != null)
            {
                if (trainingDate.IsExpired())
                {
                    return RedirectToAction("StartLearningSession", Links.DatesController,
                    new { dateId = trainingDate.TrainingPlan.Date.Id });
                }

                trainingDate.ExpiresAt =
                    DateTime.Now.AddMinutes(TrainingDate.DateStaysOpenAfterNewBegunLearningStepInMinutes);
                trainingDateRepo.Update(trainingDate);
            }
        }

        var questionViewGuid = Guid.NewGuid();

        _saveQuestionView.Run(
            questionViewGuid,
            learningSession.Steps[currentLearningStepIdx].Question,
            _sessionUser.User.Id,
            learningSession: learningSession,
            learningSessionStepGuid: learningSession.Steps[currentLearningStepIdx].Guid);

        return View(_viewLocation, new AnswerQuestionModel(questionViewGuid, Sl.Resolve<LearningSessionRepo>().GetById(learningSessionId)));
    }

    public ActionResult AnswerSet(int setId, int questionId)
    {
        var set = Resolve<SetRepo>().GetById(setId);
        var question = _questionRepo.GetById(questionId);
        return AnswerSet(set, question);
    }

    public ActionResult AnswerSet(Set set, Question question )
    {
        _sessionUiData
            .VisitedQuestions
            .Add(new QuestionHistoryItem(set, question));

        var questionViewGuid = Guid.NewGuid();
        _saveQuestionView.Run(questionViewGuid, question, _sessionUser.User);
        return View(_viewLocation, new AnswerQuestionModel(questionViewGuid, set, question));
    }

    public ActionResult AnswerQuestion(string text, int? id, int? elementOnPage, string pager, string category)
    {
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

        var question = _questionRepo.GetById((int)id);

        activeSearchSpec.PageSize = 1;
        if ((int)elementOnPage != -1)
            activeSearchSpec.CurrentPage = (int)elementOnPage;

        _sessionUiData.VisitedQuestions.Add(new QuestionHistoryItem(question, activeSearchSpec));

        var questionViewGuid = Guid.NewGuid();
        _saveQuestionView.Run(questionViewGuid, question, _sessionUser.User);

        return View(_viewLocation, new AnswerQuestionModel(questionViewGuid, question, activeSearchSpec));
    }

    public ActionResult Next(string pager, int? setId, int? questionId)
    {
        if (setId != null && questionId != null){
            var set = Resolve<SetRepo>().GetById((int)setId);
            return AnswerSet(set, set.QuestionsInSet.GetNextTo((int) questionId).Question);
        }

        var activeSearchSpec = Resolve<QuestionSearchSpecSession>().ByKey(pager);
        activeSearchSpec.NextPage(1);
        return GetViewBySearchSpec(activeSearchSpec);
    }

    public ActionResult Previous(string pager, int? setId, int? questionId)
    {
        if (setId != null && questionId != null){
            var set = Resolve<SetRepo>().GetById((int)setId);
            return AnswerSet(set, set.QuestionsInSet.GetPreviousTo((int)questionId).Question);
        }

        var activeSearchSpec = Resolve<QuestionSearchSpecSession>().ByKey(pager);
        activeSearchSpec.PreviousPage(1);
        return GetViewBySearchSpec(activeSearchSpec);
    }

    private ActionResult GetViewBySearchSpec(QuestionSearchSpec searchSpec)
    {
        using (MiniProfiler.Current.Step("GetViewBySearchSpec"))
        {
            var question = AnswerQuestionControllerSearch.Run(searchSpec);

            if (searchSpec.HistoryItem != null){
                if (searchSpec.HistoryItem.Question != null){
                    if (searchSpec.HistoryItem.Question.Id != question.Id){
                        question = Resolve<QuestionRepo>().GetById(searchSpec.HistoryItem.Question.Id);
                    }
                }

                searchSpec.HistoryItem = null;
            }

            _sessionUiData.VisitedQuestions.Add(new QuestionHistoryItem(question, searchSpec));

            var questionViewGuid = Guid.NewGuid();
            _saveQuestionView.Run(questionViewGuid, question, _sessionUser.UserId);

            return View(_viewLocation, new AnswerQuestionModel(questionViewGuid, question, searchSpec));
        }
    }

    [HttpPost]
    public JsonResult SendAnswer(int id, string answer, Guid questionViewGuid, int interactionNumber, int millisecondsSinceQuestionView)
    {
        var result = _answerQuestion.Run(id, answer, UserId, questionViewGuid, interactionNumber, millisecondsSinceQuestionView);
        var question = _questionRepo.GetById(id);
        var solution = new GetQuestionSolution().Run(question);

        return new JsonResult
        {
            Data = new
            {
                correct = result.IsCorrect,
                correctAnswer = result.CorrectAnswer,
                choices = solution.GetType() == typeof(QuestionSolutionMultipleChoice) ? 
                    ((QuestionSolutionMultipleChoice)solution).Choices
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

        var result = _answerQuestion.Run(id, answer, UserId, questionViewGuid, interactionNumber, millisecondsSinceQuestionView, learningSessionId, stepGuid);
        var question = _questionRepo.GetById(id);
        var solution = new GetQuestionSolution().Run(question);

        return new JsonResult
        {
            Data = new
            {
                correct = result.IsCorrect,
                correctAnswer = result.CorrectAnswer,
                choices = solution.GetType() == typeof(QuestionSolutionMultipleChoice) ?
                    ((QuestionSolutionMultipleChoice)solution).Choices
                    : null,
                newStepAdded = result.NewStepAdded
            }
        };
    }

    [HttpPost]
    public JsonResult GetSolution(int id, string answer, Guid questionViewGuid, int interactionNumber, int? roundId, int millisecondsSinceQuestionView = -1)
    {
        var question = _questionRepo.GetById(id);
        var solution = new GetQuestionSolution().Run(question);

        if (IsLoggedIn)
            if(roundId == null)
                R<AnswerLog>().LogAnswerView(question, this.UserId, questionViewGuid, interactionNumber, millisecondsSinceQuestionView);
            else
                R<AnswerLog>().LogAnswerView(question, this.UserId, questionViewGuid, interactionNumber, millisecondsSinceQuestionView, roundId);

        return new JsonResult
        {
            Data = new
            {
                correctAnswer = solution.CorrectAnswer(),
                correctAnswerDesc = MardownInit.Run().Transform(question.Description),
                correctAnswerReferences = question.References.Select( r => new
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
    public void LogTimeForQuestionView(Guid questionViewGuid, int millisecondsSinceQuestionView)
    {
        _saveQuestionView.LogOverallTime(questionViewGuid, millisecondsSinceQuestionView);
    }

    [HttpPost]
    public void CountLastAnswerAsCorrect(int id)
    {
        _answerQuestion.Run(id, _sessionUser.UserId, countLastAnswerAsCorrect: true);
    }

    [HttpPost]
    public void CountUnansweredAsCorrect(int id)
    {
        _answerQuestion.Run(id, _sessionUser.UserId, countUnansweredAsCorrect: true);
    }

    [HttpPost]
    public JsonResult SaveQuality(int id, int newValue)
    {
        QuestionInKnowledge.UpdateQuality(id, _sessionUser.User.Id, newValue);
        var totals = Sl.Resolve<GetQuestionTotal>().RunForQuality(id);
        return new JsonResult { Data = new { totalValuations = totals.Count, totalAverage = Math.Round(totals.Avg / 10d, 1) } };
    }

    [HttpPost]
    public JsonResult SaveRelevanceForAll(int id, int newValue)
    {
        QuestionInKnowledge.UpdateRelevanceAll(id, _sessionUser.User.Id, newValue);
        var totals = Sl.Resolve<GetQuestionTotal>().RunForRelevanceForAll(id);
        return new JsonResult { Data = new { totalValuations = totals.Count, totalAverage = Math.Round(totals.Avg / 10d, 1) } };
    }

    public ActionResult PartialAnswerHistory(int questionId)
    {
        var question = _questionRepo.GetById(questionId);
        
        var questionValuationForUser = NotNull.Run(Resolve<QuestionValuationRepo>().GetBy(question.Id, _sessionUser.UserId));
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

    public EmptyResult ClearHistory()
    {
        _sessionUiData.VisitedQuestions = new QuestionHistory();
        return new EmptyResult();
    }
}
