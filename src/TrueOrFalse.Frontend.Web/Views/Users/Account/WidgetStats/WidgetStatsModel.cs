using System.Collections.Generic;

public class WidgetStatsModel : BaseModel
{
    public IList<string> Hosts;

    public WidgetStatsModel()
    {
        var user = R<UserRepo>().GetById(UserId);
        Hosts = user.WidgetHosts() ?? new List<string>();
    }

}