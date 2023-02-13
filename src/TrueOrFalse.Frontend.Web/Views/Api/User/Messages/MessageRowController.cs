using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using NHibernate;


namespace VueApp;

public class MessageRow : BaseController
{
    private readonly ISession _session;
    public void MarkAsUnread(int id)
    {
        _session
            .CreateSQLQuery("UPDATE Message SET IsRead = " + 0 + " WHERE Id = " + id)
            .ExecuteUpdate();
    }

    public void MarkAsRead(int id)
    {
        _session
            .CreateSQLQuery("UPDATE Message SET IsRead = " + 1 + " WHERE Id = " + id)
            .ExecuteUpdate();
    }
}
