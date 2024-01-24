
public class OrderService
{
    public (List<TopicOrderNode> UpdatedOldOrder, List<TopicOrderNode> UpdatedNewOrder) MoveBefore(
        TopicOrderNode oldNode, 
        int beforeTopicId,
        int parentId,
        List<TopicOrderNode> oldOrder, 
        List<TopicOrderNode> newOrder)
    {
        if (oldOrder.FirstOrDefault().ParentId != parentId)
        {
            throw new InvalidOperationException("ParentId mismatch in the provided lists.");
        }

        var updatedOldOrder = RemoveNodeFromOrder(oldNode, oldOrder);
        var updatedNewOrder = AddBeforeNode(oldNode.TopicId, beforeTopicId, parentId, newOrder);

        return (updatedOldOrder, updatedNewOrder);
    }

    public (List<TopicOrderNode> UpdatedOldOrder, List<TopicOrderNode> UpdatedNewOrder) MoveAfter(
        TopicOrderNode oldNode,
        int afterTopicId,
        int parentId,
        List<TopicOrderNode> oldOrder,
        List<TopicOrderNode> newOrder)
    {
        if (oldOrder.FirstOrDefault()?.ParentId != parentId)
        {
            throw new InvalidOperationException("ParentId mismatch in the provided lists.");
        }

        var updatedOldOrder = RemoveNodeFromOrder(oldNode, oldOrder);
        var updatedNewOrder = AddAfterNode(oldNode.TopicId, afterTopicId, parentId, newOrder);

        return (updatedOldOrder, updatedNewOrder);
    }

    private List<TopicOrderNode> RemoveNodeFromOrder(TopicOrderNode node, List<TopicOrderNode> order)
    {
        var nodeIndex = order.IndexOf(node);
        if (nodeIndex != -1)
        {
            if (nodeIndex > 0)
            {
                order[nodeIndex - 1].NextId = nodeIndex < order.Count - 1 ? order[nodeIndex + 1].TopicId : (int?)null;
            }
            if (nodeIndex < order.Count - 1)
            {
                order[nodeIndex + 1].PreviousId = nodeIndex > 0 ? order[nodeIndex - 1].TopicId : (int?)null;
            }
            order.RemoveAt(nodeIndex);
        }

        return order;
    }

    private List<TopicOrderNode> AddBeforeNode(int topicId, int beforeTopicId, int parentId, List<TopicOrderNode> order)
    {
        return InsertNode(topicId, beforeTopicId, parentId, order, false);
    }

    private List<TopicOrderNode> AddAfterNode(int topicId, int afterTopicId, int parentId, List<TopicOrderNode> order)
    {
        return InsertNode(topicId, afterTopicId, parentId, order, true);
    }

    private List<TopicOrderNode> InsertNode(int topicId, int targetTopicId, int parentId, List<TopicOrderNode> order, bool insertAfter)
    {
        var node = new TopicOrderNode
        {
            TopicId = topicId,
            ParentId = parentId,
            PreviousId = insertAfter ? targetTopicId : null,
            NextId = !insertAfter ? targetTopicId : null,
        };
        int targetPosition = order.FindIndex(n => n.TopicId == targetTopicId);
        if (targetPosition == -1)
        {
            throw new InvalidOperationException("Target node not found in the order.");
        }

        int positionToInsert = insertAfter ? targetPosition + 1 : targetPosition;

        order.Insert(positionToInsert, node);

        node.PreviousId = insertAfter ? targetTopicId : (positionToInsert > 0 ? order[positionToInsert - 1].TopicId : (int?)null);
        node.NextId = positionToInsert < order.Count - 1 ? order[positionToInsert + 1].TopicId : (int?)null;

        if (positionToInsert > 0 && !insertAfter)
        {
            order[positionToInsert - 1].NextId = node.TopicId;
        }
        if (positionToInsert < order.Count - 1 && insertAfter)
        {
            order[positionToInsert + 1].PreviousId = node.TopicId;
        }
        if (insertAfter)
        {
            order[targetPosition].NextId = node.TopicId;
        }
        else if (targetPosition > 0)
        {
            order[targetPosition - 1].NextId = node.TopicId;
        }

        return order;
    }

    public List<TopicOrderNode> SortTopics(List<TopicOrderNode> nodes)
    {
        var sortedList = new List<TopicOrderNode>();
        var current = nodes.FirstOrDefault(x => x.PreviousId == null);

        while (current != null)
        {
            sortedList.Add(current);
            current = nodes.FirstOrDefault(x => x.TopicId == current.NextId);
        }

        return sortedList;
    }
}