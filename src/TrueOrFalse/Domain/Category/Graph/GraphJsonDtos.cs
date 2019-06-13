using System.Collections.Generic;

namespace GraphJsonDtos
{
    public class Node
    {
        public int CategoryId;
        public string Text;
    }

    public class Link
    {
        public int source;
        public int target;
    }

    public class Knowledge
    {
        public int NotInWishknowledge;
        public int NotLearned;
        public int NeedsLearning;
        public int NeedsConsolidation;
        public int Solid;
        public int Options;
    }
}