using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SafeEntranceApp.Services.Server
{
    class EnvironmentVariablesService
    {
        private const string DAYS_BEFORE_PCR_ENV_URL = "https://registrolocales-api.azurewebsites.net/env/idbp";

        public async Task<string> GetDaysBeforePCR()
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(DAYS_BEFORE_PCR_ENV_URL) as HttpWebRequest;
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                return await GetResponse(request);
            }
            catch (WebException ex)
            {
                return null;
            }
        }

        private async Task<string> GetResponse(HttpWebRequest request)
        {
            HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse;
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);

            if (response.StatusCode.Equals(HttpStatusCode.OK))
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
