using System;
using SolrNet.Attributes;

namespace TrueOrFalse.Search
{
    public class UserSolrMap
    {
        [SolrUniqueKey("Id")]
        public int Id { get; set; }

        [SolrField("Name")]
        public string Name { get; set; }

        [SolrField("DateCreated")]
        public DateTime DateCreated { get; set; }

        [SolrField("WishCountQuestions")] 
        public int WishCountQuestions { get; set; }

        [SolrField("Rank")]
        public int Rank { get; set; }
    }
}
