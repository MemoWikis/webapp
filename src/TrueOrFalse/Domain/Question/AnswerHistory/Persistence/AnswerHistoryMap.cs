﻿using FluentNHibernate.Mapping;

public class AnswerHistoryMap : ClassMap<AnswerHistory>
{
    public AnswerHistoryMap()
    {
        Id(x => x.Id);
        Map(x => x.UserId);
        Map(x => x.QuestionId);
        Map(x => x.AnswerText);
        Map(x => x.AnswerredCorrectly);
        Map(x => x.Milliseconds);
        Map(x => x.DateCreated);
    }           
}