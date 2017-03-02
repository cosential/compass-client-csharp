using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cosential.Integrations.Compass.Client;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompassApiClientTests;
using Cosential.Integrations.Compass.Client.Models;

namespace Cosential.Integrations.Compass.Client.Tests
{
    [TestClass()]
    public class PersonnelCompassClientTests
    {
        private readonly PersonnelCompassClient _client;
        private readonly List<string> _idBag;

        public PersonnelCompassClientTests()
        {
            _client = new PersonnelCompassClient(Credentials.FirmId, Credentials.ApiKey, Credentials.Username, Credentials.Password);
            _idBag = new List<string> { "PersonnelCompassClientTest1", "PersonnelCompassClientTest2", "PersonnelCompassClientTest3" };
        }

        [TestMethod()]
        public void CreateSingleTest()
        {
            var personnel = new Personnel
            {
                FirstName = "Jeff",
                LastName = "Johnstone",
                ExternalId = _idBag[0]
            };

            var result = _client.Create(personnel);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.FirstName, personnel.FirstName);
            Assert.AreEqual(result.LastName, personnel.LastName);
            Assert.AreEqual(personnel.ExternalId, personnel.ExternalId);
        }

        [TestMethod()]
        public void CreateMultipleTest()
        {
            var response = _client.Create(new List<Personnel>()
            {
                new Personnel() {FirstName="John", LastName = "Smith", ExternalId = _idBag[1]},
                new Personnel() {FirstName="Jack", LastName = "Johnson", ExternalId = _idBag[2]}
            });

            Assert.IsNotNull(response);
            Assert.AreEqual(response.Count, 2);
        }

        [TestMethod()]
        public void SearchTest()
        {
            var query = string.Join(" OR ", _idBag.Select((id) => $"ExternalId.raw:{id}"));
            var results = _client.Search(query);

            Assert.IsNotNull(results);
            Assert.AreNotEqual(results.Count, 0);
        }

        [TestMethod()]
        public void DeleteTest()
        {
            var query = string.Join(" OR ", _idBag.Select((id) => $"ExternalId.raw:{id}"));
            var results = _client.Search(query);

            Assert.AreNotEqual(results.Count, 0);

            foreach (var personnel in results)
            {
                if (personnel.PersonnelId.HasValue)
                    _client.Delete(personnel.PersonnelId.Value);
            }

            var newResults = _client.Search(query);

            Assert.AreEqual(newResults.Count, 0);
        }
    }
}