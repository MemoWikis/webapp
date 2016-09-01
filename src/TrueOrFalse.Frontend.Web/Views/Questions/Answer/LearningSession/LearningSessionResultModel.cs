using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class LearningSessionResultModel : BaseModel
{
    public LearningSession LearningSession;
    public int NumberSteps;
    public int NumberUniqueQuestions;
    public int NumberQuestions;
    public int NumberCorrectAnswers; //answered correctly at first try
    public int NumberCorrectAfterRepetitionAnswers; 
    public int NumberWrongAnswers;
    public int NumberNotAnswered;
    public int NumberCorrectPercentage;
    public int NumberCorrectAfterRepetitionPercentage;
    public int NumberWrongAnswersPercentage;
    public int NumberNotAnsweredPercentage;

    public IEnumerable<IGrouping<int, LearningSessionStep>> AnsweredStepsGrouped;

    public Date DateToLearn;
    public bool DateIsInPast;
    public TimeSpanLabel DateRemainingTimeLabel;
    public TrainingPlan TrainingPlan;
    public int TrainingDateCount;
    public string RemainingTrainingTime;

    public int KnowledgeNotLearned;
    public int KnowledgeNeedsLearning;
    public int KnowledgeNeedsConsolidation;
    public int KnowledgeSolid;

    public LearningSessionResultModel(LearningSession learningSession)
    {
        LearningSession = learningSession;
        NumberSteps = LearningSession.Steps.Count();
        NumberQuestions = LearningSession.Questions().Count;

        if (learningSession.IsDateSession)
        {
            DateToLearn = learningSession.DateToLearn;
            var remaining = DateToLearn.Remaining();
            DateIsInPast = remaining.TotalSeconds < 0;
            DateRemainingTimeLabel = new TimeSpanLabel(remaining, DateIsInPast);
            TrainingPlan = DateToLearn.TrainingPlan ?? new TrainingPlan();
            TrainingDateCount = TrainingPlan.OpenDates.Count;
            RemainingTrainingTime = new TimeSpanLabel(TrainingPlan.TimeRemaining).Full;
            var summary = KnowledgeSummaryLoader.Run(UserId, DateToLearn.AllQuestions().GetIds(), onlyValuated: false);
            KnowledgeNotLearned = summary.NotLearned;
            KnowledgeNeedsLearning = summary.NeedsLearning;
            KnowledgeNeedsConsolidation = summary.NeedsConsolidation;
            KnowledgeSolid = summary.Solid;
        }

        if (NumberSteps > 0)
        {
            AnsweredStepsGrouped = LearningSession.Steps.GroupBy(d => d.QuestionId);
            NumberUniqueQuestions = AnsweredStepsGrouped.Count();
            //var answeredSteps = LearningSession.Steps.Where(s => s.AnswerState == StepAnswerState.Answered).ToList();
            //NumberCorrectAnswers = answeredSteps.Count(s => s.Answer.AnswerredCorrectly != AnswerCorrectness.False);
            //NumberWrongAnswers = answeredSteps.Count(s => s.Answer.AnswerredCorrectly == AnswerCorrectness.False);
            //NumberNotAnswered = LearningSession.Steps.Count(s => s.AnswerState == StepAnswerState.Skipped || s.AnswerState == StepAnswerState.NotViewedOrAborted);

            NumberCorrectAnswers = AnsweredStepsGrouped.Count(g => g.First().AnswerState == StepAnswerState.Answered && g.First().Answer.AnsweredCorrectly());
            NumberCorrectAfterRepetitionAnswers = AnsweredStepsGrouped.Count(g => g.Last().AnswerState == StepAnswerState.Answered &&  g.Count() > 1 && g.Last().Answer.AnsweredCorrectly());
            NumberWrongAnswers = AnsweredStepsGrouped.Count(g => 
                    (g.Last().AnswerState == StepAnswerState.Answered && g.Last().Answer.AnswerredCorrectly == AnswerCorrectness.False) || 
                    (g.Last().AnswerState != StepAnswerState.Answered && g.Count() > 1));
            NumberNotAnswered = AnsweredStepsGrouped.Count(g => g.First().AnswerState == StepAnswerState.Skipped || g.First().AnswerState == StepAnswerState.NotViewedOrAborted);

            NumberCorrectPercentage = (int)Math.Round(NumberCorrectAnswers / (float)NumberUniqueQuestions * 100);
            NumberCorrectAfterRepetitionPercentage = (int)Math.Round(NumberCorrectAfterRepetitionAnswers / (float)NumberUniqueQuestions * 100);
            NumberWrongAnswersPercentage = (int)Math.Round(NumberWrongAnswers / (float)NumberUniqueQuestions * 100);
            NumberNotAnsweredPercentage = (int)Math.Round(NumberNotAnswered / (float)NumberUniqueQuestions * 100);

        }
    }
}
