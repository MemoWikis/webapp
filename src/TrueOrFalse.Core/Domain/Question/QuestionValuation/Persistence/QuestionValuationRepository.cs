﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse.Core
{
    public class QuestionValuationRepository : RepositoryDb<QuestionValuation> 
    {
        public QuestionValuationRepository(ISession session) : base(session){}
    }
}
