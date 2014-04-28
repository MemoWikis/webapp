using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using TrueOrFalse;
using TrueOrFalse.Web;
using TrueOrFalse.Frontend.Web.Models;

public class MaintenanceImagesModel : BaseModel
{
    public UIMessage Message;
    public List<ImageMaintenanceInfo> ImageMaintenanceInfos;

    public MaintenanceImagesModel()
    {
        ImageMaintenanceInfos = Resolve<GetImageMaintenanceInfos>().Run();
    }
}
