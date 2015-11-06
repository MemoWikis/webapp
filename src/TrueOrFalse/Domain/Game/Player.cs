using System.Linq;
using System.Collections.Generic;
using Seedworks.Lib.Persistence;

public class Player : DomainEntity
{
    public virtual User User { get; set; }
    public virtual Game Game { get; set; }

    public virtual IList<Answer> Answers { get; set; }

    public virtual int AnsweredCorrectly {
        get{
            return Answers.Count(x => x.AnswerredCorrectly == AnswerCorrectness.True);
        } 
    }

    public virtual int AnsweredWrong {
        get{
            return Answers.Count(x => x.AnswerredCorrectly == AnswerCorrectness.False);
        }    
    }

    public virtual int Position { get; set; }

    public virtual bool IsCreator { get; set; }

    public Player ()
    {
        Answers = new List<Answer>();        
    }
}
