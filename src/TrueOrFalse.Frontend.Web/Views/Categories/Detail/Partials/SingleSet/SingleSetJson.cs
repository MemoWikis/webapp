public class SingleSetJson
{
    public Set Set;
    public int SetId;
    public string SetText;

    public SingleSetJson(Set set)
    {
        SetId = set.Id;
        Set = set;
        SetText = set.Text;
    }
}
