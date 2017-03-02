using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cosential.Integrations.Compass.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompassApiClientTests;

namespace Cosential.Integrations.Compass.Client.Tests
{
    [TestClass()]
    public class CompassClientTests
    {
        private readonly CompassClient _client;
        
        public CompassClientTests()
        {
            _client = new CompassClient(Credentials.FirmId, Credentials.ApiKey, Credentials.Username, Credentials.Password);
        }

        [TestMethod()]
        public void IsAuthTest()
        {
            var isAuth = _client.IsAuth();
            Assert.IsTrue(isAuth);
        }

        [TestMethod()]
        public void GetAuthenticatedUserTest()
        {
            var authUser = _client.GetAuthenticatedUser();
            Assert.IsNotNull(authUser);
        }
    }
}