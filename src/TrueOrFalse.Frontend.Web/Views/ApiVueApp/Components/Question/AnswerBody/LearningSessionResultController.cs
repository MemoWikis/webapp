﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

public class VueLearningSessionResultController(LearningSessionCache _learningSessionCache,
    ImageMetaDataReadingRepo _imageMetaDataReadingRepo,
    IHttpContextAccessor _httpContextAccessor,
    QuestionReadingRepo _questionReadingRepo) : Controller
{
    public record struct LearningSessionResult(
        int UniqueQuestionCount, 
        CorrectWrongOrNotAnswered Correct,
        CorrectWrongOrNotAnswered CorrectAfterRepetition,
        CorrectWrongOrNotAnswered Wrong,
        CorrectWrongOrNotAnswered NotAnswered,
        string TopicName,
        int TopicId,
        bool InWuwi,
        TinyQuestion[] Questions);
    public record struct CorrectWrongOrNotAnswered(int Percentage, int Count);
    public record struct TinyQuestion(
        string CorrectAnswerHtml,
        int Id,
        string ImgUrl,
        string Title,
        Step[] Steps);

    public record struct Step(AnswerState AnswerState, string AnswerAsHtml); 

    [HttpGet]
    public LearningSessionResult Get() => GetLearningSessionResult();

    private LearningSessionResult GetLearningSessionResult()
    {
        var learningSession = _learningSessionCache.GetLearningSession();
        var model = new LearningSessionResultModel(learningSession);
        var tinyQuestions = model.AnsweredStepsGrouped
            .Where(g => g.First().Question.Id != 0)
            .Select(g =>
            {
                var question = g.First().Question;
                return new TinyQuestion(
                    CorrectAnswerHtml: GetQuestionSolution.Run(question).GetCorrectAnswerAsHtml(),
                    Id: question.Id,
                    ImgUrl: GetQuestionImageFrontendData.Run(
                            question,
                            _imageMetaDataReadingRepo,
                            _httpContextAccessor,
                            _questionReadingRepo)
                        .GetImageUrl(128, true).Url,
                    Title: question.GetShortTitle(),
                    Steps: g.Select(s => new Step(
                    
                        AnswerState: s.AnswerState,
                        AnswerAsHtml: Question.AnswersAsHtml(s.Answer, question.SolutionType)
                    )).ToArray()
                );
            }).ToArray();

        return new LearningSessionResult(
            UniqueQuestionCount: model.NumberUniqueQuestions,
            Correct: new CorrectWrongOrNotAnswered(
            
                Percentage: model.NumberCorrectPercentage,
                Count: model.NumberCorrectAnswers
            ),
            CorrectAfterRepetition: new CorrectWrongOrNotAnswered(
            
                Percentage: model.NumberCorrectAfterRepetitionPercentage,
                Count: model.NumberCorrectAfterRepetitionAnswers
            ),
            Wrong: new CorrectWrongOrNotAnswered(
            
                Percentage: model.NumberWrongAnswersPercentage,
                Count: model.NumberWrongAnswers
            ),
            NotAnswered: new CorrectWrongOrNotAnswered(
            
                Percentage: model.NumberNotAnsweredPercentage,
                Count: model.NumberNotAnswered
            ),
            TopicName: learningSession.Config.Page.Name,
            TopicId: learningSession.Config.Page.Id,
            InWuwi: learningSession.Config.InWuwi,
            Questions:  tinyQuestions
        );
    }
}