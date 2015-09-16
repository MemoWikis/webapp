using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using StackExchange.Profiling;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Updates;
using TrueOrFalse.Web;

public class AnswerQuestionController : BaseController
{
    private readonly QuestionRepository _questionRepository;

    private readonly AnswerQuestion _answerQuestion;
    private readonly AnswerHistoryLog _answerHistoryLog;
    private readonly UpdateQuestionAnswerCount _updateQuestionAnswerCount;
    private readonly SaveQuestionView _saveQuestionView;
    private const string _viewLocation = "~/Views/Questions/Answer/AnswerQuestion.aspx";

    public AnswerQuestionController(QuestionRepository questionRepository,
                                    AnswerQuestion answerQuestion,
                                    AnswerHistoryLog answerHistoryLog,
                                    UpdateQuestionAnswerCount updateQuestionAnswerCount,
                                    SaveQuestionView saveQuestionView)
    {
        _questionRepository = questionRepository;
        _answerQuestion = answerQuestion;
        _answerHistoryLog = answerHistoryLog;
        _updateQuestionAnswerCount = updateQuestionAnswerCount;
        _saveQuestionView = saveQuestionView;
    }


    [SetMenu(MenuEntry.QuestionDetail)]
    public ActionResult Answer(string text, int? id, int? elementOnPage, string pager, int? setId, int? questionId, string category)
    {
        if (setId != null && questionId != null)
            return AnswerSet((int)setId, (int)questionId);

        return AnswerQuestion(text, id, elementOnPage, pager, category);
    }

    public ActionResult Learn(int learningSessionId, string learningSessionName, int stepNo, int skipStepId = -1)
    {
        ////_sessionUiData.VisitedQuestions.Add(new QuestionHistoryItem(question, activeSearchSpec));
        //_saveQuestionView.Run(question, _sessionUser.User);

        var learningSession = Sl.Resolve<LearningSessionRepo>().GetById(learningSessionId);

        if(learningSession.User != _sessionUser.User)
            throw new Exception("not logged in or not possessing user");

        if (skipStepId != -1 && learningSession.Steps.Any(s => s.Id == skipStepId))
        {
            LearningSessionStep.Skip(skipStepId);
            return RedirectToAction("Learn", Links.AnswerQuestionController, 
                new {learningSessionId, learningSessionName = learningSessionName, stepNo});
        }

        var currentLearningStepIdx = learningSession.CurrentLearningStepIdx();

        if (currentLearningStepIdx == -1) //None of the steps is uncompleted
            return RedirectToAction("LearningSessionResult", Links.LearningSessionResultController,
                new { learningSessionId, learningSessionName = learningSessionName });

        if (currentLearningStepIdx != stepNo - 1)//Correct url if stepNo is adjusted
        {
            return Redirect(Links.LearningSession(learningSession, currentLearningStepIdx));
        }

        return View(_viewLocation, new AnswerQuestionModel(Sl.Resolve<LearningSessionRepo>().GetById(learningSessionId), currentLearningStepIdx + 1));
    }

    public ActionResult AnswerSet(int setId, int questionId)
    {
        var set = Resolve<SetRepo>().GetById(setId);
        var question = _questionRepository.GetById(questionId);
        return AnswerSet(set, question);
    }

