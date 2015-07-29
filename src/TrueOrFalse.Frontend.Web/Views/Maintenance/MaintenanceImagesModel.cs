using System.Collections.Generic;
using TrueOrFalse;
using TrueOrFalse.Web;

public class MaintenanceImagesModel : BaseModel
{
    public UIMessage Message;
    public List<ImageMaintenanceInfo> ImageMaintenanceInfos;

    public PagerModel Pager;

    public bool CkbOpen { get; set; }
    public bool CkbExcluded { get; set; }
    public bool CkbApproved { get; set; }

    public int TotalResults;

    public MaintenanceImagesModel()
    {
    }

    public MaintenanceImagesModel(int? page)
    {
        Init(page);
    }

    public void Init(int? page)
    {
        var searchSpec = Sl.R<SessionUiData>().ImageMetaDataSearchSpec;

        if (page.HasValue)
            searchSpec.CurrentPage = page.Value;

        searchSpec.LicenseStates.Clear();
        if (CkbOpen)
            searchSpec.LicenseStates.Add(ImageLicenseState.Unknown);
        if (CkbExcluded)
            searchSpec.LicenseStates.Add(ImageLicenseState.NotApproved);
        if (CkbApproved)
            searchSpec.LicenseStates.Add(ImageLicenseState.Approved);

        ImageMaintenanceInfos = Resolve<GetImageMaintenanceInfos>().Run(searchSpec);
        TotalResults = searchSpec.TotalItems;

        Pager = new PagerModel(searchSpec);
    }
}