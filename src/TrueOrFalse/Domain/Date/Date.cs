using System;
using System.Collections.Generic;
using System.Linq;
using Seedworks.Lib.Persistence;
public class Date : DomainEntity
{
    public virtual string Details { get; set; }

    public virtual DateTime DateTime { get; set; }

    public virtual User User { get; set; }

    public virtual IList<Set> Sets { get; set; }

    public virtual DateVisibility Visibility { get; set; }

    public Date()
    {
        Sets = new List<Set>();
    }

    public virtual IList<Question> AllQuestions()
    {
        return Sets
            .SelectMany(s => s.QuestionsInSet.Select(qs => qs.Question))
            .ToList();
    }
}