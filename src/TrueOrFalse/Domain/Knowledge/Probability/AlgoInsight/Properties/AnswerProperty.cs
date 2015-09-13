using System;
using Seedworks.Lib.Persistence;

public class AnswerProperty : DomainEntity
{
    public string Key;
    public string Name;

    public Func<AnswerHistory, Question, User, bool> Filter;
}