using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Cmc.Framework.DocumentProvider.Interfaces;
using Cmc.Framework.DocumentProvider.Messages;
using Cmc.Framework.DocumentProvider.Perceptive;
using Microsoft.Extensions.Logging;

namespace ViewDocument
{
    public class Program
    {
        static void Main(string[] args)
        {
            //var container = ContainerConfig.Configure();

            //var builder = new ContainerBuilder();
            //builder.Register(x => x.Resolve<ILoggerFactory>().CreateLogger<PerceptiveProcessor>()).SingleInstance();
            //builder.RegisterType<PerceptiveProcessor>().As<IPerceptiveProcessor>().SingleInstance();
            //builder.RegisterType<PerceptiveAdaptor>().As<IDocumentAdaptor>().SingleInstance();
            //var container = builder.Build();
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            var result = "https://cltpocfe1.campusmgmt.com:8443/Experience/#documents/view/321Z2CF_00007EHH000001J/document/321Z563_002WMS60J00001L";
            using (var client = new WebClient())
            using (var stream = client.OpenRead(result))
            using (var textReader = new StreamReader(stream, Encoding.UTF8, true))
            {
                var ss1= textReader.ReadToEnd();
            }

            using (var client = new HttpClient())
            {

                var plainTextBytes = Encoding.UTF8.GetBytes($"patnama:B19lp@yment");
                string encodedCredential = Convert.ToBase64String(plainTextBytes);
                // Create authorization header
                string authorizationHeader = "Basic " + encodedCredential;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 |
                                                       SecurityProtocolType.Tls;
                ServicePointManager.ServerCertificateValidationCallback +=
                    (sender, certificate, chain, sslPolicyErrors) => { return true; };


                client.DefaultRequestHeaders.Add("Authorization", authorizationHeader);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");

                #region commented

                //client.DefaultRequestHeaders.Accept = new HttpHeaderValueCollection<MediaTypeWithQualityHeaderValue>()
                //client.DefaultRequestHeaders.Add("X-IntegrationServer-Username", "patnama@campusmgmt.com");
                //client.DefaultRequestHeaders.Add("X-IntegrationServer-Password", "B17lp@yment"); 

                #endregion

                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri("https://cltpocfe1.campusmgmt.com:8443/integrationserver/connection"),
                    Method = HttpMethod.Get,
                };

                var response = client.SendAsync(request).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    var responseContent =
                        response.Headers.GetValues("X-IntegrationServer-Session-Hash").FirstOrDefault();
                    var sessionId = responseContent;

                    // clear and add the session header to the upload request
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.TryAddWithoutValidation("X-IntegrationServer-Session-Hash", sessionId);

                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type",
                        "application/json; charset=utf-8");
                    var request1 = new HttpRequestMessage()
                    {
                        RequestUri = new Uri(result),
                        Method = HttpMethod.Get,
                    };

                    var response1 = client.SendAsync(request1).GetAwaiter().GetResult();

                    if (response1.IsSuccessStatusCode)
                    {
                        var responseContent1 =
                            response1.Content.ReadAsStringAsync();
                    }
                }
            }

            var cln = new WebClient();
            cln.DownloadFile(new Uri(result), "filename");

            var ss = "hj";

            //using (var scope = container.BeginLifetimeScope())
            //{


            //    var getOrRedirectToProviderUrlRequest = new GetOrRedirectToProviderUrlRequest()
            //    {
            //        AutoRedirect = true,
            //        DocRequestType = DocRequestType.View,
            //        DocumentDetails = new DocumentDetails
            //        {
            //            CmcDocumentId = 12587,
            //            DocumentName = "Sample",
            //            DocumentType = "Sample001",
            //            Module = "Engage",
            //            ProviderDocumentId = string.Empty,
            //            StudentId = 12587,
            //            StudentName = "Ayyappa",
            //            StudentNumber = "Student001"
            //        },
            //        ProviderInfo = new DocumentProviderInfo
            //        {
            //            BaseUrl = "https://cltpocfe1.campusmgmt.com:8443/Experience",
            //            CaptureProfile = "Web Client Capture",
            //            CustomerFolder = "321Z2CF_00007EHH000001J",
            //            Username = "",
            //            Password = ""
            //        }

            //    };
            //    var documentAdaptor = scope.Resolve<IDocumentAdaptor>();
            //    var response = documentAdaptor.GetOrRedirectToProviderUrl(getOrRedirectToProviderUrlRequest);
            //}
        }
    }
}
