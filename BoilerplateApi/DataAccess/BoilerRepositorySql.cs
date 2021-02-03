using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoilerplateApi.DataAccess.DTO;
using SqlKata;
using SqlKata.Compilers;
using Dapper;

namespace BoilerplateApi.DataAccess
{
    public class BoilerRepositorySql : BoilerRepository
    {
        private readonly Compiler _compiler;
        private readonly DbContext _dbContext;
        public BoilerRepositorySql(Compiler compiler, DbContext dbContext)
        {
            _compiler = compiler;
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Boiler>> GetAll()
        {
            var query = _compiler.Compile(
                new Query("FrontWidgets").Select("Id", "Name", "ContractJSON")
            );

            using (var _connection = _dbContext.Connection())
            {
                var result = await _connection.QueryAsync<Boiler>(query.Sql, query.NamedBindings);
                return result;
            }
        }
    }
}
