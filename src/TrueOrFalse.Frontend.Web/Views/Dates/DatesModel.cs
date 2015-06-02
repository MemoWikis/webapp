using System.Collections.Generic;

public class DatesModel : BaseModel
{
    public IList<Date> Dates = new List<Date>();
    public IList<Date> DatesInNetwork = new List<Date>();

    public DatesModel()
    {
    }
}
