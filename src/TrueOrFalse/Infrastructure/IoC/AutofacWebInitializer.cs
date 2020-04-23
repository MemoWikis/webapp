using System.Reflection;
using Autofac;
using Autofac.Integration.Mvc;
using AutofacContrib.SolrNet;
using AutofacContrib.SolrNet.Config;
using SolrNet;
using SolrNet.Impl;
using TrueOrFalse.Search;

namespace TrueOrFalse.Infrastructure
{
    public static class AutofacWebInitializer
    {
        public static IContainer Run(
            bool registerForAspNet = false,
            Assembly assembly = null)
        {
            var builder = new ContainerBuilder();

            if (registerForAspNet)
            {
                builder.RegisterControllers(assembly);
                builder.RegisterModelBinders(assembly);
            }

            builder.RegisterModule<AutofacCoreModule>();

            var solrUrl = Settings.SolrUrl;
            var solrSuffix = Settings.SolrCoresSuffix;

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

            //RegisterPostSolrConnection(cores, builder);

            return builder.Build();
        }

        private static void RegisterPostSolrConnection(SolrServers cores, ContainerBuilder builder)
        {
            foreach (SolrServerElement core in cores)
            {
                var coreConnectionId = core.Id + typeof(SolrConnection);
                var solrConnection = new SolrConnection(core.Url);

                builder.RegisterType(typeof(PostSolrConnection))
                    .Named(coreConnectionId, typeof(ISolrConnection))
                    .WithParameters(new[]
                    {
                        new NamedParameter("conn", solrConnection),
                        new NamedParameter("serverUrl", core.Url)
                    });
            }
        }
    }
}