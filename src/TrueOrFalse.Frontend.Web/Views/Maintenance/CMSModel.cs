using System;
using System.Collections.Generic;
using System.Linq;
using TrueOrFalse.Infrastructure;
using TrueOrFalse.Web;

public class CMSModel : BaseModel
{
    public UIMessage Message;

    public CMSModel Init()
    {
        return this;
    }
}