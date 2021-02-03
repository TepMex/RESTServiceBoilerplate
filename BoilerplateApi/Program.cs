using BoilerplateApi.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.SelfHost;
using Swashbuckle.Application;
using Swashbuckle.SwaggerUi;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.ServiceProcess;
using System.Web.Http.Cors;
using Autofac;
using Autofac.Integration.WebApi;
using System.Reflection;
using System.Web.Http.Dependencies;

namespace BoilerplateApi
{
    class Program
    {
        public const string ServiceName = "SvoppApi";
        private static HttpSelfHostServer currentServer = null;
        private static readonly string svoppDbConnectionString = ConfigurationManager.ConnectionStrings["SvoppFrontend"].ConnectionString;
        private static readonly string valuesRepositoryConnectionString = ConfigurationManager.AppSettings["DataSourceConnectionString"];

        public class Service : ServiceBase
        {
            public Service()
            {
                ServiceName = Program.ServiceName;
            }

            protected override void OnStart(string[] args)
            {
                Program.Start(args);
            }

            protected override void OnStop()
            {
                Program.Stop();
            }
        }
        static void Main(string[] args)
        {
            if (!Environment.UserInteractive)
            {
                using (var service = new Service())
                {
                    ServiceBase.Run(service);
                }
            }
            else
            {
                // running as console app
                Start(args);

                Console.WriteLine("Press any key to stop...");
                Console.ReadKey(true);

                Stop();
            }
        }

        private static void Start(string[] args)
        {
#if DEBUG
            string bindingAddr = ConfigurationManager.AppSettings["BindingAddressTest"];
#else
            string bindingAddr = ConfigurationManager.AppSettings["BindingAddressProd"];
#endif
            var corsSettings = new EnableCorsAttribute("*", headers: "*", methods: "OPTIONS, GET, POST, PUT, DELETE");
            corsSettings.SupportsCredentials = true;


            var config = AppConfiguration(bindingAddr, corsSettings);

            var container = RegisterInjections(config);
            config.Filters.Add(new BoilerplateLoggingFilter());
            var dependencyResolver = new AutofacWebApiDependencyResolver(container);
            config.DependencyResolver = dependencyResolver;

            currentServer = new HttpSelfHostServer(config);

            var task = currentServer.OpenAsync();
        }

        private static HttpSelfHostConfiguration AppConfiguration(string bindingAddress, ICorsPolicyProvider corsSettings)
        {
            var config = new NtlmSelfHostConfiguration(bindingAddress);

            config.Formatters.JsonFormatter.SupportedMediaTypes
                .Add(new System.Net.Http.Headers.MediaTypeHeaderValue("text/html"));

            config.EnableSwagger(c => {
                c.SingleApiVersion("v1", "Boiler API");
            })
                .EnableSwaggerUi();

            config.EnableCors(corsSettings);

            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "API Default",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            return config;
        }

        private static IContainer RegisterInjections(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<SqlKata.Compilers.SqlServerCompiler>().As<SqlKata.Compilers.Compiler>().SingleInstance();
            builder.Register(ctx => new BoilerDbContextMsSql(svoppDbConnectionString)).As<DbContext>().SingleInstance();
            builder.RegisterType<BoilerRepositorySql>().As<BoilerRepository>();

            return builder.Build();
        }

        private static void Stop()
        {
            currentServer.CloseAsync().Wait();
        }
    }
}
