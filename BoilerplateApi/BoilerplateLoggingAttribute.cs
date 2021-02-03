using NLog;
using BoilerplateApi.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using System.Diagnostics;

namespace BoilerplateApi
{
    public class BoilerplateLoggingAttribute : ActionFilterAttribute
    {
    }
}
