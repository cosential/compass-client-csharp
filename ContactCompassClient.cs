using System;
using System.Collections.Generic;
using System.Globalization;
using Cosential.Integrations.Compass.Client.Models;
using RestSharp;

namespace Cosential.Integrations.Compass.Client
{
    public class ContactCompassClient : CompassClient
    {

        public ContactCompassClient(
                int firmId, 
                Guid apiKey, 
                string username, 
                string password, 
                Uri host = null
            ) 
            : 
            base(
                firmId, 
                apiKey, 
                username, 
                password,
                host
                )
        { }

        #region CRUD

        public Contact Get(int ContactId)
        {
            var request = NewRequest(
                "contacts/{id}", 
                Method.GET
                );

            request.AddUrlSegment("id", ContactId.ToString(CultureInfo.InvariantCulture));

            var results = Execute<Contact>(request);

            return results.Data;

        }

        public IList<Contact> List(
                int from, int take, bool fullRecord=true
            )
        {
            var request = NewRequest(
                "contacts",
                Method.GET
                );

            request.AddQueryParameter("from", from.ToString(CultureInfo.InvariantCulture));
            request.AddQueryParameter("size", take.ToString(CultureInfo.InvariantCulture));
            request.AddQueryParameter("full", fullRecord.ToString());

            var results = Execute<List<Contact>>(request);
            return results.Data;
        }

        #endregion

    }
}
