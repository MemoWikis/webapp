using System.Collections.Generic;
using System.Linq;
using NHibernate;

public class QuestionChangesOverviewModel : BaseModel
{
    public IList<QuestionChangeDayModel> Days;
    public int PageToShow;

    public QuestionChangesOverviewModel(int pageToShow)
    {
        PageToShow = pageToShow;
        const int revisionsToShow = 10;
        var revisionsToSkip = (PageToShow - 1) * revisionsToShow;
        var query = $@"
            
            SELECT * FROM QuestionChange qc ORDER BY qc.DateCreated DESC LIMIT {revisionsToSkip},{revisionsToShow}

            ";
        var revisions = Sl.R<ISession>().CreateSQLQuery(query).AddEntity(typeof(QuestionChange)).List<QuestionChange>();

        Days = revisions
            .GroupBy(change => change.DateCreated.Date)
            .OrderByDescending(group => group.Key)
            .Select(group => new QuestionChangeDayModel(
                group.Key,
                (IList<QuestionChange>)group.OrderByDescending(g => g.DateCreated).ToList()
            ))
            .ToList();
    }
}