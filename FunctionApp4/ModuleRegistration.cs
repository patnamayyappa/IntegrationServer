using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using AzureFunctions.Autofac.Configuration;
using Cmc.Core.DocumentProvider.Interfaces;
using Cmc.Core.DocumentProvider.Perceptive;
using Microsoft.Extensions.Logging;

namespace FunctionApp1
{
    public class DIConfig
    {
        public DIConfig(string functionName)
        {
            DependencyInjection.Initialize(builder =>
            {
                builder.Register(x => x.Resolve<ILoggerFactory>().CreateLogger<IPerceptiveProcessor>()).SingleInstance();
                //builder.RegisterType<PerceptiveProcessor>().As<IPerceptiveProcessor>(); // Naive
                builder.RegisterType<PerceptiveAdaptor>().As<IDocumentAdaptor>(); // Naive
            }, functionName);
        }
    }

}
