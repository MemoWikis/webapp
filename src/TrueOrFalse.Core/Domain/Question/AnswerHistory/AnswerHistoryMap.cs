using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace TrueOrFalse.Core
{
    public class AnswerHistoryMap : ClassMap<AnswerHistory>
    {
        public AnswerHistoryMap()
        {
            Id(x => x.Id);
            Map(x => x.UserId);
            Map(x => x.QuestionId);
            Map(x => x.AnswerText);
            Map(x => x.Milliseconds);
            Map(x => x.DateCreated);
        }
            
    }
}
