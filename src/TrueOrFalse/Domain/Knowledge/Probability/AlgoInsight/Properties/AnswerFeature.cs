using System;
using System.Collections.Generic;
using Seedworks.Lib.Persistence;

public class AnswerFeature : DomainEntity
{
    public virtual string Id2 { get; set; }
    public virtual string Name { get; set; }

    public virtual IList<AnswerHistory> AnswerHistories { get; set; }

    public virtual Func<AnswerHistory, Question, User, bool> DoesApply { get; set; }
}