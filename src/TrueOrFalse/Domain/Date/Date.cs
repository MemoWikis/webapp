using System;
using System.Collections.Generic;
using System.Linq;
using Seedworks.Lib;
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

    public virtual int CountQuestions()
    {
        return AllQuestions().Count;
    }

    public virtual string GetTitle(bool shorten = false)
    {
        if (Details == null)
            Details = "";

        if (Details.Length > 6)
            Details.WordWrap(50);

        if (shorten && Details.Length > 40)
            return Details.Truncate(40) + "...";

        if (Details.Length > 0)
            return Details;

        return "(" + CountQuestions() + " Fragen bis zum " + DateTime.ToString("dd.MM.yyy") + ")";
    }
}