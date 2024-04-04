using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

public class LearningSessionResultModel
{
    public LearningSession LearningSession;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public int NumberSteps;
    public int NumberUniqueQuestions;
    public int NumberCorrectAnswers; //answered correctly at first try
    public int NumberCorrectAfterRepetitionAnswers; 
    public int NumberWrongAnswers;
    public int NumberNotAnswered;
    public int NumberCorrectPercentage;
    public int NumberCorrectAfterRepetitionPercentage;
    public int NumberWrongAnswersPercentage;
    public int NumberNotAnsweredPercentage;

    public IEnumerable<IGrouping<int, LearningSessionStep>> AnsweredStepsGrouped;
    public bool ShowSummaryText;
    public int PercentageAverageRightAnswers;

    public LearningSessionResultModel(LearningSession learningSession,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment,
        bool isInTestMode = false)
    {
        ShowSummaryText = !isInTestMode;
        LearningSession = learningSession;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
        NumberSteps = LearningSession.Steps.Count();
        var numberQuestions = LearningSession.Steps.Count(s => s.AnswerState == AnswerState.Unanswered || s.AnswerState == AnswerState.Skipped);
        PercentageAverageRightAnswers = (int)Math.Round(LearningSession.Steps.Sum(s => s.Question.CorrectnessProbability) / (float)numberQuestions);

        if (NumberSteps > 0)
        {
            AnsweredStepsGrouped = LearningSession.Steps.GroupBy(d => d.Question.Id);

            NumberUniqueQuestions = AnsweredStepsGrouped.Count();

            NumberCorrectAnswers = AnsweredStepsGrouped.Count(
                g => g.First().AnswerState != AnswerState.Unanswered
                && g.First().AnswerState == AnswerState.Correct);

            NumberCorrectAfterRepetitionAnswers = AnsweredStepsGrouped.Count(
                g => g.Last().AnswerState != AnswerState.Unanswered 
                && g.Count() > 1 && g.Last().AnswerState == AnswerState.Correct);

            NumberNotAnswered = AnsweredStepsGrouped.Count(g => g.All(a => a.AnswerState == AnswerState.Unanswered || a.AnswerState == AnswerState.Skipped));

            NumberWrongAnswers = NumberUniqueQuestions - NumberNotAnswered - NumberCorrectAnswers - NumberCorrectAfterRepetitionAnswers;
            
            if (NumberWrongAnswers < 0)
                Logg.r.Error("Answered questions (wrong+skipped+...) don't add up at LearningSessionResult for LearningSession");

            PercentageShares.FromAbsoluteShares(
                new List<ValueWithResultAction>
                {
                    new() {AbsoluteValue = NumberCorrectAnswers, ActionForPercentage = p => NumberCorrectPercentage = p},
                    new() {AbsoluteValue = NumberCorrectAfterRepetitionAnswers, ActionForPercentage = p => NumberCorrectAfterRepetitionPercentage = p},
                    new() {AbsoluteValue = NumberWrongAnswers, ActionForPercentage = p => NumberWrongAnswersPercentage = p},
                    new() {AbsoluteValue = NumberNotAnswered, ActionForPercentage = p => NumberNotAnsweredPercentage = p},
                });
        }
    }
}
