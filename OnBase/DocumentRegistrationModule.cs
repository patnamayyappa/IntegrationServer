using Autofac;
using Autofac.Multitenant;
using Cmc.Framework.DocumentProvider.Interfaces;
using Cmc.Framework.DocumentProvider.OnBase;
using Cmc.Framework.DocumentProvider.OnBase.DerivedOptions;
using Cmc.Framework.DocumentProvider.OnBase.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace OnBase
{
    public class DocumentRegistrationModule : Module
    {
        /// <summary>
        /// Registers services with the container builder.
        /// </summary>
        /// <param name="builder">The Autofac container builder</param>
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.Register(x => x.Resolve<ILoggerFactory>().CreateLogger<OnBaseProcessor>()).SingleInstance();
            builder.Register(x => x.Resolve<ILoggerFactory>().CreateLogger<OnBaseDocTypeProcessor>()).SingleInstance();
            builder.Register(x => x.Resolve<ILoggerFactory>().CreateLogger<DisconnectSession>()).SingleInstance();


            builder.Register(x =>
            {
                var config = x.Resolve<IConfiguration>();
                var onBaseOpts = new OnBaseOptions();
                onBaseOpts.Url = config["AppSettings:DocumentProvider:HylandApiSettings:Url"];
                onBaseOpts.Scope = config["AppSettings:DocumentProvider:HylandApiSettings:Scope"];
                onBaseOpts.ApiServerProductUrl =
                    config["AppSettings:DocumentProvider:HylandApiSettings:ApiServerProductUrl"];
                onBaseOpts.OnBaseLicense = config["AppSettings:DocumentProvider:HylandApiSettings:License"];
                var disconnectSessionVal = config["AppSettings:DocumentProvider:HylandApiSettings:DisconnectSession"];
                bool isDisconnectSession = true;
                bool.TryParse(disconnectSessionVal, out isDisconnectSession);
                onBaseOpts.DisconnectSession = isDisconnectSession;
                return Options.Create(onBaseOpts);
            }).InstancePerTenant().As<IOptions<OnBaseOptions>>();

            builder.Register(x =>
            {
                var config = x.Resolve<IConfiguration>();
                var onBaseTokenOpts = new OnBaseTokenOptions();
                onBaseTokenOpts.Authority = config["AppSettings:DocumentProvider:HylandApiSettings:Authority"];
                var cache = config["AppSettings:DocumentProvider:HylandApiSettings:CacheCookies"];
                bool isCache = false;
                bool.TryParse(cache, out isCache);
                onBaseTokenOpts.CacheCookies = isCache;
                onBaseTokenOpts.ClientId = config["AppSettings:DocumentProvider:HylandApiSettings:ClientId"];
                onBaseTokenOpts.ClientSecret = config["AppSettings:DocumentProvider:HylandApiSettings:ClientSecret"];
                onBaseTokenOpts.UserName = config["AppSettings:DocumentProvider:HylandApiSettings:UserName"];
                onBaseTokenOpts.Password = config["AppSettings:DocumentProvider:HylandApiSettings:Password"];
                var retryAttemptsRaw = config["AppSettings:DocumentProvider:HylandApiSettings:RetryAttempts"];
                int retryAttempts = 0;
                int.TryParse(retryAttemptsRaw, out retryAttempts);
                onBaseTokenOpts.RetryAttempts = retryAttempts;
                onBaseTokenOpts.TokenUri = config["AppSettings:DocumentProvider:HylandApiSettings:TokenUri"];

                return Options.Create(onBaseTokenOpts);
            }).InstancePerTenant().As<IOptions<OnBaseTokenOptions>>();

            //builder.RegisterType<OnBaseAdaptor>().InstancePerTenant().Named<IDocumentAdaptor>("HYLONBASE");
            //builder.RegisterType<OnBaseProcessor>().InstancePerTenant().As<IOnBaseProcessor>();
            builder.RegisterType<OnBaseDocTypeProcessor>().InstancePerTenant().As<IOnBaseDocTypeProcessor>();
            builder.RegisterType<OnBaseTokenService>().InstancePerTenant().As<IOnBaseTokenService>();
            builder.RegisterType<DisconnectSession>().InstancePerTenant().As<IDisconnectSession>();
        }
    }
}