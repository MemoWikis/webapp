﻿using System;
using System.Linq;
using System.Web.Mvc;
using TrueOrFalse;
using TrueOrFalse.Web;
using TrueOrFalse.Web.Context;

[HandleError]
public class AnswerQuestionController : BaseController
{
    private readonly QuestionRepository _questionRepository;
    private readonly QuestionValuationRepository _questionValuation;
    private readonly TotalsPersUserLoader _totalsPerUserLoader;
    private readonly AnswerQuestion _answerQuestion;
    private readonly SaveQuestionView _saveQuestionView;
    private const string _viewLocation = "~/Views/Questions/Answer/AnswerQuestion.aspx";

    public AnswerQuestionController(QuestionRepository questionRepository,
                                    QuestionValuationRepository questionValuation,
                                    TotalsPersUserLoader totalsPerUserLoader,
                                    AnswerQuestion answerQuestion,
                                    SaveQuestionView saveQuestionView)
    {
        _questionRepository = questionRepository;
        _questionValuation = questionValuation;
        _totalsPerUserLoader = totalsPerUserLoader;
        _answerQuestion = answerQuestion;
        _saveQuestionView = saveQuestionView;
    }

    [SetMenu(MenuEntry.QuestionDetail)]
    public ActionResult Answer(string text, int id, int elementOnPage)
    {
        var question = _questionRepository.GetById(id);
        var questionValuation = _questionValuation.GetBy(id, _sessionUser.User.Id);

        _sessionUiData.VisitedQuestions.Add(new QuestionHistoryItem(question));

        _saveQuestionView.Run(id, _sessionUser.User.Id);

        return View(_viewLocation, 
            new AnswerQuestionModel(
                question,
                _totalsPerUserLoader.Run(_sessionUser.User.Id, question.Id),
                NotNull.Run(questionValuation), 
                _sessionUiData.SearchSpecQuestion, 
                elementOnPage)
        );
    }

    public ActionResult Next()
    {
        _sessionUiData.SearchSpecQuestion.NextPage(1);
        return GetViewByCurrentSearchSpec();
    }

    public ActionResult Previous()
    {
        _sessionUiData.SearchSpecQuestion.PreviousPage(1);
        return GetViewByCurrentSearchSpec();
    }

    private ActionResult GetViewByCurrentSearchSpec()
    {
        var question = Resolve<AnswerQuestionControllerSearch>().Run();
        var questionValuation = _questionValuation.GetBy(question.Id, _sessionUser.User.Id);
        _sessionUiData.VisitedQuestions.Add(new QuestionHistoryItem(question));

        _saveQuestionView.Run(question.Id, _sessionUser.User.Id);

        return View(_viewLocation, 
            new AnswerQuestionModel(
                question, 
                _totalsPerUserLoader.Run(_sessionUser.User.Id, question.Id),
                NotNull.Run(questionValuation), 
                _sessionUiData.SearchSpecQuestion)
        );
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
