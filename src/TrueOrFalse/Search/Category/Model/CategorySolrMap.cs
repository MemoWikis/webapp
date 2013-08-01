using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        [SolrField("DateCreated")]
        public DateTime DateCreated { get; set; }
    }
}
