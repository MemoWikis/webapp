using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
