using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class LearningSessionResultModel : BaseModel
{
    public LearningSessionNew LearningSession;
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

    public IEnumerable<IGrouping<int, LearningSessionStepNew>> AnsweredStepsGrouped;
    public List<LearningSessionStepNew> AnsweredStepsGroupedForDetailedAnswerAnalysis;

    public Date DateToLearn;
    public TrainingPlan TrainingPlan;
    public int TrainingDateCount;

    public int WishCountQuestions;
    public int WishCountSets;
    public bool ShowSummaryText;
    public int PercentageAverageRightAnswers;
    public int CounterSteps { get; set; }

    public LearningSessionResultModel(LearningSessionNew learningSession, bool isInTestMode = false)
    {
        ShowSummaryText = !isInTestMode;
        LearningSession = learningSession;
        NumberSteps = LearningSession.Steps.Count();
        var numberQuestions = LearningSession.Steps.Count(s => s.AnswerState == AnswerStateNew.Unanswered || s.AnswerState == AnswerStateNew.Skipped);
        PercentageAverageRightAnswers = (int)Math.Round(LearningSession.Steps.Sum(s => s.Question.CorrectnessProbability) / (float)numberQuestions);
        
        if (learningSession.Config.InWishknowledge)
        {
            WishCountQuestions = learningSession.User.WishCountQuestions;
            WishCountSets = learningSession.User.WishCountSets;
        }

        if (NumberSteps > 0)
        {
            AnsweredStepsGrouped = LearningSession.Steps.GroupBy(d => d.Question.Id);
           

            NumberUniqueQuestions = AnsweredStepsGrouped.Count();

            NumberCorrectAnswers = AnsweredStepsGrouped.Count(
                g => g.First().AnswerState != AnswerStateNew.Unanswered
                && g.First().AnswerState == AnswerStateNew.Correct);

            NumberCorrectAfterRepetitionAnswers = AnsweredStepsGrouped.Count(
                g => g.Last().AnswerState != AnswerStateNew.Unanswered 
                && g.Count() > 1 && g.Last().AnswerState == AnswerStateNew.Correct);

            NumberNotAnswered = AnsweredStepsGrouped.Count(g => g.All(a => a.AnswerState == AnswerStateNew.Unanswered || a.AnswerState == AnswerStateNew.Skipped));

            NumberWrongAnswers = NumberUniqueQuestions - NumberNotAnswered - NumberCorrectAnswers - NumberCorrectAfterRepetitionAnswers;
            
            if (NumberWrongAnswers < 0)
                Logg.r().Error("Answered questions (wrong+skipped+...) don't add up at LearningSessionResult for LearningSession");

            PercentageShares.FromAbsoluteShares(
                new List<ValueWithResultAction>
                {
                    new ValueWithResultAction{AbsoluteValue = NumberCorrectAnswers, ActionForPercentage = p => NumberCorrectPercentage = p},
                    new ValueWithResultAction{AbsoluteValue = NumberCorrectAfterRepetitionAnswers, ActionForPercentage = p => NumberCorrectAfterRepetitionPercentage = p},
                    new ValueWithResultAction{AbsoluteValue = NumberWrongAnswers, ActionForPercentage = p => NumberWrongAnswersPercentage = p},
                    new ValueWithResultAction{AbsoluteValue = NumberNotAnswered, ActionForPercentage = p => NumberNotAnsweredPercentage = p},
                });
        }
    }
}
