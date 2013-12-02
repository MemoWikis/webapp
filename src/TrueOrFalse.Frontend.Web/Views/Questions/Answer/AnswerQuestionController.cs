using System;
using System.Linq;
using System.Web.Mvc;
using FluentNHibernate.Utils;
using TrueOrFalse;
using TrueOrFalse.Web;
using TrueOrFalse.Web.Context;

[HandleError]
public class AnswerQuestionController : BaseController
{
    private readonly QuestionRepository _questionRepository;

    private readonly AnswerQuestion _answerQuestion;
    private readonly SaveQuestionView _saveQuestionView;
    private const string _viewLocation = "~/Views/Questions/Answer/AnswerQuestion.aspx";

    public AnswerQuestionController(QuestionRepository questionRepository,
                                    AnswerQuestion answerQuestion,
                                    SaveQuestionView saveQuestionView)
    {
        _questionRepository = questionRepository;
        _answerQuestion = answerQuestion;
        _saveQuestionView = saveQuestionView;
    }


    [SetMenu(MenuEntry.QuestionDetail)]
    public ActionResult Answer(string text, int? id, int? elementOnPage, string pager, int? setId, int? questionId)
    {
        if (setId != null && questionId != null)
            return AnswerSet((int)setId, (int)questionId);

        return AnswerQuestion(text, id, elementOnPage, pager);
    }

    public ActionResult AnswerSet(int setId, int questionId)
    {
        var set = Resolve<SetRepository>().GetById(setId);
        var question = _questionRepository.GetById(questionId);
        return AnswerSet(set, question);
    }

    public ActionResult AnswerSet(Set set, Question question )
    {
        _sessionUiData.VisitedQuestions.Add(new QuestionHistoryItem(set, question));
        return View(_viewLocation, new AnswerQuestionModel(set, question));
    }

    public ActionResult AnswerQuestion(string text, int? id, int? elementOnPage, string pager)
    {
        var activeSearchSpec = Resolve<QuestionSearchSpecSession>().ByKey(pager);

        if (text == null && id == null && elementOnPage == null)
            return GetViewBySearchSpec(activeSearchSpec);

        var question = _questionRepository.GetById((int)id);

        _sessionUiData.VisitedQuestions.Add(new QuestionHistoryItem(question, activeSearchSpec));
        _saveQuestionView.Run(question, _sessionUser.User.Id);

        return View(_viewLocation, new AnswerQuestionModel(question, activeSearchSpec, (int)elementOnPage));
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
        var question = Resolve<AnswerQuestionControllerSearch>().Run(searchSpec);
        _sessionUiData.VisitedQuestions.Add(new QuestionHistoryItem(question, searchSpec));
        _saveQuestionView.Run(question, _sessionUser.User.Id);

        return View(_viewLocation, new AnswerQuestionModel(question, searchSpec));
    }

    [HttpPost]
    public JsonResult SendAnswer(int id, string answer)
    {
        var result = _answerQuestion.Run(id, answer, _sessionUser.User.Id);

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
        var solution = new GetQuestionSolution().Run(question.SolutionType, question.Solution);
        return new JsonResult
                   {
                       Data = new
                                  {
                                      correctAnswer = solution.CorrectAnswer(),
                                      correctAnswerDesc = MardownInit.Run().Transform(question.Description)
                                  }
                   };
    }

    [HttpPost]
    public JsonResult SaveQuality(int id, int newValue)
    {
        Sl.Resolve<UpdateQuestionTotals>().UpdateQuality(id, _sessionUser.User.Id, newValue);
        var totals = Sl.Resolve<GetQuestionTotal>().RunForQuality(id);
        return new JsonResult { Data = new { totalValuations = totals.Count, totalAverage = Math.Round(totals.Avg / 10d, 1) } };
    }

    [HttpPost]
    public JsonResult SaveRelevancePersonal(int id, int newValue)
    {
        var oldKnowledgeCount = Sl.Resolve<GetWishQuestionCountCached>().Run(_sessionUser.User.Id, forceReload: true);
        
        Sl.Resolve<UpdateQuestionTotals>().UpdateRelevancePersonal(id, _sessionUser.User.Id, newValue);
        var totals = Sl.Resolve<GetQuestionTotal>().RunForRelevancePersonal(id);

        var newKnowledgeCount = Sl.Resolve<GetWishQuestionCountCached>().Run(_sessionUser.User.Id, forceReload: true);

        return new JsonResult { Data = new
            {
                totalValuations = totals.Count, 
                totalAverage = Math.Round(totals.Avg / 10d, 1),
                totalWishKnowledgeCount = Sl.Resolve<GetWishQuestionCountCached>().Run(_sessionUser.User.Id, forceReload:true),
                totalWishKnowledgeCountChange = oldKnowledgeCount != newKnowledgeCount
            }};
    }

    [HttpPost]
    public JsonResult SaveRelevanceForAll(int id, int newValue)
    {
        Sl.Resolve<UpdateQuestionTotals>().UpdateRelevanceAll(id, _sessionUser.User.Id, newValue);
        var totals = Sl.Resolve<GetQuestionTotal>().RunForRelevanceForAll(id);
        return new JsonResult { Data = new { totalValuations = totals.Count, totalAverage = Math.Round(totals.Avg / 10d, 1) } };
    }
}
