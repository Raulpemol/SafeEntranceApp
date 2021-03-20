using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SafeEntranceApp.Services.Server
{
    class PlacesApiService
    {
        private const string GET_PLACE_URL = "https://registrolocales-api.azurewebsites.net/api/places/getPlace";

        public async Task<string> GetPlace(string id)
        {
            HttpWebRequest request = WebRequest.Create(GET_PLACE_URL + "/" + id) as HttpWebRequest;
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse;
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);

            if(response.StatusCode.Equals(HttpStatusCode.OK) || response.StatusCode.Equals(HttpStatusCode.Created))
            {
                return await reader.ReadToEndAsync();
            }
            else
            {
                return null;
            }
        }
    }
}
