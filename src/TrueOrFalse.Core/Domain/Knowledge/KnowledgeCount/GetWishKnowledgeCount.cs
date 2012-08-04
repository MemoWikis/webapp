using System.Web;
using NHibernate;
using TrueOrFalse.Core.Web.Context;

namespace TrueOrFalse.Core
{
    public class GetWishKnowledgeCount : IRegisterAsInstancePerLifetime 
    {
        private readonly ISession _session;

        public GetWishKnowledgeCount(ISession session){
            _session = session;
        }

        public int Run(int userId, bool forceReload = false)
        {
            if(!forceReload)
                if (HttpContext.Current.Items[ContextItemKeys.WishKnowledgeCount] != null)
                    return (int)HttpContext.Current.Items[ContextItemKeys.WishKnowledgeCount];

            var result = _session.CreateSQLQuery("SELECT count(*) FROM QuestionValuation " +
                                                 "WHERE UserId = " + userId +
                                                 "AND RelevancePersonal > -1 ")
                                 .UniqueResult<int>();

            HttpContext.Current.Items[ContextItemKeys.WishKnowledgeCount] = result;
            return result;
        }
    }
}
