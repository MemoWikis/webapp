using Seedworks.Lib.Persistence;

public class LearningSessionStep : DomainEntity, IRegisterAsInstancePerLifetime
{
    public virtual Question Question { get; set; }
    public virtual AnswerHistory AnswerHistory { get; set; }
    public virtual StepAnswerState AnswerState { get; set; }

    public static void Skip(int stepId)
    {
        var stepRepo = Sl.Resolve<LearningSessionStepRepo>();
        var stepToSkip = stepRepo.GetById(stepId);
        if (stepToSkip != null && stepToSkip.AnswerState != StepAnswerState.Answered)
        {
            stepToSkip.AnswerState = StepAnswerState.Skipped;
            stepRepo.Update(stepToSkip);
        }
    }
}