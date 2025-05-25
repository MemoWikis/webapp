public class LearningSessionResultModel
{
    public LearningSession LearningSession;
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

    public LearningSessionResultModel(
        LearningSession learningSession,
        bool isInTestMode = false)
    {
        ShowSummaryText = !isInTestMode;
        LearningSession = learningSession;
        NumberSteps = LearningSession.Steps.Count();
        var numberQuestions = LearningSession.Steps.Count(s => s.AnswerState == AnswerState.Unanswered || s.AnswerState == AnswerState.Skipped);
        PercentageAverageRightAnswers = (int)Math.Round(LearningSession.Steps.Sum(s => s.Question.CorrectnessProbability) / (float)numberQuestions);

        if (NumberSteps <= 0)
        {
            return;
        }

        AnsweredStepsGrouped = LearningSession.Steps.GroupBy(d => d.Question.Id);

        var answeredStepsGrouped = 
            AnsweredStepsGrouped as IGrouping<int, LearningSessionStep>[] ?? 
            AnsweredStepsGrouped.ToArray();
            
        NumberUniqueQuestions = answeredStepsGrouped.Count();

        NumberCorrectAnswers = answeredStepsGrouped
            .Count(group => 
                group.First().AnswerState != AnswerState.Unanswered && 
                group.First().AnswerState == AnswerState.Correct
            );

        NumberCorrectAfterRepetitionAnswers = answeredStepsGrouped
            .Count(group => 
                group.Last().AnswerState != AnswerState.Unanswered && 
                group.Count() > 1 && group.Last().AnswerState == AnswerState.Correct);

        NumberNotAnswered = answeredStepsGrouped
            .Count(group => 
                group.All(a => 
                    a.AnswerState == AnswerState.Unanswered || 
                    a.AnswerState == AnswerState.Skipped)
            );

        NumberWrongAnswers = 
            NumberUniqueQuestions - 
            NumberNotAnswered - 
            NumberCorrectAnswers - 
            NumberCorrectAfterRepetitionAnswers;
            
        if (NumberWrongAnswers < 0)
            Log.Error("Answered questions (wrong+skipped+...) don't add up at LearningSessionResult for LearningSession");

        PercentageShares.FromAbsoluteShares(
            new List<ValueWithResultAction>
            {
                new() { AbsoluteValue = NumberCorrectAnswers, ActionForPercentage = p => NumberCorrectPercentage = p },
                new() { AbsoluteValue = NumberCorrectAfterRepetitionAnswers, ActionForPercentage = p => NumberCorrectAfterRepetitionPercentage = p },
                new() { AbsoluteValue = NumberWrongAnswers, ActionForPercentage = p => NumberWrongAnswersPercentage = p },
                new() { AbsoluteValue = NumberNotAnswered, ActionForPercentage = p => NumberNotAnsweredPercentage = p },
            });
    }
}
