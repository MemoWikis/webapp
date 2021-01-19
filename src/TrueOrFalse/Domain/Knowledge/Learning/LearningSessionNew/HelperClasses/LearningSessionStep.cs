using System;

[Serializable]
public class LearningSessionStep
{
    public readonly Question Question;
    public AnswerState AnswerState = AnswerState.Unanswered;
    public string Answer { get; set; }
    public LearningSessionStep(Question question) => Question = question;
}
