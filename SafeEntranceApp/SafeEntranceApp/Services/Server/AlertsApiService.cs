using SafeEntranceApp.Common;
using SafeEntranceApp.Models;
using SafeEntranceApp.Repositories;
using SafeEntranceApp.Services.Database;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SafeEntranceApp.Services.Server
{
    public class AlertsApiService
    {
        private const string INSERT_ALERT_URL = "https://registrolocales-api.azurewebsites.net/api/alerts/addAlert";
        private const string GET_AFFECTING_ALERTS = "https://registrolocales-api.azurewebsites.net/api/alerts/getAffectingAlerts";

        private CovidContactService covidContactService;

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
            catch (WebException)
            {
                return null;
            }
        }

        public async Task<List<CovidContact>> GetPossibleContacts(List<Visit> visits, int minutesForContact, DateTime lastSync, List<CovidAlert> ownAlerts)
        {
            try
            {
                var places = visits.Select(v => v.PlaceID).ToList();
                var ownAlertsIds = ownAlerts.Select(a => a.CentralID).ToList();

                HttpWebRequest request = WebRequest.Create(GET_AFFECTING_ALERTS) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/json";

                string body = JsonParser.GetAlertsBodyToJSON(places, ownAlertsIds, lastSync);

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(body);
                }

                HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse;
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);

                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    string responseText = await reader.ReadToEndAsync();
                    List<Visit> possibleContacts = JsonParser.ParsePossibleContacts(responseText);

                    List<CovidContact> contacts = await GetContacts(possibleContacts, visits, minutesForContact);

                    return contacts;
                }
                else
                {
                    return null;
                }
            }
            catch (WebException)
            {
                return null;
            }
        }

        private async Task<List<CovidContact>> GetContacts(List<Visit> possibleContacts, List<Visit> ownVisits, int minutesForContact)
        {
            List<CovidContact> result = new List<CovidContact>();
            covidContactService = new CovidContactService();

            List<CovidContact> previousContacts = await covidContactService.GetAll();

            possibleContacts.ForEach(pc =>
            {
                List<CovidContact> contacts = ownVisits.AsParallel().Where(ov => ov.PlaceID.Equals(pc.PlaceID)) //Pick visits to the same place as the alert
                    .Where(ov => !(ov.EnterDateTime > pc.ExitDateTime || ov.ExitDateTime < pc.EnterDateTime)) //Filter visits to obtain only the concurrent ones
                    .Where(ov => ov.EnterDateTime.AddMinutes(minutesForContact) <= pc.ExitDateTime || pc.EnterDateTime.AddMinutes(minutesForContact) <= ov.ExitDateTime) //Filter again to obtain concurrent visits lasting at least the minimum required to be considered dangerous contact
                    .Select(ov => new CovidContact 
                    { 
                        PlaceID = pc.PlaceID, 
                        ContactDate = pc.EnterDateTime < ov.EnterDateTime ? ov.EnterDateTime : pc.EnterDateTime 
                    }) //Create new contact object
                    .Where(ov =>
                    {
                        bool previous = false;
                        foreach (var contact in previousContacts)
                        {
                            if (contact.PlaceID.Equals(ov.PlaceID) && contact.ContactDate.Equals(ov.ContactDate))
                            {
                                previous = true;
                                break;
                            }
                        }

                        return !previous;
                    } //Check if the contact existed in the local DB. If true, it is skipped
                    ).ToList();
                result.AddRange(contacts);
            });

            return result;
        }
    }
}
