using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                password
                )
        { }

        #region CRUD

        public Contact Get(int ContactId)
        {
            var request = new RestRequest(
                "contacts/{id}", 
                Method.GET
                )
            {
                RequestFormat = DataFormat.Json
            };

            request.AddUrlSegment("id", ContactId.ToString());

            var results = Execute<Contact>(request);

            return results.Data;

        }

        public IList<Contact> List(
                int from, int take
            )
        {
            var request = new RestRequest(
                "contacts",
                Method.GET
                )
            {
                RequestFormat = DataFormat.Json
            };

            request.AddQueryParameter( "from", from.ToString() );
            request.AddQueryParameter( "take", take.ToString() );

            var results = Execute<List<Contact>>(request);
            return results.Data;
        }

        #endregion

    }
}
