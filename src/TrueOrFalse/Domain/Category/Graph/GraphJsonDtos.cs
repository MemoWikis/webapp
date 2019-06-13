using System.Collections.Generic;

namespace GraphJsonDtos
{
    public class Node
    {
        public KnowledgeSummary Knowledge;
        
        public int CategoryId;
        public string Text;
    }

    public class Link
    {
        public int source;
        public int target;
    }
}