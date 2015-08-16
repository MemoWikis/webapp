using Seedworks.Lib.Persistence;

public class LearningSessionStep : DomainEntity, IRegisterAsInstancePerLifetime
{
    public virtual Question Question { get; set; }
    public virtual AnswerHistory AnswerHistory { get; set; }
    public virtual StepAnswerState AnswerState { get; set; }
}