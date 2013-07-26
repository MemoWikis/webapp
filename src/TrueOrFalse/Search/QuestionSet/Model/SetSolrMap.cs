using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolrNet.Attributes;

namespace TrueOrFalse.Search
{
    public class SetSolrMap
    {
        [SolrUniqueKey("Id")]
        public int Id { get; set; }

        [SolrField("CreatorId")]
        public int CreatorId { get; set; }

        [SolrField("Text")]
        public string Name { get; set; }

        [SolrField("Description")]
        public string Text { get; set; }

        [SolrField("AllQuestionsTitles")]
        public string AllQuestionsTitles { get; set; }

        [SolrField("AllQuestionsBodies")]
        public string AllQuestionsBodies { get; set; }

        [SolrField("DateCreated")]
        public DateTime DateCreated { get; set; }
    }
}
