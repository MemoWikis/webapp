using System.Collections.Generic;
using System.Linq;

public class DatesModel : BaseModel
{
    public IList<Date> Dates = new List<Date>();
    public IList<Date> DatesInNetwork = new List<Date>();

    public bool HasPreviousDates = false;

    public DatesModel()
    {
        if (!IsLoggedIn)
            return;

        var dateRepo = R<DateRepo>();

        Dates = dateRepo.GetBy(UserId, onlyUpcoming: true);
        HasPreviousDates = dateRepo.AmountOfPreviousItems(UserId) > 0;
    }
}