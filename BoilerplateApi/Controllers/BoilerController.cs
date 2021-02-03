using NLog;
using BoilerplateApi.DataAccess;
using BoilerplateApi.DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace BoilerplateApi.Controllers
{
    public class BoilerController : ApiController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly BoilerRepository _repository;

        public BoilerController(BoilerRepository repository)
        {
            _repository = repository;
        }

        [BoilerplateAuthorize]
        [HttpGet]
        public async Task<IHttpActionResult> GetBoilers()
        {
            try
            {
                IEnumerable<Boiler> data = await _repository.GetAll();
                return Ok(new { data = data });
            }
            catch (Exception e)
            {
                Logger.Error(e, e.Message);
                return InternalServerError(e);
            }
        }
    }
}
