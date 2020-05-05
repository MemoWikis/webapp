using System.Collections.Generic;
using NHibernate.Mapping;

public class LearningSessionStepNew
{
    public readonly Question Question;
    public bool IsAnswered; 
    public bool IsAnswerCorrect;
    public bool IsSkip;

    public LearningSessionStepNew(Question question) => Question = question;
}
