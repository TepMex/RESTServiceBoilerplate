using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoilerplateApi.DataAccess
{
    public class BoilerDbContextMsSql : DbContext
    {
        private readonly string _connectionString;

        public BoilerDbContextMsSql(string connectionString)
        {
            _connectionString = connectionString;
        }
        public IDbConnection Connection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