    public ActionResult AnswerSet(Set set, Question question )
    {
        _sessionUiData
            .VisitedQuestions
            .Add(new QuestionHistoryItem(set, question));

        return View(_viewLocation, new AnswerQuestionModel(set, question));
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
            }
        }

        if (text == null && id == null && elementOnPage == null)
            return GetViewBySearchSpec(activeSearchSpec);

        var question = _questionRepository.GetById((int)id);

        activeSearchSpec.PageSize = 1;
        if ((int)elementOnPage != -1)
            activeSearchSpec.CurrentPage = (int)elementOnPage;

        _sessionUiData.VisitedQuestions.Add(new QuestionHistoryItem(question, activeSearchSpec));
        _saveQuestionView.Run(question, _sessionUser.User);

        return View(_viewLocation, new AnswerQuestionModel(question, activeSearchSpec));
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
            var question = Resolve<AnswerQuestionControllerSearch>().Run(searchSpec);

            if (searchSpec.HistoryItem != null){
                if (searchSpec.HistoryItem.Question != null){
                    if (searchSpec.HistoryItem.Question.Id != question.Id){
                        question = Resolve<QuestionRepository>().GetById(searchSpec.HistoryItem.Question.Id);
                    }
                }

                searchSpec.HistoryItem = null;
            }

            _sessionUiData.VisitedQuestions.Add(new QuestionHistoryItem(question, searchSpec));
            _saveQuestionView.Run(question, _sessionUser.UserId);

            return View(_viewLocation, new AnswerQuestionModel(question, searchSpec));
        }
    }

    [HttpPost]
    public JsonResult SendAnswer(int id, string answer)
    {
        var result = _answerQuestion.Run(id, answer, UserId);
        var question = _questionRepository.GetById(id);
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
    public JsonResult SendAnswerLearningSession(int id, int stepId, string answer)
    {
        var result = _answerQuestion.Run(id, answer, UserId, stepId);
        var question = _questionRepository.GetById(id);
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
    public JsonResult GetSolution(int id, string answer)
    {
        var question = _questionRepository.GetById(id);
        var solution = new GetQuestionSolution().Run(question);
        return new JsonResult
        {
            Data = new
            {
                correctAnswer = solution.CorrectAnswer(),
                correctAnswerDesc = MardownInit.Run().Transform(question.Description),
                correctAnswerReferences = question.References.Select( r => new
                {
                    referenceId = r.Id,
                    categoryId = r.Category == null ? -1 : r.Category.Id,
                    referenceType = r.ReferenceType.GetName(),
                    additionalInfo = r.AdditionalInfo ?? "",
                    referenceText = r.ReferenceText ?? ""
                })
            }
        };
    }

    [HttpPost]
    public void CountLastAnswerAsCorrect(int id)
    {
        _answerHistoryLog.CountLastAnswerAsCorrect(_questionRepository.GetById(id), _sessionUser.UserId);
        _updateQuestionAnswerCount.ChangeOneWrongAnswerToCorrect(id);
    }

    [HttpPost]
    public void CountUnansweredAsCorrect(int id)
    {
        _answerHistoryLog.CountUnansweredAsCorrect(_questionRepository.GetById(id), _sessionUser.UserId);
        _updateQuestionAnswerCount.Run(id, true);
    }

    [HttpPost]
    public JsonResult SaveQuality(int id, int newValue)
    {
        Sl.Resolve<UpdateQuestionTotals>().UpdateQuality(id, _sessionUser.User.Id, newValue);
        var totals = Sl.Resolve<GetQuestionTotal>().RunForQuality(id);
        return new JsonResult { Data = new { totalValuations = totals.Count, totalAverage = Math.Round(totals.Avg / 10d, 1) } };
    }

    [HttpPost]
    public JsonResult SaveRelevanceForAll(int id, int newValue)
    {
        Sl.Resolve<UpdateQuestionTotals>().UpdateRelevanceAll(id, _sessionUser.User.Id, newValue);
        var totals = Sl.Resolve<GetQuestionTotal>().RunForRelevanceForAll(id);
        return new JsonResult { Data = new { totalValuations = totals.Count, totalAverage = Math.Round(totals.Avg / 10d, 1) } };
    }

    public ActionResult PartialAnswerHistory(int questionId)
    {
        var question = _questionRepository.GetById(questionId);
        
        var questionValuationForUser = NotNull.Run(Resolve<QuestionValuationRepo>().GetBy(question.Id, _sessionUser.UserId));
        var valuationForUser = Resolve<TotalsPersUserLoader>().Run(_sessionUser.UserId, question.Id);

        return View("HistoryAndProbability",
            new HistoryAndProbabilityModel
            {
                LoadJs = true,
                AnswerHistory = new AnswerHistoryModel(question, valuationForUser),
                CorrectnessProbability = new CorrectnessProbabilityModel(question, questionValuationForUser)
            }
        );
    }

    public EmptyResult ClearHistory()
    {
        _sessionUiData.VisitedQuestions = new QuestionHistory();
        return new EmptyResult();
    }
}
