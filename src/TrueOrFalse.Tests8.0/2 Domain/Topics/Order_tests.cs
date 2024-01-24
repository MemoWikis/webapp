

class Order_tests : BaseTest
{
    [Test]
    public void SortTopics_ShouldCorrectlySortNodes()
    {
        var unorderedNodes = new List<TopicOrderNode>
        {
            new TopicOrderNode { TopicId = 3, ParentId = 10, PreviousId = 2, NextId = null },
            new TopicOrderNode { TopicId = 1, ParentId = 10, PreviousId = null, NextId = 2 },
            new TopicOrderNode { TopicId = 2, ParentId = 10, PreviousId = 1, NextId = 3 }
        };

        var oderService = new OrderService();
        var sortedNodes = oderService.SortTopics(unorderedNodes);

        Assert.IsNotNull(sortedNodes);
        Assert.AreEqual(3, sortedNodes.Count);
        Assert.AreEqual(1, sortedNodes[0].TopicId);
        Assert.AreEqual(2, sortedNodes[1].TopicId);
        Assert.AreEqual(3, sortedNodes[2].TopicId);
    }

    [Test]
    public void MoveBefore_ShouldCorrectlyReorderNodes()
    {
        var oldOrder = new List<TopicOrderNode>
        {
            new TopicOrderNode { TopicId = 1, ParentId = 10, PreviousId = null, NextId = 2 },
            new TopicOrderNode { TopicId = 2, ParentId = 10, PreviousId = 1, NextId = 3 },
            new TopicOrderNode { TopicId = 3, ParentId = 10, PreviousId = 2, NextId = null }
        };
        var newOrder = new List<TopicOrderNode>
        {
            new TopicOrderNode { TopicId = 4, ParentId = 10, PreviousId = null, NextId = 5 },
            new TopicOrderNode { TopicId = 5, ParentId = 10, PreviousId = 4, NextId = null }
        };

        var oldNode = oldOrder[1];
        int beforeTopicId = 5;
        int parentId = 10;

        var oderService = new OrderService();
        var result = oderService.MoveBefore(oldNode, beforeTopicId, parentId, oldOrder, newOrder);

        Assert.IsNotNull(result.UpdatedOldOrder);
        Assert.IsNotNull(result.UpdatedNewOrder);

        Assert.IsFalse(result.UpdatedOldOrder.Any(n => n.TopicId == oldNode.TopicId));

        var newNodeIndex = result.UpdatedNewOrder.FindIndex(n => n.TopicId == oldNode.TopicId);
        Assert.AreEqual(1, newNodeIndex);
        Assert.AreEqual(4, result.UpdatedNewOrder[newNodeIndex - 1].TopicId);
        Assert.AreEqual(5, result.UpdatedNewOrder[newNodeIndex + 1].TopicId);
    }
}
