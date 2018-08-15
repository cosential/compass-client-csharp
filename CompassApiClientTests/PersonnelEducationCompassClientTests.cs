using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cosential.Integrations.Compass.Client;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompassApiClientTests;
using Cosential.Integrations.Compass.Client.Models;
using Cosential.Integrations.Compass.Client.Contexts;

namespace Cosential.Integrations.Compass.Client.Tests
{
    [TestClass()]
    public class PersonnelEducationCompassClientTests
    {
        private readonly PersonnelContext _context;

        public PersonnelEducationCompassClientTests()
        {
            _context = new PersonnelContext(new CompassClient(Credentials.FirmId, Credentials.ApiKey, Credentials.Username, Credentials.Password));
        }

        [TestMethod()]
        public void CreateEducationRecord()
        {
            //Any valid Personnel ID to which the education would be associated
            var _personnelId = 1079654;

            var education = new PersonnelEducation
            {
                DegreeId = 0,
                Major = "Information Systems",
                School = "University of Boston",
                Year = "2017",
                SF330_DegreeInd = 0
            };

            var result = _context.AddEducationToPersonnel(_personnelId, education);
            Assert.IsNotNull(result);
        }
    }
}