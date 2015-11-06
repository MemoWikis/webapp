using System.Collections.Generic;
using NHibernate;

public class GetLastAnswers : IRegisterAsInstancePerLifetime
{
    public IList<Answer> Run(int userId, int amount)
    {
        return Sl.R<ISession>()
            .QueryOver<Answer>()
            .Where(a => a.UserId == userId)
            .OrderBy(a => a.DateCreated).Desc
            .Take(amount)
            .List<Answer>();
    }
}