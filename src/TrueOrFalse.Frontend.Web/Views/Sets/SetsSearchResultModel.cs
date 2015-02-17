using System.Collections.Generic;

public class SetsSearchResultModel : BaseModel
{
    public bool NotAllowed;
    public IEnumerable<SetRowModel> SetRows { get; set; }
    public PagerModel Pager { get; set; }

    public SetsSearchResultModel(SetsModel setsModel)
    {
        NotAllowed = setsModel.AccessNotAllowed;
        SetRows = setsModel.Rows;
        Pager = setsModel.Pager;
    }
}
