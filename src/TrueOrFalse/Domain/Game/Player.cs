using System.Collections.Generic;
using Seedworks.Lib.Persistence;

public class Player : DomainEntity
{
    public virtual User User { get; set; }
    public virtual Game Game { get; set; }

    public virtual IList<AnswerHistory> Answers { get; set; }

    public virtual int AnsweredCorrectly { get; set; }
    public virtual int AnsweredWrong { get; set; }

    public virtual int Position { get; set; }

    public virtual bool IsCreator { get; set; }

    public Player ()
    {
        Answers = new List<AnswerHistory>();        
    }
}
