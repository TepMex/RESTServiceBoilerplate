using BoilerplateApi.DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoilerplateApi.DataAccess
{
    public interface AuthRepository
    {
        IEnumerable<User> GetAllUsers();
    }
}
