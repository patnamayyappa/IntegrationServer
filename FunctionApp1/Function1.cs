using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using AzureFunctions.Autofac;
using Cmc.Framework.DocumentProvider.Interfaces;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace FunctionApp1
{
    [DependencyInjectionConfig(typeof(DIConfig))]
    public static class Function1
    {
        static Function1()
        {
            FunctionUtilities.ConfigureBindingRedirects();
            //var list = AppDomain.CurrentDomain.GetAssemblies()
            //    .Select(a => a.GetName())
            //    .OrderByDescending(a => a.Name)
            //    .ThenByDescending(a => a.Version)
            //    .Select(a => a.FullName)
            //    .ToList();
            //AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            //{
            //    var requestedAssembly = new AssemblyName(args.Name);
            //    foreach (string asmName in list)
            //    {
            //        if (asmName.StartsWith(requestedAssembly.Name + ","))
            //        {
            //            return Assembly.Load(asmName);
            //        }
            //    }
            //    return null;
            //};
        }

        [FunctionName("Function1")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequestMessage req, [Inject] IDocumentAdaptor documentAdaptor, TraceWriter log)
        {

            log.Info("C# HTTP trigger function processed a request.");

            //FunctionUtilities.ConfigureBindingRedirects();
            // parse query parameter
            string name = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "name", true) == 0)
                .Value;

            if (name == null)
            {
                // Get request body
                dynamic data = await req.Content.ReadAsAsync<object>();
                name = data?.name;
            }

            return name == null
                ? req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name on the query string or in the request body")
                : req.CreateResponse(HttpStatusCode.OK, "Hello " + name);
        }
    }
}
