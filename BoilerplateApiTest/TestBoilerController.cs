using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Web.Http;
using Autofac.Integration.WebApi;
using Autofac;
using System.Data.SqlClient;
using BoilerplateApi.DataAccess;
using Dapper;
using BoilerplateApi.Controllers;
using BoilerplateApi.DataAccess.DTO;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Http.Results;
using System.IO;
using System.Collections.Generic;

namespace SvoppApiTest
{
    [TestClass]
    public class TestCellController
    {
        [TestMethod]
        public void TestBoilerplate()
        {
            Assert.AreEqual(2, 2);
        }

    }
}
