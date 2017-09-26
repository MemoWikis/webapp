using System.Collections.Generic;

public class WidgetViewsModel : BaseModel
{
    public IList<string> Hosts;

    public WidgetViewsModel()
    {
        var user = R<UserRepo>().GetById(UserId);
        Hosts = user.WidgetHosts() ?? new List<string>();
    }

}