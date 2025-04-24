[Serializable]
public class LearningSessionStep
{
    public QuestionCacheItem Question;
    public AnswerState AnswerState = AnswerState.Unanswered;
    public string Answer { get; set; }
    public LearningSessionStep(QuestionCacheItem question) => Question = question;
}
