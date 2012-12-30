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

        public void Run(IList<Category> categories )
        {
            foreach (var category in categories)
            {
                var query =
                    "UPDATE category SET QuestionCount = " +
                    "(SELECT COUNT(*) FROM categoriestoquestions WHERE Category_id = category.Id )" +
                    "WHERE Id = " + category.Id;

                _session.CreateSQLQuery(query).ExecuteUpdate();                
            }
        }
    }
}
