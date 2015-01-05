using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse
{
    public class QuestionInSetRepo : RepositoryDb<QuestionInSet>
    {
        public QuestionInSetRepo(ISession session) : base(session){}

        public override void Delete(int questionInSetId)
        {
            var questionInSet = GetById(questionInSetId);
            base.Delete(questionInSetId);

            Sl.R<UpdateSetDataForQuestion>().Run(questionInSet.Question);
        }
    }
}
