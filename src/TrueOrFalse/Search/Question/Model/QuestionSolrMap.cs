using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SolrNet.Attributes;

namespace TrueOrFalse.Search
{
    public class QuestionSolrMap
    {
        [SolrUniqueKey("Id")]
        public int Id { get; set; }

        [SolrField("CreatorId")]
        public int CreatorId { get; set; }

        [SolrField("ValuatorIds")]
        public ICollection<int> ValuatorIds { get; set; }

        [SolrField("IsPrivate")]
        public bool IsPrivate { get; set; }

        [SolrField("Text")]
        public string Text { get; set; }

        [SolrField("Description")]
        public string Description { get; set; }

        [SolrField("Solution")]
        public string Solution { get; set; }

        [SolrField("SolutionType")]
        public int SolutionType { get; set; }

        [SolrField("Categories")]
        public ICollection<string> Categories { get; set; }

        [SolrField("CategoryIds")]
        public ICollection<int> CategoryIds { get; set; }

        [SolrField("Quality")]
        public int AvgQuality { get; set; }

        [SolrField("Valuation")]
        public int Valuation { get; set; }

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
