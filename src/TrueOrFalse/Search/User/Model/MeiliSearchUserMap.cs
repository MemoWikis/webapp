using System;
using SolrNet.Attributes;

namespace TrueOrFalse.Search
{
    public class MeiliSearchUserMap
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public int WishCountQuestions { get; set; }
        public int Rank { get; set; }
    }
}
