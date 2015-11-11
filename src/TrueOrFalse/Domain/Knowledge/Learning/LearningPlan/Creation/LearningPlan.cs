using System;
using System.Collections.Generic;
using FluentNHibernate.Data;

public class LearningPlan : Entity
{
    public IList<LearningDate> LearningDates;

    public IList<Question> Questions;
    public Date Date;

    public TimeSpan TimeRemaining;
    public TimeSpan TimeSpent;
}