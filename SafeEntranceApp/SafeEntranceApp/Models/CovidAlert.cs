using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SafeEntranceApp.Models
{
    class CovidAlert
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public DateTime AlertDate { get; set; }
        public DateTime SymptomsDate { get; set; }
        public List<Visit> Visits { get; set; }

        public string ToJSON()
        {
            string result = "{" +
                        "'alertDate': " + AlertDate +
                        "'symptomsDate': " + SymptomsDate + 
                        "'visits': " + "[";

            Visits.ForEach(v => 
            {
                result += "{" +
                        "'placeID': " + v.PlaceID +
                        "'enterDateTime': " + v.EnterDateTime +
                        "'exitDateTime': " + v.ExitDateTime +
                        "},";
            });

            result = result.Substring(0, result.Length - 1);
            result += "]}";

            return result;
        }
    }
}
