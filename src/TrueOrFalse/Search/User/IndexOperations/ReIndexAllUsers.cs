using SolrNet;

namespace TrueOrFalse.Search
{
    //todo: Remark to Delete
    public class ReIndexAllUsers : IRegisterAsInstancePerLifetime
    {
        private readonly UserRepo _userRepo;
        private readonly ISolrOperations<UserSolrMap> _solrOperations;

        public ReIndexAllUsers(
            UserRepo userRepo, 
            ISolrOperations<UserSolrMap> solrOperations)
        {
            _userRepo = userRepo;
            _solrOperations = solrOperations;
        }

        public void Run()
        {
            _solrOperations.Delete(new SolrQuery("*:*")); //Delete all sets
            _solrOperations.Commit();
            
            foreach (var user in _userRepo.GetAll())
                _solrOperations.Add(ToUserSolrMap.Run(user));

            _solrOperations.Commit();
        }
    }
}