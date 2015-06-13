using System.Collections.Generic;
using TrueOrFalse;
using TrueOrFalse.Web;

public class MaintenanceImagesModel : BaseModel
{
    public UIMessage Message;
    public List<ImageMaintenanceInfo> ImageMaintenanceInfos;

    public MaintenanceImagesModel()
    {
        ImageMaintenanceInfos = Resolve<GetImageMaintenanceInfos>().Run();
    }
}
