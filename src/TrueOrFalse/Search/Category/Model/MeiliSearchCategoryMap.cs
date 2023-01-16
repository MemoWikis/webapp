using System;
using System.Collections.Generic;
using SolrNet.Attributes;

namespace TrueOrFalse.Search
{
    public class MeiliSearchCategoryMap
    {
        public int Id { get; set; }
        public int CreatorId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int QuestionCount { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
