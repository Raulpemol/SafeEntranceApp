﻿using SafeEntranceApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SafeEntranceApp.Common
{
    class JsonParser
    {
        /*
         * Transforma un JSON en una lista de visitas
         */
        public static List<Visit> ParsePossibleContacts(string response)
        {
            var divResponse = response.Replace("]", "").Replace("{", "").Replace("\"", "").Replace("[", "").Split('}')
                .Where(s => !s.Equals(string.Empty))
                .Select(s => s.Split(','))
                .ToList();

            string[][] normalizedResponse = new string[divResponse.Count][];
            for (int i = 0; i < divResponse.Count; i++)
            {
                normalizedResponse[i] = new string[3];
                int k = 0;
                for(int j = 0; j < divResponse[i].Count(); j++)
                {
                    if (!divResponse[i][j].Equals(string.Empty))
                    {
                        normalizedResponse[i][k] = divResponse[i][j];
                        k += 1;
                    }
                }
            }

            var possibleContacts = normalizedResponse.Select(s => 
            {
                string placeID = s[0].Substring(8);
                DateTime enter = DateTime.Parse(s[1].Substring(14));
                DateTime exit = DateTime.Parse(s[2].Substring(13));
                return new Visit { PlaceID = placeID, EnterDateTime = enter, ExitDateTime = exit };
            }).ToList();

            return possibleContacts;
        }

        /*
         * Transforma una alerta junto con la lista de visitas asociadas a ella en un JSON
         */
        public static string AlertToJSON(CovidAlert alert, List<Visit> visits)
        {
            string result = "{" +
                        "\"code\": \"" + alert.Code + "\"," +
                        "\"alertDate\": \"" + alert.AlertDate + "\"," +
                        "\"symptomsDate\": \"" + alert.SymptomsDate + "\"," +
                        "\"state\": \""+ alert.State.ToString() + "\"," +
                        "\"visits\": " + "[";

            visits.ForEach(v =>
            {
                result += "{" +
                        "\"placeID\": \"" + v.PlaceID + "\"," +
                        "\"enterDateTime\": \"" + v.EnterDateTime + "\"," +
                        "\"exitDateTime\": \"" + v.ExitDateTime + "\"" +
                        "},";
            });

            if (visits.Count > 0)
                result = result.Substring(0, result.Length - 1);

            result += "]}";

            return result;
        }

        /*
         * Genera el JSON necesario para actualizar la lista de alertas
         */
        public static string GetAlertsBodyToJSON(List<string> places, List<string> alerts, DateTime lastSync)
        {
            string body = "{\"places\": [";
            places.ForEach(p => body += "\"" + p + "\",");
            if(places.Count > 0)
                body = body.Remove(body.Length - 1);
            body += "], \"fromDate\": \"" + lastSync.ToString("O") + "\", \"exclude\": [";
            alerts.ForEach(a => body += "\"" + a + "\",");
            if(alerts.Count > 0)
                body = body.Remove(body.Length - 1);
            body += "]}";

            return body;
        }

        /*
         * Genera un JSON conteniendo el identificador de un local y si se va a entrar o salir de él
         */
        public static string GetScanRequest(string id, bool isInside)
        {
            string body = "{\"id\": \"" + id + "\", \"isEntry\": " + (isInside ? "false" : "true") + "}";
            return body;
        }

        public static bool HasErrorMessage(string response)
        {
            return response.Contains("\"error\":");
        }
    }
}
