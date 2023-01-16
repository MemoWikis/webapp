using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SolrNet.Attributes;

namespace TrueOrFalse.Search
{
    public class MeiliSearchQuestionMap : IRegisterAsInstancePerLifetime
    {
        public int Id { get; set; }
        public int? CreatorId { get; set; }
        public ICollection<int> ValuatorIds { get; set; }
        public bool IsPrivate { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        public string Solution { get; set; }
        public int SolutionType { get; set; }
        public ICollection<string> Categories { get; set; }
        public ICollection<int> CategoryIds { get; set; }
        public int AvgQuality { get; set; }
        public int Valuation { get; set; }
        public int Views { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
