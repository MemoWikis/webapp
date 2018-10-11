using System.Collections.Generic;
using NHibernate.Util;

public class DatesModel : BaseModel
{
    public IList<Date> Dates = new List<Date>();
    public IList<Date> DatesInNetwork = new List<Date>();

    public bool HasPreviousDates = false;

    public bool IsFollowingAnyUsers = false;

    public DatesModel()
    {
        if (!IsLoggedIn)
            return;

        var dateRepo = R<DateRepo>();

        HasPreviousDates = dateRepo.AmountOfPreviousItems(UserId) > 0;

        IsFollowingAnyUsers = R<TotalIFollow>().Run(UserId) > 0;

        Dates = dateRepo.GetBy(UserId, onlyUpcoming: true);
        DatesInNetwork = R<GetDatesInNetwork>().Run(UserId);
    }
}