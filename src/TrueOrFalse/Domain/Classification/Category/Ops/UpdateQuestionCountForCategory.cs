using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace TrueOrFalse
{
    public class UpdateQuestionCountForCategory : IRegisterAsInstancePerLifetime
    {
        private readonly ISession _session;

        public UpdateQuestionCountForCategory(ISession session){
            _session = session;
        }

        public void Run(int questionId)
        {
            var query =
                "UPDATE category SET QuestionCount = " +
                "(SELECT COUNT(*) FROM categoriestoquestions WHERE Category_id = category.Id " +
                " AND Question_id = " + questionId + " )";

            _session.CreateSQLQuery(query).ExecuteUpdate();
        }
    }
}
