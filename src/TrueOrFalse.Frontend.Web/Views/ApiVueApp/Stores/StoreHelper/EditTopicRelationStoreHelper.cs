namespace HelperClassesControllers;

public class EditTopicRelationStoreHelper
{
    public class RemoveTopicsJson
    {
        public int parentId { get; set; }
        public int[] childIds { get; set; }
    }
}

