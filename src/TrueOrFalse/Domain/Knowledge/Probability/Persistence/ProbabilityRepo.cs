using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using Seedworks.Lib.Persistence;

public class ProbabilityRepo : RepositoryDb<Probability>
{
    public ProbabilityRepo(ISession session) : base(session)
    {

    }
}