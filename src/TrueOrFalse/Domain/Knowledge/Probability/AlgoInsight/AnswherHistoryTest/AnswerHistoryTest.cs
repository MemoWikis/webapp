using Seedworks.Lib.Persistence;

public class AnswerHistoryTest : DomainEntity
{
	public virtual AnswerHistory AnswerHistory { get; set; }
	public virtual int AlgoId { get; set; }
	public virtual int Probability { get; set; }
	public virtual bool IsCorrect { get; set; }
}
