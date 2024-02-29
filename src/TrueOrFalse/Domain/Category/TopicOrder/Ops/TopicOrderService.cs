
public class TopicOrderService
{
    private readonly CategoryRepository _categoryRepository;

    public TopicOrderService(CategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public (List<CategoryRelation> UpdatedOldOrder, List<CategoryRelation> UpdatedNewOrder) MoveBefore(
        CategoryRelation relation, 
        int beforeTopicId,
        int newParentId,
        int oldParentId)
    {
        var updatedNewOrder = AddBefore(relation.Child.Id, beforeTopicId, newParentId);
        var updatedOldOrder = Remove(relation, oldParentId);

        return (updatedOldOrder, updatedNewOrder);
    }

    public (List<CategoryRelation> UpdatedOldOrder, List<CategoryRelation> UpdatedNewOrder) MoveAfter(
        CategoryRelation relation,
        int afterTopicId,
        int newParentId,
        int oldParentId)
    {
        var updatedNewOrder = AddAfter(relation.Child.Id, afterTopicId, newParentId);
        var updatedOldOrder = Remove(relation, oldParentId);

        return (updatedOldOrder, updatedNewOrder);
    }

    private List<CategoryRelation> Remove(CategoryRelation relation, int oldParentId)
    {
        var relations = _categoryRepository.GetById(oldParentId).ParentRelations;

        var nodeIndex = relations.IndexOf(relation);
        if (nodeIndex != -1)
        {
            if (nodeIndex > 0)
            {
                relations[nodeIndex - 1].NextId = nodeIndex < relations.Count - 1 ? relations[nodeIndex + 1].Child.Id : (int?)null;
            }
            if (nodeIndex < relations.Count - 1)
            {
                relations[nodeIndex + 1].PreviousId = nodeIndex > 0 ? relations[nodeIndex - 1].Child.Id : (int?)null;
            }
            relations.RemoveAt(nodeIndex);
        }

        return relations.ToList();
    }

    private List<CategoryRelation> AddBefore(int topicId, int beforeTopicId, int parentId)
    {
        var parent = _categoryRepository.GetById(parentId);
        var relations = parent.ParentRelations.ToList();
        var child = _categoryRepository.GetById(topicId);
        return Insert(child, beforeTopicId, parent, relations, false);
    }

    private List<CategoryRelation> AddAfter(int topicId, int afterTopicId, int parentId)
    {
        var parent = _categoryRepository.GetById(parentId);
        var relations = parent.ParentRelations.ToList();
        var child = _categoryRepository.GetById(topicId);
        return Insert(child, afterTopicId, parent, relations, true);
    }

    private List<CategoryRelation> Insert(Category child, int targetTopicId, Category parent, List<CategoryRelation> relations, bool insertAfter)
    {
        var node = new CategoryRelation
        {
            Child = child,
            Parent = parent,
            PreviousId = insertAfter ? targetTopicId : null,
            NextId = !insertAfter ? targetTopicId : null,
        };
        int targetPosition = relations.FindIndex(n => n.Child.Id == targetTopicId);
        if (targetPosition == -1)
        {
            throw new InvalidOperationException("Target node not found in the order.");
        }

        int positionToInsert = insertAfter ? targetPosition + 1 : targetPosition;

        relations.Insert(positionToInsert, node);

        node.PreviousId = insertAfter ? targetTopicId : (positionToInsert > 0 ? relations[positionToInsert - 1].Child.Id : (int?)null);
        node.NextId = positionToInsert < relations.Count - 1 ? relations[positionToInsert + 1].Child.Id : (int?)null;

        if (positionToInsert > 0 && !insertAfter)
        {
            relations[positionToInsert - 1].NextId = node.Child.Id;
        }
        if (positionToInsert < relations.Count - 1 && insertAfter)
        {
            relations[positionToInsert + 1].PreviousId = node.Child.Id;
        }
        if (insertAfter)
        {
            relations[targetPosition].NextId = node.Child.Id;
        }
        else if (targetPosition > 0)
        {
            relations[targetPosition - 1].NextId = node.Child.Id;
        }

        return relations;
    }

    public List<CategoryRelation> SortTopics(List<CategoryRelation> categoryRelations)
    {
        var sortedList = new List<CategoryRelation>();
        var current = categoryRelations.FirstOrDefault(x => x.PreviousId == null);

        while (current != null)
        {
            sortedList.Add(current);
            current = categoryRelations.FirstOrDefault(x => x.Child.Id == current.NextId);
        }

        return sortedList;
    }
}