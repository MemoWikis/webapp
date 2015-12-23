using System;
using System.Collections.Generic;
using Seedworks.Lib.Persistence;

public class TrainingDate : DomainEntity
{
    public DateTime DateTime;
    public IList<Question> Questions;
}