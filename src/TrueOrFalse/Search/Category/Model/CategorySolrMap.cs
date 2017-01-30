using System;
using SolrNet.Attributes;

namespace TrueOrFalse.Search
{
    public class CategorySolrMap
    {
        [SolrUniqueKey("Id")]
        public int Id { get; set; }

        [SolrField("CreatorId")]
        public int CreatorId { get; set; }

        [SolrField("Name")]
        public string Name { get; set; }

        [SolrField("Description")]
        public string Description { get; set; }

        [SolrField("QuestionCount")]
        public int QuestionCount { get; set; }

        [SolrField("DateCreated")]
        public DateTime DateCreated { get; set; }
    }
}
