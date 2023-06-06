using System.Collections.Generic;
using SolrNet.Attributes;

namespace TrueOrFalse.Search
{
    //todo mark to delete
    public class CategorySolrMap
    {
        [SolrUniqueKey("Id")]
        public int Id { get; set; }

        [SolrField("CreatorId")]
        public int CreatorId { get; set; }

        [SolrField("ValuatorIds")]
        public ICollection<int> ValuatorIds { get; set; }

        [SolrField("ValuationsCount")]
        public int ValuationsCount { get; set; }

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
