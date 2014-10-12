using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using AutofacContrib.SolrNet;
using AutofacContrib.SolrNet.Config;
using HibernatingRhinos.Profiler.Appender.NHibernate;
using RollbarSharp;
using TrueOrFalse;
using TrueOrFalse.Infrastructure;
using TrueOrFalse.Search;
using TrueOrFalse.View;
using TrueOrFalse.Web.Context;
using TrueOrFalse.Web.JavascriptView;
using TrueOrFalse.Updates;

namespace TrueOrFalse.Frontend.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class Global : HttpApplication
    {        
        protected void Application_Start()
        {
            InitializeAutofac();
#if DEBUG
            NHibernateProfiler.Initialize();
#endif
            
            Sl.Resolve<Update>().Run();

            AreaRegistration.RegisterAllAreas();

            BundleConfig.RegisterBundles(BundleTable.Bundles); 
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new JavaScriptViewEngine());
            ViewEngines.Engines.Add(new PartialSubDirectoriesViewEngine());
        }


        private void Application_BeginRequest()
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("de-DE");
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("de-DE");            
        }

        private void InitializeAutofac()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterModelBinders(Assembly.GetExecutingAssembly());
            builder.RegisterModule<AutofacCoreModule>();

            var solrUrl = WebConfigSettings.SolrUrl;
            var solrSuffix = WebConfigSettings.SolrCoresSuffix;

            var cores = new SolrServers {
                                new SolrServerElement {
                                        Id = "question",
                                        DocumentType = typeof (QuestionSolrMap).AssemblyQualifiedName,
                                        Url = solrUrl + "tofQuestion" + solrSuffix
                                    },
                                new SolrServerElement {   
                                        Id = "set",
                                        DocumentType = typeof (SetSolrMap).AssemblyQualifiedName,
                                        Url = solrUrl + "tofSet" + solrSuffix
                                    },
                                new SolrServerElement {   
                                        Id = "category",
                                        DocumentType = typeof (CategorySolrMap).AssemblyQualifiedName,
                                        Url = solrUrl + "tofCategory" + solrSuffix
                                    },
                                new SolrServerElement {   
                                        Id = "users",
                                        DocumentType = typeof (UserSolrMap).AssemblyQualifiedName,
                                        Url = solrUrl + "tofUser" + solrSuffix
                                    }
                            };

            builder.RegisterModule(new SolrNetModule(cores));

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        protected void Session_Start()
        {
            if(!Sl.Resolve<SessionUser>().IsLoggedIn)
                Sl.Resolve<LoginFromCookie>().Run();
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            if (exception == null)
                return;

            var code = (exception is HttpException) ? (exception as HttpException).GetHttpCode() : 500;

            if (code != 404)
            {
                try
                {
                    (new RollbarClient()).SendException(exception);
                }
                catch{}
            }

            Response.Clear();
            Server.ClearError();

            Response.Redirect(string.Format("~/Fehler/{0}", code), false);
        }
    }
}