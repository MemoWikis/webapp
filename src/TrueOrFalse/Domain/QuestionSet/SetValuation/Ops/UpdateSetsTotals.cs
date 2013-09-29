using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace TrueOrFalse
{
    public class UpdateSetsTotals : IRegisterAsInstancePerLifetime
    {
        private readonly ISession _session;
        private readonly SetValuationRepository _setValuationRepo;

        public UpdateSetsTotals(ISession session, SetValuationRepository setValuationRepo){
            _session = session;
            _setValuationRepo = setValuationRepo;
        }

        public void Run(SetValuation setValuation)
        {
            
        }

        public void UpdateRelevancePersonal(int id, int i, int newValue)
        {
        }
    }
}
