

class Order_tests : BaseTest
{
    [Test]
    public void SortTopics_ShouldCorrectlySortTopics()
    {
        var unsortedRelations = new List<CategoryCacheRelation>
        {
            new CategoryCacheRelation { ChildId = 3, ParentId = 10, PreviousId = 2, NextId = null },
            new CategoryCacheRelation { ChildId = 1, ParentId = 10, PreviousId = null, NextId = 2 },
            new CategoryCacheRelation { ChildId = 2, ParentId = 10, PreviousId = 1, NextId = 3 }
        };

        var sortedRelations = TopicOrderer.Sort(unsortedRelations, 10);

        Assert.IsNotNull(sortedRelations);
        Assert.AreEqual(3, sortedRelations.Count);
        Assert.AreEqual(1, sortedRelations[0].ChildId);
        Assert.AreEqual(2, sortedRelations[1].ChildId);
        Assert.AreEqual(3, sortedRelations[2].ChildId);
    }

    //[Test]
    //public void MoveBefore_ShouldCorrectlyReorderNodes_SameParentId()
    //{
    //    var oldRelations = new List<CategoryCacheRelation>
    //    {
    //        new CategoryCacheRelation { ChildId = 2, ParentId = 10, PreviousId = null, NextId = 3 },
    //        new CategoryCacheRelation { ChildId = 4, ParentId = 10, PreviousId = 2, NextId = 4 },
    //        new CategoryCacheRelation { ChildId = 5, ParentId = 10, PreviousId = 3, NextId = null }
    //    };

    //    var relationToMove = oldRelations[1];
    //    int beforeTopicId = 1;
    //    int parentId = 10;

    //    var result = TopicOrderer.MoveBefore(relationToMove, beforeTopicId, parentId, parentId);

    //    Assert.IsNotNull(result.UpdatedOldOrder);
    //    Assert.IsNotNull(result.UpdatedNewOrder);

    //    Assert.IsFalse(result.UpdatedOldOrder.Any(n => n.ChildId == relationToMove.ChildId));

    //    var newNodeIndex = result.UpdatedNewOrder.FindIndex(n => n.ChildId == relationToMove.ChildId);
    //    Assert.AreEqual(1, newNodeIndex);
    //    Assert.AreEqual(4, result.UpdatedNewOrder[newNodeIndex - 1].ChildId);
    //    Assert.AreEqual(5, result.UpdatedNewOrder[newNodeIndex + 1].ChildId);
    //}

    //[Test]
    //public void MoveAfter_ShouldCorrectlyReorderNodes_SameParentId()
    //{
    //    var oldRelations = new List<CategoryCacheRelation>
    //    {
    //        new CategoryCacheRelation { ChildId = 2, ParentId = 10, PreviousId = null, NextId = 3 },
    //        new CategoryCacheRelation { ChildId = 3, ParentId = 10, PreviousId = 2, NextId = 4 },
    //        new CategoryCacheRelation { ChildId = 4, ParentId = 10, PreviousId = 3, NextId = null }
    //    };

    //    var relationToMove = oldRelations[1];
    //    int afterTopicId = 3;
    //    int parentId = 10;

    //    var result = TopicOrderer.MoveAfter(relationToMove, afterTopicId, parentId, parentId);

    //    Assert.IsNotNull(result.UpdatedOldOrder);
    //    Assert.IsNotNull(result.UpdatedNewOrder);

    //    Assert.IsFalse(result.UpdatedOldOrder.Any(n => n.ChildId == relationToMove.ChildId));

    //    var newNodeIndex = result.UpdatedNewOrder.FindIndex(n => n.ChildId == relationToMove.ChildId);
    //    Assert.AreEqual(1, newNodeIndex);
    //    Assert.AreEqual(5, result.UpdatedNewOrder[newNodeIndex + 1].ChildId);
    //    Assert.AreEqual(4, result.UpdatedNewOrder[newNodeIndex - 1].ChildId);
    //}
}
