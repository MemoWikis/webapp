using System.Collections.Generic;
using System.Linq;
using NHibernate;
using TrueOrFalse.Search;
using TrueOrFalse.Web;
using TrueOrFalse.Web.Context;

namespace TrueOrFalse
{
    public class UserRepository : RepositoryDbBase<User>
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

        public User GetByName(string name)
        {
            return _session.QueryOver<User>()
               .Where(k => k.Name == name)
               .SingleOrDefault<User>();
        }

        public override void Update(User user)
        {
            ThrowIfNot_IsUserOrAdmin(user.Id);

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

            ThrowIfNot_IsUserOrAdmin(user.Id);

            if (Sl.R<SessionUser>().IsValidUserOrAdmin(user.Id))
                throw new InvalidAccessException();

            _searchIndexUser.Delete(user);
            base.Delete(id);
        }

        public override IList<User> GetByIds(params int[] userIds)
        {
            var questions = base.GetByIds(userIds);
            return userIds.Select(t => questions.First(q => q.Id == t)).ToList();
        }

        public IList<User> GetWhereReputationIsBetween(int newReputation, int oldRepution)
        {
            var query = _session.QueryOver<User>();

            if(newReputation < oldRepution)
                query.Where(u => u.Reputation > newReputation && u.Reputation < oldRepution);
            else
                query.Where(u => u.Reputation < newReputation && u.Reputation > oldRepution);
       
            return query.List();
        }
    }
}
