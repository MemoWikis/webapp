using System.Collections.Generic;
using TrueOrFalse;
using TrueOrFalse.Web;

public class MaintenanceImagesModel : BaseModel
{
    public UIMessage Message;
    public List<ImageMaintenanceInfo> ImageMaintenanceInfos;

    public PagerModel Pager { get; set; }

    public MaintenanceImagesModel(int? page)
    {
        var searchSpec = Sl.R<SessionUiData>().ImageMetaDataSearchSpec;

        if (page.HasValue)
            searchSpec.CurrentPage = page.Value;

        ImageMaintenanceInfos = Resolve<GetImageMaintenanceInfos>().Run(searchSpec);

        Pager = new PagerModel(searchSpec);
    }
}
