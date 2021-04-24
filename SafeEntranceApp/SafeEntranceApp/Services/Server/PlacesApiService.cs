using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SafeEntranceApp.Services.Server
{
    public class PlacesApiService : BaseApiService
    {
        private const string GET_PLACE_URL = "https://registrolocales-api.azurewebsites.net/api/places/getPlace";
        private const string GET_PLACE_NAME_URL = "https://registrolocales-api.azurewebsites.net/api/places/getPlaceName";

        public async Task<string> GetPlace(string id)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(GET_PLACE_URL + "/" + id) as HttpWebRequest;
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                return await GetResponse(request);
            }
            catch(WebException ex)
            {
                return null;
            }
        }

        public async Task<string> GetPlaceName(string id)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(GET_PLACE_NAME_URL + "/" + id) as HttpWebRequest;
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                return await GetResponse(request);
            }
            catch (WebException ex)
            {
                return null;
            }
        }
    }
}
