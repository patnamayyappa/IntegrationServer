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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IntegrationServer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //DeleteDocument();
            //UploadDocument();
        }


        public static void DeleteDocument()
        {

            var url =
                "https://cltpocfe1.dev.campusmgmt.com:8443/integrationserver/v1/document/321Z58V_002ZHC6VN0000DR/page/321Z58V_002ZHC6VN0000DW";


            var uri = new Uri(url);
            var regex = new Regex(".*document/(.*)/page.*");
            if (regex.IsMatch(uri.AbsoluteUri))
            {
                var myCapturedText = regex.Match(uri.AbsoluteUri).Groups[1].Value;
                Console.WriteLine("This is my captured text: {0}", myCapturedText);
            }

            var plainTextBytes = Encoding.UTF8.GetBytes($"patnama:B19lp@yment");
            string encodedCredential = Convert.ToBase64String(plainTextBytes);
            // Create authorization header
            string authorizationHeader = "Basic " + encodedCredential;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) =>
            {
                return true;
            };
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", authorizationHeader);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri($"{url}//document/321Z58V_002ZHC6VN0000DR"),
                    Method = HttpMethod.Delete,
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


                }
            }
        }


        public void UpdateDocument()
        {
            var plainTextBytes = Encoding.UTF8.GetBytes($"patnama:B19lp@yment");
            string encodedCredential = Convert.ToBase64String(plainTextBytes);
            // Create authorization header
            string authorizationHeader = "Basic " + encodedCredential;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) =>
            {
                return true;
            };
            var url = "https://cltpocfe1.campusmgmt.com:8443/integrationserver";
            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Add("Authorization", authorizationHeader);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");

                #region commented

                #endregion

                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri($"{url}//connection"),
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


                }

            }
        }

        public class strincontent
        {
            public string name { get; set; }
        }
        public static void UploadDocument()
        {
            //var documentRequest1 = new DocumentRequest()
            //{
            //    info = new Info()
            //    {
            //        name = string.Empty,
            //        locationid = "locationId",
            //        keys = new Keys()
            //        {
            //            drawer = "drawer",
            //            documentType = "documenttype",
            //        },
            //        version = 1,
            //    },
            //    properties = new List<Property>(),
            //    pages = new List<Page>(),
            //    workflowItems = new List<object>()
            //};



            //var propertieType = documentRequest1.info.keys?.GetType();
            //var order = 1;
            //propertieType?.GetProperty($"field{order}")?.SetValue(documentRequest1.info.keys, "customattributeValue");


            var handler = new HttpClientHandler();
            handler.SslProtocols = SslProtocols.Tls12;
            //handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            var cert = new X509Certificate2(@"C:\Users\PatnamAyyappa\OneDrive - Anthology Inc\Desktop\Patnama.cer");
            handler.ClientCertificates.Add(cert);

            // Encode with base64
            //var plainTextBytes = Encoding.UTF8.GetBytes($"ContentAdmin:Qaad!234");
            var plainTextBytes = Encoding.UTF8.GetBytes($"patnama:B20lp@yment");
            string encodedCredential = Convert.ToBase64String(plainTextBytes);
            // Create authorization header
            string authorizationHeader = "Basic " + encodedCredential;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) =>
            {
                return true;
            };
                //var url = "https://cltpocfe1.campusmgmt.com:8443/integrationserver";
            var url = "https://cltpocfe2.campusmgmt.com:8443/integrationserver";
            using (var client = new HttpClient())
            {

                //client.DefaultRequestHeaders.Add("Authorization", authorizationHeader);
                client.DefaultRequestHeaders.Add("REMOTE_USER", "payyappa");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
                #region commented
                //client.DefaultRequestHeaders.Accept = new HttpHeaderValueCollection<MediaTypeWithQualityHeaderValue>()
                //client.DefaultRequestHeaders.Add("X-IntegrationServer-Username", "patnama@campusmgmt.com");
                //client.DefaultRequestHeaders.Add("X-IntegrationServer-Password", "B17lp@yment"); 
                #endregion
                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri($"{url}/connection"),
                    Method = HttpMethod.Get,
                };

                var response = client.SendAsync(request).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Headers.GetValues("X-IntegrationServer-Session-Hash").FirstOrDefault();
                    var sessionId = responseContent;

                    // clear and add the session header to the upload request
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.TryAddWithoutValidation("X-IntegrationServer-Session-Hash", sessionId);
                    client.DefaultRequestHeaders.Add("REMOTE_USER", "payyappa");

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

                    request = new HttpRequestMessage()
                    {
                        RequestUri = new Uri($"{url}/property"),
                        Method = HttpMethod.Get,
                    };

                    response = client.SendAsync(request).GetAwaiter().GetResult();


                    var propertyString = response.Content.ReadAsStringAsync().Result;
                    var ss = JsonConvert.DeserializeObject<CustomProperties>(propertyString);

                    response = client.GetAsync(new Uri($"{url}/document/321Z5DQ_003GRM0PM00046R/page")).GetAwaiter().GetResult();
                    string documentResponse = string.Empty;
                    if (response.IsSuccessStatusCode)
                    {
                        documentResponse = response.Content.ReadAsStringAsync().Result;
                        var ssl =JsonConvert.DeserializeObject<Pages>(documentResponse);
                    }

                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");


                    //client.DefaultRequestHeaders
                    //    .Accept
                    //    .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
                    var stringcontent = JsonConvert.SerializeObject(new strincontent() { name = "ayyappa" });

                    //http client data persists so first we clear headers
                    client.DefaultRequestHeaders.Clear();

                    //API will default to returning XML if we do not 
                    //specify the Accept header
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //Retrieve the access token from memory and 
                    //add it to the request as a custom header
                    client.DefaultRequestHeaders.Add("X-IntegrationServer-Session-Hash", sessionId);

                    //This line adds the post data and sets the content-type 
                    //header to application/json
                    var fileName = "naku oddu ee godava.";
                    var data = new StringContent("{\"name\":\"" + fileName + "\"}", Encoding.UTF8, "application/json");
                    response = client.PutAsync(new Uri($"{url}//document/321Z59H_002ZH66VN0000F7/name?action=APPEND"), data).GetAwaiter().GetResult();
                    
                    if (response.IsSuccessStatusCode)
                    {
                        documentResponse = response.Content.ReadAsStringAsync().Result;
                    }

                    var documentRequest = new DocumentRequest()
                    {
                        info = new Info()
                        {
                            name = ("AyyappaDocument112" + DateTime.Now.Ticks),
                            locationid = "321Z569_002WMT60J0000DP",
                            keys = new Keys()
                            {
                                drawer = "Application Management",
                                documentType = "Application",
                                field1 = "1234567",
                                field2 = "Ayyappa",
                                field3 = "1234567890",
                                field4 = DateTime.Now.ToString(CultureInfo.CurrentCulture),
                                field5 = "9a26384c-23cd-eb11-bacc-000d3a5a250e",
                            },
                            version = 1,
                        },
                        properties = new List<Property>(){new Property(){   id = "Application period",
                            type = "STRING",
                            value = "customattributeValue"
                        }},
                        pages = new List<Page>(),
                        workflowItems = new List<object>()
                    };


                    //var documentRequest = JsonConvert.DeserializeObject<DocumentRequest>(
                    //    @"{'info':{'id':null,'name':'eStatement4.pdf','keys':{'drawer':'Application Management','field1':null,'field2':null,'field3':null,'field4':null,'field5':null,'documentType':'Application'},'version':1,'locationid':'321Z2CF_00007EHH000001J'},'workflowItems':[],'pages':[],'properties':[{'id':'321Z56M_002X6LH4100003G','type':'STRING','value':'Test Period','childProperties':null},{'id':'321Z56E_002X1SNHP000007','type':'STRING','value':'ddf9d295-04d2-eb11-bacc-000d3a33e733','childProperties':null}]}");
                    var content = new StringContent(@"{'info':{'id':null,'name':'imaging-US State list and Abbrevations.xlsx-637660962186040817','keys':{'drawer':'Application Management','field1':null,'field2':null,'field3':null,'field4':null,'field5':null,'documentType':'Application'},'version':1,'locationid':'321Z2CF_00007EHH000001J'},'workflowItems':[],'pages':[],'properties':[{'id':'1234567','type':'string','value':'276API_Firstname823 337API_Lastname755-131-program-appreview900 ','childProperties':null},{'id':'1234567','type':'Optionset','value':'175490001','childProperties':null},{'id':'123456','type':'string','value':'276API_Firstname823 337API_Lastname755','childProperties':null},{'id':'321Z56E_002X1SNHP000007','type':'STRING','value':'19fbc15c-200b-ec11-b6e5-000d3a9aadf8','childProperties':null}]}");
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

                    //var content = new StringContent(JsonConvert.SerializeObject(documentRequest), Encoding.UTF8, "application/json");
                    response = client.PostAsync(new Uri($"{url}//document"), content).GetAwaiter().GetResult();

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

                        var result = client.PostAsync(fileUploadUrl, byteArrayContent).GetAwaiter().GetResult();
                        var filelocation = result.Headers.GetValues("Location").FirstOrDefault();
                        var respmsg = result.Content.ReadAsStringAsync();


                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.TryAddWithoutValidation("X-IntegrationServer-Session-Hash", sessionId);
                        client.DefaultRequestHeaders.TryAddWithoutValidation("ContentType", "application/octet-stream");
                        response = client.GetAsync($"{filelocation}/file").GetAwaiter().GetResult();
                        if (response.IsSuccessStatusCode)
                        {
                            var byten = result.Content.ReadAsByteArrayAsync().Result;
                        }
                    }

                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.TryAddWithoutValidation("X-IntegrationServer-Session-Hash", sessionId);
                    var deleteResult = client.DeleteAsync(new Uri($"{url}//connection"));
                }



            }

        }



    }
}
