using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Multitenant;
using Cmc.Framework.DocumentProvider.Interfaces;
using Cmc.Framework.DocumentProvider.Perceptive;
using Microsoft.Extensions.Logging;

namespace ViewDocument
{
    //public static class ContainerConfig
    //{
    //    public static IContainer Configure()
    //    {
    //        var builder = new ContainerBuilder();
    //        builder.Register(x => x.Resolve<ILoggerFactory>().CreateLogger<PerceptiveProcessor>()).SingleInstance();
    //        builder.RegisterType<PerceptiveProcessor>().As<IPerceptiveProcessor>().SingleInstance();
    //        builder.RegisterType<PerceptiveAdaptor>().As<IDocumentAdaptor>().SingleInstance();
    //        return builder.Build();
    //    }



    //}


    /// <summary>
    /// An Autofac module that can register the services in this project.
    /// </summary>
    public class PerceptiveRegistrationModule : Module
    {
        /// <summary>
        /// Registers services with the container builder.
        /// </summary>
        /// <param name="builder">The Autofac container builder</param>
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.Register(x => x.Resolve<ILoggerFactory>().CreateLogger<PerceptiveProcessor>()).SingleInstance();
            builder.RegisterType<PerceptiveAdaptor>().InstancePerTenant().Named<IDocumentAdaptor>("HYLPERCEP");
            builder.RegisterType<PerceptiveProcessor>().InstancePerTenant().As<IPerceptiveProcessor>();
        }
    }
}
