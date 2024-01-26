[Serializable]
public class TopicOrderCache
{
    public IList<TopicOrderNodeCacheItem> ToTopicOrders(List<TopicOrderNode> topicOrderNode)
    {
        var result = new List<TopicOrderNodeCacheItem>();

        if (topicOrderNode.Count <= 0 || topicOrderNode == null)
        {
            return result;
        }

        foreach (var node in topicOrderNode)
        {
            result.Add(ToTopicOrderNodeCacheItem(node));
        }

        return result;
    }

    public static TopicOrderNodeCacheItem ToTopicOrderNodeCacheItem(TopicOrderNode node)
    {
        return new TopicOrderNodeCacheItem
        {
            TopicId = node.TopicId,
            ParentId = node.ParentId,
            PreviousId = node.PreviousId,
            NextId = node.NextId
        };
    }
}