using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SafeEntranceApp.Services.Server
{
    public class EnvironmentVariablesService : BaseApiService
    {
        public const string SYMPTOMS_DEVELOPING_DAYS = "idbp";
        public const string INFECTIVE_PERIOD = "dapi";
        public const string TIME_TO_BE_DIRECT_CONTACT = "mfdc";

        private const string GET_VARIABLE_URL = "https://registrolocales-api.azurewebsites.net/env/getVariable/";

        public async Task<string> GetEnvironmentVariable(string name)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(GET_VARIABLE_URL + name) as HttpWebRequest;
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
