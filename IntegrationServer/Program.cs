using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IntegrationServer
{
    class Program
    {
        static async Task Main(string[] args)
        {

            var handler = new HttpClientHandler();
            handler.SslProtocols = SslProtocols.Tls12;
            //handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            var cert = new X509Certificate2(@"C:\Users\PatnamAyyappa\OneDrive - Anthology Inc\Desktop\Patnama.cer");
            handler.ClientCertificates.Add(cert);

            // Encode with base64
            var plainTextBytes = Encoding.UTF8.GetBytes($"patnama:B17lp@yment");
            string encodedCredential = Convert.ToBase64String(plainTextBytes);
            // Create authorization header
            string authorizationHeader = "Basic " + encodedCredential;
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

            using (var client = new HttpClient(handler))
            {

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

                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Headers.GetValues("X-IntegrationServer-Session-Hash").FirstOrDefault();
                    var sessionId = responseContent;

                    // clear and add the session header to the upload request
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.TryAddWithoutValidation("X-IntegrationServer-Session-Hash", sessionId);

                    #region commented
                    // prepare the document request
                    //var documentRequest = new DocumentRequest()
                    //{
                    //    Info = new Info()
                    //    {
                    //        Name = "AyyappaDocument",
                    //        LocationId = "321Z2CF_00007EHH000001J",
                    //        Keys = new Keys()
                    //        {
                    //            Drawer = "AD",
                    //            DocumentType = "AD - Transcript",
                    //            Field1 = "1234567",
                    //            Field2 = "Ayyappa",
                    //            Field3 = "1234567890",
                    //            Field4 = DateTime.Now.ToString(CultureInfo.CurrentCulture),
                    //            Field5 = "SN123456789"
                    //        }
                    //    },

                    //};
                    //client.DefaultRequestHeaders.TryAddWithoutValidation("ContentType", "application/json"); 
                    #endregion

                    client.DefaultRequestHeaders
                        .Accept
                        .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
                    response = await client.GetAsync(new Uri("https://cltpocfe1.campusmgmt.com:8443/integrationserver/document/321Z2DN_00032JEW4000056"));
                    string documentResponse = string.Empty;
                    if (response.IsSuccessStatusCode)
                    {
                        documentResponse = response.Content.ReadAsStringAsync().Result;
                    }

                    var documentRequest = new DocumentRequest()
                    {
                        info = new Info()
                        {
                            name = "AyyappaDocument5",
                            locationid = "321Z2CF_00007EHH000001J",
                            keys = new Keys()
                            {
                                drawer = "Application Management",
                                documentType = "Application",
                                field1 = "1234567",
                                field2 = "Ayyappa",
                                field3 = "1234567890",
                                field4 = DateTime.Now.ToString(CultureInfo.CurrentCulture),
                                field5 = "SN123456789",
                            },
                            version = 1,
                        },
                        properties = new List<object>(),
                        pages = new List<Page>(),
                        workflowItems = new List<object>()
                    };

                    #region commented
                    //var documentRequest = new JObject();
                    //documentRequest.Add("info",
                    //                        new JObject(
                    //                                new JProperty("name", "AyyappaDocument"),
                    //                                new JProperty("locationid", "321Z2CF_00007EHH000001J"),
                    //                                new JProperty("keys",
                    //                                                new JObject(
                    //                                                    new JProperty("drawer", "Application Management"),
                    //                                                    new JProperty("documentType", "Application"),
                    //                                                    new JProperty("field1", "1234567"),
                    //                                                    new JProperty("field2", "Ayyappa")
                    //                                                    )
                    //                                    ))); 
                    //client.DefaultRequestHeaders.TryAddWithoutValidation("ContentType", "application/json");
                    //client.DefaultRequestHeaders
                    //    .Accept
                    //    .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
                    #endregion

                    var content = new StringContent(JsonConvert.SerializeObject(documentRequest), Encoding.UTF8, "application/json");
                    response = await client.PostAsync(new Uri("https://cltpocfe1.campusmgmt.com:8443/integrationserver/document"), content);

                    if (response.IsSuccessStatusCode)
                    {
                        string fileUploadUrl = $"{response.Headers.GetValues("Location").FirstOrDefault()}/page";
                        var filePath = @"C:\Users\PatnamAyyappa\Downloads\Perceptive_Integration_Server_on_Tomcat_Installation_Guide_7.4.x (1).pdf";
                        byte[] fileContents = null;
                        long length = 0;
                        using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                        {
                            length = fs.Length;
                            fileContents = new byte[fs.Length];
                            fs.Read(fileContents, 0, (int)fs.Length);
                        }
                        //byte[] fileContents = Encoding.Default.GetBytes(buffer);
                        ByteArrayContent byteArrayContent = new ByteArrayContent(fileContents);
                        byteArrayContent.Headers.TryAddWithoutValidation("X-IntegrationServer-Resource-Name", "page1");
                        byteArrayContent.Headers.TryAddWithoutValidation("X-IntegrationServer-File-Size", length.ToString());
                        byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        //byteArrayContent.Headers.Add("Content", "application/octet-stream");

                        var result = await client.PostAsync(fileUploadUrl, byteArrayContent);
                        var respmsg = result.Content.ReadAsStringAsync();

                    }
                    else
                    {

                    }
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.TryAddWithoutValidation("X-IntegrationServer-Session-Hash", sessionId);
                    var deleteResult = client.DeleteAsync(new Uri("https://cltpocfe1.campusmgmt.com:8443/integrationserver/connection"));
                }



            }

        }
    }
}
