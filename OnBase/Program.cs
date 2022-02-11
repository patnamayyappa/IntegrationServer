using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Multitenant;
using Cmc.Framework.Caching;
using Cmc.Framework.Caching.DictionaryCache;
using Cmc.Framework.DocumentProvider.Interfaces;
using Cmc.Framework.DocumentProvider.Messages;
using Cmc.Framework.DocumentProvider.OnBase;
using Cmc.Framework.DocumentProvider.OnBase.DerivedOptions;
using Cmc.Framework.DocumentProvider.OnBase.Services;
using Cmc.Framework.HttpClients.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace OnBase
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<LoggerFactory>()
                .As<ILoggerFactory>()
                .SingleInstance();
            builder.RegisterGeneric(typeof(Logger<>))
                .As(typeof(ILogger<>))
                .SingleInstance();

            builder.Register(x => x.Resolve<ILoggerFactory>().CreateLogger<OnBaseProcessor>()).SingleInstance();
            builder.Register(x => x.Resolve<ILoggerFactory>().CreateLogger<OnBaseDocTypeProcessor>()).SingleInstance();
            builder.Register(x => x.Resolve<ILoggerFactory>().CreateLogger<DisconnectSession>()).SingleInstance();
            builder.Register(x => x.Resolve<ILoggerFactory>().CreateLogger<OAuthTokenService>()).SingleInstance();

            builder.Register(x =>
            {
                var cache = new DictionaryCache(null);
                return cache;
            }).InstancePerLifetimeScope().As<ICache>();

            builder.Register(x =>
            {
                var config = x.ResolveOptional<IConfiguration>();
                var onBaseOpts = new OnBaseOptions();
                onBaseOpts.Url = "http://10.3.136.143";
                onBaseOpts.Scope = "evolution";
                onBaseOpts.ApiServerProductUrl = "apiserver/onbase/core";
                onBaseOpts.OnBaseLicense = "QueryMetering";
                var disconnectSessionVal = "false";
                bool isDisconnectSession = true;
                bool.TryParse(disconnectSessionVal, out isDisconnectSession);
                onBaseOpts.DisconnectSession = isDisconnectSession;
                return Options.Create(onBaseOpts);
            }).InstancePerLifetimeScope().As<IOptions<OnBaseOptions>>();

            builder.Register(x =>
            {
                var config = x.ResolveOptional<IConfiguration>();
                var onBaseTokenOpts = new OnBaseTokenOptions();
                onBaseTokenOpts.Authority = "http://10.3.136.143";
                var cache = "true";
                bool isCache = false;
                bool.TryParse(cache, out isCache);
                onBaseTokenOpts.CacheCookies = isCache;
                onBaseTokenOpts.ClientId = "onbase";
                onBaseTokenOpts.ClientSecret = "secret";
                onBaseTokenOpts.UserName = "manager";
                onBaseTokenOpts.Password = "password";
                var retryAttemptsRaw = "0";
                int retryAttempts = 0;
                int.TryParse(retryAttemptsRaw, out retryAttempts);
                onBaseTokenOpts.RetryAttempts = retryAttempts;
                onBaseTokenOpts.TokenUri = "/identityprovider/connect/token";

                return Options.Create(onBaseTokenOpts);
            }).InstancePerLifetimeScope().As<IOptions<OnBaseTokenOptions>>();

            builder.RegisterType<OnBaseAdaptor>().InstancePerLifetimeScope().As<IDocumentAdaptor>();
            builder.RegisterType<OnBaseProcessor>().InstancePerLifetimeScope().As<IOnBaseProcessor>();
            builder.RegisterType<OnBaseDocTypeProcessor>().InstancePerLifetimeScope().As<IOnBaseDocTypeProcessor>();
            builder.RegisterType<OnBaseTokenService>().InstancePerLifetimeScope().As<IOnBaseTokenService>();
            builder.RegisterType<DisconnectSession>().InstancePerLifetimeScope().As<IDisconnectSession>();
            var container = builder.Build();

            var documentAdaptor = container.Resolve<IDocumentAdaptor>();
            var baseProcessor = container.Resolve<IOnBaseProcessor>();
            var baseOptions = container.Resolve<IOptions<OnBaseOptions>>();
            var options = container.Resolve<IOptions<OnBaseTokenOptions>>();

            //documentAdaptor.GetOrRedirectToProviderUrl(new GetOrRedirectToProviderUrlRequest()
            //{
            //    AutoRedirect = true,
            //    DocRequestType = DocRequestType.Upload,
            //    DocumentDetails = new DocumentDetails() { DocumentType = "Type" },
            //    DocumentRequest = new DocumentRequest() { ProviderInfo = new DocumentProviderInfo() { BaseUrl = baseOptions.Value.Url, CaptureProfile = baseOptions.Value.Scope } },
            //    ProviderInfo = new DocumentProviderInfo()

            //});

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
            var byteArrayContent = new ByteArrayContent(fileContents);
            var documentResponse = documentAdaptor.GetDoctype("BC");
            var documentClientRequest = new DocumentClientRequest()
            {
                DocumentInfo = new DocumentInfo()
                {
                    //CmDocumentID = "216",
                    //ExternalDocumentID = "208",
                    //AwardYear = args.Request.AwardYear,
                    FileBytesArray = fileContents,
                    FileBytes = Convert.ToBase64String(fileContents),
                    FileNameWithExtension = "Perceptive_Integration_Server_on_Tomcat_Installation_Guide_7.4.x.pdf",
                    Filesize = long.Parse(length.ToString()),
                    FileTypeID = 16,
                    //ProgramVersionCode = "ProgramVersionName",
                    //VendorCode = "VendorCode",
                    DocumentType = new DocumentTypeInfo
                    {
                        DoctypeCode = "117",
                        DoctypeDescrip = "TestPB",
                        ExternalDocTypeID = "117",
                        Module = new ModuleInfo()
                        {
                            ModuleID = "191",
                            ModuleCode = "TestDocumentPB",
                            ModuleDescrip = "TestDocumentPB",
                        }
                    }
                },
                StudentInfo = new StudentDetailInfo
                {
                    //CampusCode = "CampusCode",
                    //CampusID = "CampusId",
                    //CrmID = "crmIdentifier",
                    EmailAddress = "EmailAddress",
                    FirstName = "StudentFirstName",
                    LastName = "StudentLastName",
                    //ProgramCode = "ProgramCode",
                    //ProgramDescrip = "ProgramName",
                    //ProgramVersionCode = "ProgramVersionName",
                    //ProgramVersionDescrip = "ProgramVersionName",
                    //StartDate = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                    //StudentID = Convert.ToString("StudentId"),
                    //StudentNumber = "StudentNumber"
                },
                //DocRequestType = new DocRequestType()
                //{

                //},
                //ProviderInfo = new DocumentProviderInfo
                //{

                //},
                ApiProviderInfo = new DocumentApiProviderInfo()
                {
                    SchoolDocsDatabaseName = "",
                    ApiServerProductUrl = "",
                    SchoolDocsUrlClientID = "",
                    OnbasekeywordTypes = new OnbaseKeywordTypes
                    {
                        items = new OnbaseKeywordTypesItems()
                        {
                            typeGroupId = onbaseTypeGroupId,
                            onbasekeywordconfig = onBaseKeyWordConfigList
                        }
                    }
                }

            };

            var response = documentAdaptor.PostDocument(documentClientRequest);
            if (response != null && response.DocumentProviderException != null)
            {

            }


        }

        private void UploadDocument()
        {


        }

    }
}
