using System.Collections.Generic;
using TrueOrFalse;
using TrueOrFalse.Web;

public class MaintenanceImagesModel : BaseModel
{
    public UIMessage Message;
    public List<ImageMaintenanceInfo> ImageMaintenanceInfos;

    public PagerModel Pager { get; set; }

    public bool CkbOpen { get; set; }
    public bool CkbExcluded { get; set; }
    public bool CkbApproved { get; set; }

    public MaintenanceImagesModel(){}

    public MaintenanceImagesModel(int? page)
    {
        Init(page);
    }

    private void Init(int? page)
    {
        var searchSpec = Sl.R<SessionUiData>().ImageMetaDataSearchSpec;

        if (page.HasValue)
            searchSpec.CurrentPage = page.Value;


        ImageMaintenanceInfos = Resolve<GetImageMaintenanceInfos>().Run(searchSpec);

        Pager = new PagerModel(searchSpec);
    }
}