using System.Collections.Generic;
using System.Linq;

public class Crumbtrail
{
    public CrumbtrailItem Current;
    public IList<CrumbtrailItem> Items = new List<CrumbtrailItem>();

    public Crumbtrail(Category category)
    {
        Current = new CrumbtrailItem(category);
    }

    public string ToDebugString() => Items
        .Select(c => c.Text)
        .Aggregate((a, b) => a + " => " + b) 
        + " => [" + Current.Text + "]";
}
