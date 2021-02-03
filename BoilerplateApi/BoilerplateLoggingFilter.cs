using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace BoilerplateApi
{
    class BoilerplateLoggingFilter : IActionFilter
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public bool AllowMultiple
        {
            get
            {
                return true;
            }
        }

        public Task<HttpResponseMessage> ExecuteActionFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            var logAttribute = actionContext
                .ActionDescriptor
                .GetCustomAttributes<BoilerplateLoggingAttribute>()
                .SingleOrDefault();

            if (logAttribute == null)
            {
                return continuation();
            }

            actionContext.Request.Properties[actionContext.ActionDescriptor.ActionName] = Stopwatch.StartNew();

            var result = continuation();
            result.Wait();

            var request = actionContext.Request;
            var response = actionContext.Response;

            bool isAuthorized = actionContext.RequestContext.Principal.Identity.Name.ToLower().Contains("domain");
            Stopwatch watch = (Stopwatch)actionContext.Request.Properties[actionContext.ActionDescriptor.ActionName];
            Logger.Info(
                "{Username} requests {Action} with body: {Arguments}, IsAuthorized: {IsAuthorized}, UserHostName:{UserHostName}, Executed In {ExecutionTime}, IsOk: {IsOk}",
                actionContext.RequestContext.Principal.Identity.Name,
                actionContext.Request.Method.Method + " " + actionContext.Request.RequestUri.AbsolutePath,
                actionContext.Request.Content.ReadAsStringAsync().Result,
                isAuthorized,
                "",
                watch.ElapsedMilliseconds,
                actionContext.Response.StatusCode == System.Net.HttpStatusCode.OK
            );
            return result;
        }
    }
}
