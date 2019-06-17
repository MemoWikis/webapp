using System.Collections.Generic;

namespace GraphJsonDtos
{
    public class Node
    {
        public KnowledgeSummary Knowledge;
        public int CategoryId;
        public string title;
        public int id;
    }

    public class Link
    {
        public int source;
        public int target;
    }
}