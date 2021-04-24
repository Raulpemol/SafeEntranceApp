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
        private const string DAYS_BEFORE_PCR_ENV_URL = "https://registrolocales-api.azurewebsites.net/env/idbp";
        private const string DAYS_AFTER_POSSIBLE_INFECTION = "https://registrolocales-api.azurewebsites.net/env/dapi";
        private const string MINUTES_FOR_DANGEROUS_CONTACT = "https://registrolocales-api.azurewebsites.net/env/mfdc";

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

        public async Task<string> GetDaysAfterPossibleInfection()
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(DAYS_AFTER_POSSIBLE_INFECTION) as HttpWebRequest;
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                return await GetResponse(request);
            }
            catch (WebException ex)
            {
                return null;
            }
        }

        public async Task<string> GetMinutesForContact()
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(MINUTES_FOR_DANGEROUS_CONTACT) as HttpWebRequest;
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
