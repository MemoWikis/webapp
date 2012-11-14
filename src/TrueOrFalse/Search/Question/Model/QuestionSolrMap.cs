using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using SolrNet.Attributes;

namespace TrueOrFalse.Search
{
    public class QuestionSolrMap
    {
        [SolrUniqueKey("Id")]
        public int Id { get; set; }

        [SolrField("CreatorId")]
        public int CreatorId { get; set; }

        [SolrField("Title")]
        public string Title { get; set; }

        [SolrField("Description")]
        public string Description { get; set; }

        [SolrField("Solution")]
        public string Solution { get; set; }

        [SolrField("SolutionType")]
        public int SolutionType { get; set; }

        [SolrField("Categories")]
        public ICollection<string> Categories { get; set; }

        [SolrField("Quality")]
        public int Quality { get; set; }
        
        [SolrField("Views")]
        public int Views { get; set; }
        
        [SolrField("DateCreated")]
        public DateTime DateCreated { get; set; }

        public QuestionSolrMap()
        {
            Categories = new Collection<string>();
        }
    }
}
