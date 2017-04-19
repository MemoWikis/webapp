using System;
using System.Collections.Generic;
using System.Linq;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse.Infrastructure
{
    public class DbSettings : DomainEntity
    {
        public virtual int AppVersion { get; set; }
        public virtual string SuggestedGames { get; set; }

        public virtual List<Set> SuggestedGameSets()
        {
            var setIds = SuggestedGames
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => Convert.ToInt32(x));

            var setRepo = Sl.R<SetRepo>();

            return setIds
                .Select(setId => setRepo.GetById(setId))
                .Where(set => set != null)
                .ToList();
        }

        public virtual string SuggestedSetsIdString { get; set; }

        public virtual List<Set> SuggestedSets()
        {
            if (string.IsNullOrEmpty(SuggestedSetsIdString))
                return new List<Set>();

            var setIds = SuggestedSetsIdString
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => Convert.ToInt32(x));

            var setRepo = Sl.R<SetRepo>();

            return setIds
                .Select(setId => setRepo.GetById(setId))
                .Where(set => set != null)
                .ToList();
        }
    }
}