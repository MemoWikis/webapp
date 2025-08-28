using NHibernate;

public class UserSkillRepo(ISession session) : RepositoryDb<UserSkill>(session)
{
    public IList<UserSkill> GetByUserId(int userId)
    {
        return Session
            .QueryOver<UserSkill>()
            .Where(us => us.UserId == userId)
            .OrderBy(us => us.DateCreated).Desc
            .List();
    }

    public UserSkill GetByUserAndPage(int userId, int pageId)
    {
        return Session
            .QueryOver<UserSkill>()
            .Where(us => us.UserId == userId && us.PageId == pageId)
            .SingleOrDefault();
    }

    public void DeleteByUserAndPage(int userId, int pageId)
    {
        var userSkill = GetByUserAndPage(userId, pageId);
        if (userSkill != null)
        {
            Session.Delete(userSkill);
        }
    }

    public int GetCountByUserId(int userId)
    {
        return Session
            .QueryOver<UserSkill>()
            .Where(us => us.UserId == userId)
            .RowCount();
    }

    public IList<UserSkill> GetByPageId(int pageId)
    {
        return Session
            .QueryOver<UserSkill>()
            .Where(us => us.PageId == pageId)
            .OrderBy(us => us.DateCreated).Desc
            .List();
    }
}
