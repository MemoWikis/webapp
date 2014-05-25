using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;


class ExecAsync
{
    public static void Go(Action action)
    {
        if (HttpContext.Current == null)
            Task.Factory.StartNew(action);
        else
            HostingEnvironment.QueueBackgroundWorkItem(cancellationToken => action());
    }
}

