using SqlKata.Compilers;
using BoilerplateApi.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dependencies;

namespace BoilerplateApi
{
    public class BoilerplateAuthorizeAttribute : AuthorizeAttribute
    {
    }
}
