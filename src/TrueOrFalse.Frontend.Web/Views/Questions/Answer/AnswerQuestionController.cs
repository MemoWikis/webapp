using System;
using System.Linq;
using System.Web.Hosting;
using System.Web.Mvc;
using FluentNHibernate.Utils;
using StackExchange.Profiling;
using TrueOrFalse;
using TrueOrFalse.Web;
using TrueOrFalse.Web.Context;

public class AnswerQuestionController : BaseController
{
    private readonly QuestionRepository _questionRepository;

    private readonly AnswerQuestion _answerQuestion;
    private readonly AnswerHistoryLog _answerHistoryLog;
    private readonly SaveQuestionView _saveQuestionView;
    private const string _viewLocation = "~/Views/Questions/Answer/AnswerQuestion.aspx";

    public AnswerQuestionController(QuestionRepository questionRepository,
                                    AnswerQuestion answerQuestion,
                                    AnswerHistoryLog answerHistoryLog,
                                    SaveQuestionView saveQuestionView)
    {
        _questionRepository = questionRepository;
        _answerQuestion = answerQuestion;
        _answerHistoryLog = answerHistoryLog;
        _saveQuestionView = saveQuestionView;
    }


    [SetMenu(MenuEntry.QuestionDetail)]
    public ActionResult Answer(string text, int? id, int? elementOnPage, string pager, int? setId, int? questionId, string category)
    {
        if (setId != null && questionId != null)
            return AnswerSet((int)setId, (int)questionId);

        return AnswerQuestion(text, id, elementOnPage, pager, category);
    }

    public ActionResult AnswerSet(int setId, int questionId)
    {
        var set = Resolve<SetRepository>().GetById(setId);
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

        if (String.IsNullOrEmpty(category))
        {
            var categoryDb = R<CategoryRepository>().GetById(Convert.ToInt32(category));
            if (categoryDb != null)
            {
                activeSearchSpec.Filter.Categories.Add(categoryDb.Id);
                activeSearchSpec.OrderBy.OrderByPersonalRelevance.Desc();
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
            var set = Resolve<SetRepository>().GetById((int)setId);
            return AnswerSet(set, set.QuestionsInSet.GetNextTo((int) questionId).Question);
        }

        var activeSearchSpec = Resolve<QuestionSearchSpecSession>().ByKey(pager);
        activeSearchSpec.NextPage(1);
        return GetViewBySearchSpec(activeSearchSpec);
    }

    public ActionResult Previous(string pager, int? setId, int? questionId)
    {
        if (setId != null && questionId != null){
            var set = Resolve<SetRepository>().GetById((int)setId);
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

        return new JsonResult
                   {
                       Data = new
                                  {
                                      correct = result.IsCorrect,
                                      correctAnswer = result.CorrectAnswer
                                  }
                   };
    }

    [HttpPost]
    public JsonResult GetAnswer(int id, string answer)
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
                }),
            }
        };
    }

    [HttpPost]
    public void CountLastAnswerAsCorrect(int id)
    {
        _answerHistoryLog.CountLastAnswerAsCorrect(_questionRepository.GetById(id), _sessionUser.UserId);
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
        
        var questionValuationForUser = NotNull.Run(Resolve<QuestionValuationRepository>().GetBy(question.Id, _sessionUser.UserId));
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
