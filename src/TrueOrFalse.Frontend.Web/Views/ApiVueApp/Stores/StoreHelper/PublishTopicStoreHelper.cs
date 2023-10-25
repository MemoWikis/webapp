using System.Collections.Generic;

namespace HelperClassesControllers;

public class PublishTopicStoreHelper
{
    public class PublishTopicJson
    {
        public int id { get; set; }
    }

    public class PublishQuestionsJson
    {
        public List<int> questionIds { get; set; }
    }
}

