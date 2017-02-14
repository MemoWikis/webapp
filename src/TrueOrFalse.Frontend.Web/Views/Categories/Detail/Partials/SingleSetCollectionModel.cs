using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;
using NHibernate.Util;


public class SingleSetCollectionModel : BaseModel
{
    public IList<Set> Sets;
    public string Title;
    public string CardOrientation;

   public SingleSetCollectionModel(IList<Set> sets, string title = "", string cardOrientationLandscapeOrPortrait = null)
    {
        Sets = sets;
        Title = title;

        if (cardOrientationLandscapeOrPortrait == "Landscape" || cardOrientationLandscapeOrPortrait == "Portrait")
            CardOrientation = cardOrientationLandscapeOrPortrait;
        else
        {
            CardOrientation = Sets.Count % 3 == 0 ? "Portrait" : "Landscape";
        }
    }
}
