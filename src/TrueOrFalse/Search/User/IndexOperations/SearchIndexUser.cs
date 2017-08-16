using System;
using SolrNet;

namespace TrueOrFalse.Search
{
    public class SearchIndexUser : IRegisterAsInstancePerLifetime
    {
        private readonly ISolrOperations<UserSolrMap> _solrOperations;

        public SearchIndexUser(ISolrOperations<UserSolrMap> solrOperations){
            _solrOperations = solrOperations;
        }

        public void Update(User user, bool runSolrUpdateAsync = false)
        {
            void action()
            {
                _solrOperations.Add(ToUserSolrMap.Run(user));
                _solrOperations.Commit();
            }

            if (runSolrUpdateAsync)
                AsyncExe.Run(action);
            else
                action();
        }

        public void Delete(User user)
        {
            _solrOperations.Delete(ToUserSolrMap.Run(user));
            _solrOperations.Commit();
        }
    }
}