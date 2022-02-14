using System;

[Serializable]
public class LearningSessionStep
{
    public Question Question;
    public AnswerState AnswerState = AnswerState.Unanswered;
    public string Answer { get; set; }
    public LearningSessionStep(Question question) => Question = question;
}
