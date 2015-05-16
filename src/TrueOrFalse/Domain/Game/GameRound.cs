using Seedworks.Lib.Persistence;

public class GameRound : DomainEntity
{
    public virtual GameRoundStatus Status { get; set; }
    public virtual Set Set { get; set; }
    public virtual Question Question { get; set; }

    public virtual Game Game { get; set; }
}

public enum GameRoundStatus
{
    Open = 1,
    Current = 2,
    Completed = 3
}