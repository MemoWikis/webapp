using System.Collections.Generic;
using System.Linq;
using NHibernate;

public class HistoryOfEverythingModel : BaseModel
{
    public IList<ChangeDayModel> Days;
    public int PageToShow;
    
    public HistoryOfEverythingModel(int pageToShow)
    {
        PageToShow = pageToShow;
        const int revisionsToShow = 10;
        var revisionsToSkip = (PageToShow - 1) * revisionsToShow;
        var query = $@"
            
            SELECT * FROM CategoryChange cc ORDER BY cc.DateCreated DESC LIMIT {revisionsToSkip},{revisionsToShow}

            ";
        var revisions = Sl.R<ISession>().CreateSQLQuery(query).AddEntity(typeof(CategoryChange)).List<CategoryChange>();
        
        Days = revisions
            .GroupBy(change => change.DateCreated.Date)
            .OrderByDescending(group => group.Key)
            .Select(group => new ChangeDayModel(
                group.Key,
                (IList<CategoryChange>)group.OrderByDescending(g => g.DateCreated).ToList()
            ))
            .ToList();
    }
}