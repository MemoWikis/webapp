using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

[Serializable]
public class FeaturedSetsJson
{
    public int[] FeaturedSetIds;

    public List<Set> FeaturedSets;

    public string FeaturedSetsHeading;

    public string LearningButtonText;
}
