[Serializable]
public class TopicOrderNodeCacheItem
{
    public virtual required int TopicId { get; set; }
    public virtual required int ParentId { get; set; }
    public virtual int? PreviousId { get; set; }
    public virtual int? NextId { get; set; }
}