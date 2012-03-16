using System.Collections.Generic;
using NHibernate;
using TrueOrFalse.Core;

namespace TrueOrFalse.Tests
{
    public class UserSearch : IRegisterAsInstancePerLifetime
    {
        private readonly ISession _session;

        public UserSearch(ISession session)
        {
            _session = session;
        }

        public IList<Category> Run(string likeStatement)
        {
            return _session.CreateQuery("from User as u where u.Name like :nameLike")
                .SetString("nameLike", "%" + likeStatement + "%")
                .SetMaxResults(20)
                .List<Category>();
        }
    }
}