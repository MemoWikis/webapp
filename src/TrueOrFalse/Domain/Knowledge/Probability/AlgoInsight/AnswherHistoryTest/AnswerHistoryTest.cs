using Seedworks.Lib.Persistence;

public class AnswerHistoryTest : DomainEntity
{
	public AnswerHistory AnswerHistory { get; set; }
	public int AlgoId;
	public int Probability;
}
