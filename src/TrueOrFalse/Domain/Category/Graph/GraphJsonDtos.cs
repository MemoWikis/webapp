namespace GraphJsonDtos
{
    public class Node
    {
        public KnowledgeSummary Knowledge;
        public int CategoryId;
        public string Title;
        public int Id;
        public int Level;
    }

    public class Link
    {
        public Link() { }

        public Link(int source, int target)
        {
            this.source = source;
            this.target = target;
        }

        public int source;
        public int target;
    }
}