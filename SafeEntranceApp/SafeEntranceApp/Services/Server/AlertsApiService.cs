using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SafeEntranceApp.Services.Server
{
    class AlertsApiService
    {
        private const string INSERT_ALERT_URL = "https://registrolocales-api.azurewebsites.net/api/alerts/addAlert";

        public async Task<string> InsertAlert(string alert)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(INSERT_ALERT_URL) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/json";

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(alert);
                }

                HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse;
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);

                if (response.StatusCode.Equals(HttpStatusCode.Created))
                {
                    return await reader.ReadToEndAsync();
                }
                else
                {
                    return null;
                }
            }
            catch (WebException ex)
            {
                return null;
            }
        }
    }
}
