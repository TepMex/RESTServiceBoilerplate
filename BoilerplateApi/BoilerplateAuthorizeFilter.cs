using BoilerplateApi.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace BoilerplateApi
{
    class BoilerplateAuthorizeFilter : IAuthorizationFilter
    {
        private readonly AuthRepository _repository;
        public BoilerplateAuthorizeFilter(AuthRepository repository)
        {
            _repository = repository;
        }

        public Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            var authAttribute = actionContext
                .ActionDescriptor
                .GetCustomAttributes<BoilerplateAuthorizeAttribute>()
                .SingleOrDefault();

            if (authAttribute == null)
            {
                return continuation();
            }

            IPrincipal principal = actionContext.RequestContext.Principal;
            if (principal.Identity.AuthenticationType != "NTLM" || !IsAuthorized(principal.Identity.Name.ToLower()))
            {
                return Task.FromResult(
                       actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized));
            }
            else
            {
                return continuation();
            }
        }
        protected bool IsAuthorized(string userName)
        {
            return IsUserInDomain(userName) && IsUserRegistered(userName);
        }

        private bool IsUserInDomain(string userName)
        {
            return userName.Contains("domain");
        }

        private bool IsUserRegistered(string userName)
        {
            string[] userDomainAndName = userName.Split('\\');
            if (userDomainAndName.Length > 1)
            {
                return IsUserNameFoundInDb(userDomainAndName[1]);
            }
            return false;
        }

        private bool IsUserNameFoundInDb(string userName)
        {
            var result = _repository.GetAllUsers().Any(user => user.Username.ToLower() == userName.ToLower());
            return result;
        }
        public bool AllowMultiple
        {
            get
            {
                return true;
            }
        }

    }
}
