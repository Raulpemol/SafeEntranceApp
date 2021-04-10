using SafeEntranceApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SafeEntranceApp.Common
{
    class JsonParser
    {
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
    }
}
