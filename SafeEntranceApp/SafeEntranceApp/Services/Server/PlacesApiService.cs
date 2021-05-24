using SafeEntranceApp.Common;
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
        private const string SCAN_PLACE_URL = "https://registrolocales-api.azurewebsites.net/api/places/scanPlace";
        private const string GET_PLACE_NAME_URL = "https://registrolocales-api.azurewebsites.net/api/places/getPlaceName";

        public async Task<string> GetPlace(string id)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(GET_PLACE_URL + "/" + id) as HttpWebRequest;
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                return await GetResponse(request);
            }
            catch(WebException)
            {
                return null;
            }
        }

        public async Task<string> ScanPlace(string id, bool isEntry)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(SCAN_PLACE_URL) as HttpWebRequest;
                request.Method = Constants.REST_POST;
                request.ContentType = Constants.JSON_FORMAT;

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(JsonParser.GetScanRequest(id, isEntry));
                }

                return await GetResponse(request);
            }
            catch (WebException)
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
            catch (WebException)
            {
                return null;
            }
        }
    }
}
