using Autofac;
using Autofac.Integration.WebApi;
using BankID.Client;
using System.Configuration;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BankID.WebDemo
{
    public class MvcApplication : HttpApplication
    {
        public static IContainer Container;

        protected void Application_Start()
        {
            RegisterAutofac();

            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void RegisterAutofac()
        {
            var bankIdIsProduction = bool.Parse(ConfigurationManager.AppSettings["BankID.IsProduction"]);
            var bankIdCertificateThumbprint = ConfigurationManager.AppSettings["BankID.CertificateThumbprint"];

            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterInstance(new BankIdClient(bankIdIsProduction, bankIdCertificateThumbprint)).As<IBankIdClient>();
            
            Container = builder.Build();
        }
    }
}
