﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace Seedworks.Lib.Persistence
{
    public interface ISessionManager : IDisposable
    {
        ISession Session { get; }
    }
}

