using Seedworks.Lib.Persistence;

public class AnswerTest : DomainEntity
{
	public virtual Answer Answer { get; set; }
	public virtual int AlgoId { get; set; }
	public virtual int Probability { get; set; }
	public virtual bool IsCorrect { get; set; }
}
