using System.Collections.Generic;

namespace GraphJsonDtos
{
    public class Node
    {
        public KnowledgeSummary Knowledge;
        public int CategoryId;
        public string Title;
        public int Id;
    }

    public class Link
    {
        public int source;
        public int target;
    }
}