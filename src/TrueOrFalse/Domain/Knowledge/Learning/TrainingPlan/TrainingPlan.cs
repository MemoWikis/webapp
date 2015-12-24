using System;
using System.Collections.Generic;
using FluentNHibernate.Data;

public class TrainingPlan : Entity
{
    public IList<TrainingDate> LearningDates;

    public IList<Question> Questions;
    public Date Date;

    public TimeSpan TimeRemaining;
    public TimeSpan TimeSpent;
}