using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace TrueOrFalse
{
    public class UpdateQuestionCountForAllCategories : IRegisterAsInstancePerLifetime
    {
        private readonly ISession _session;

        public UpdateQuestionCountForAllCategories(ISession session){
            _session = session;
        }

        public void Run()
        {
            var query =
                "UPDATE category SET QuestionCount = " +
                "(SELECT COUNT(*) FROM categoriestoquestions WHERE Category_id = category.Id)";

            _session.CreateSQLQuery(query).ExecuteUpdate();
        }
    }
}
