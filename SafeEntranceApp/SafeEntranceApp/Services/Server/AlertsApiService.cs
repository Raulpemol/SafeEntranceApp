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
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                request.Method = "POST";
                request.ContentType = "application/json";
                Stream dataStream = request.GetRequestStream();
                StreamWriter writer = new StreamWriter(dataStream);
                writer.Write(alert);
                writer.Close();
                dataStream.Close();

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
            catch (WebException ex)
            {
                return null;
            }
        }
    }
}
