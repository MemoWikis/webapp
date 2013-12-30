using System.Collections.Generic;
using System.Linq;
using NHibernate;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Search;

namespace TrueOrFalse
{
    public class UserRepository : RepositoryDb<User>
    {
        private readonly SearchIndexUser _searchIndexUser;

        public UserRepository(ISession session, SearchIndexUser searchIndexUser): base(session){
            _searchIndexUser = searchIndexUser;
        }

        public User GetByEmail(string emailAddress)
        {
            return _session.QueryOver<User>()
                           .Where(k => k.EmailAddress == emailAddress)
                           .SingleOrDefault<User>();
        }

        public override void Update(User user)
        {
            _searchIndexUser.Update(user);
            base.Update(user);
        }

        public override void Create(User user)
        {
            base.Create(user);
            _searchIndexUser.Update(user);
        }

        public override void Delete(int id)
        {
            var user = GetById(id);
            _searchIndexUser.Delete(user);
            base.Delete(id);
        }

        public override IList<User> GetByIds(params int[] userIds)
        {
            var questions = base.GetByIds(userIds);
            return userIds.Select(t => questions.First(q => q.Id == t)).ToList();
        }
    }
}
