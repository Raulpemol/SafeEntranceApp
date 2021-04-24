using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SafeEntranceApp.Services.Server
{
    public class BaseApiService
    {
        protected async Task<string> GetResponse(HttpWebRequest request)
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
