using System.Collections.Generic;
using NHibernate.Mapping;

public class LearningSessionStepNew
{
    public readonly Question Question;
    public AnswerStateNew AnswerState = AnswerStateNew.Unanswered;
    public LearningSessionStepNew(Question question) => Question = question;
}
