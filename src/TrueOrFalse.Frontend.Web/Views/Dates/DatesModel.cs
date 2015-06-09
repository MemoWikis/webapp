using System.Collections.Generic;
using System.Linq;

public class DatesModel : BaseModel
{
    public IList<Date> Dates = new List<Date>();
    public IList<Date> DatesInNetwork = new List<Date>();

    public DatesModel()
    {
        if (!IsLoggedIn)
            return;

        Dates = R<DateRepo>().GetBy(UserId);
    }
}
