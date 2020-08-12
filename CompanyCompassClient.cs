﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using Cosential.Integrations.Compass.Client.Models;
using RestSharp;

namespace Cosential.Integrations.Compass.Client
{
    [Obsolete("Use CompanyContext instead")]
    public class CompanyCompassClient : CompassClient
    {

        public CompanyCompassClient(int firmId, Guid apiKey, string username, string password, Uri host=null) : base(firmId, apiKey, username, password, host)
        {
        }

        #region CRUD

        public Company Get(int companyId)
        {
            var request = NewRequest("companies/{id}", Method.GET);
            request.AddUrlSegment("id", companyId.ToString(CultureInfo.InvariantCulture));

            var results = Execute<Company>(request);
            return results.Data;
        }

        public List<Company> List(int from, int size, bool full=true)
        {
            var request = NewRequest("companies", Method.GET);
            request.AddQueryParameter("from", from.ToString(CultureInfo.InvariantCulture));
            request.AddQueryParameter("size", size.ToString(CultureInfo.InvariantCulture));
            request.AddQueryParameter("full", full.ToString());

            var results = Execute<List<Company>>(request);
            if (results.Data == null)
            {
                Debug.Write(results.Content);
            }
            return results.Data;
        }

        #endregion

        #region Subitems

        public List<CompanyAddress> GetAddresses(int companyId)
        {
            return GetSubItems<CompanyAddress>(PrimaryEntityType.Company, companyId, "addresses");
        }

        public List<CompanyType> GetTypes(int companyId)
        {
            return GetSubItems<CompanyType>(PrimaryEntityType.Company, companyId, "companytypes");
        }

        public List<Studio> GetStudios(int companyId)
        {
            return GetSubItems<Studio>(PrimaryEntityType.Company, companyId, "studios");
        }

        public List<PracticeArea> GetPracticeAreas(int companyId)
        {
            return GetSubItems<PracticeArea>(PrimaryEntityType.Company, companyId, "practiceareas");
        }



        #endregion

    }
}
