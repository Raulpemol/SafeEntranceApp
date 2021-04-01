using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SafeEntranceApp.Models
{
    class Alert
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public DateTime AlertDate { get; set; }
        public DateTime SymptomsDate { get; set; }
        public List<Visit> Visits { get; set; }

        public Alert(DateTime symptomsDate, List<Visit> visits)
        {
            AlertDate = DateTime.Now;
            SymptomsDate = symptomsDate;
            Visits = visits;
        }

        public string ToJSON()
        {
            string result = "{" +
                        "'id': " + ID +
                        "'alertDate': " + AlertDate +
                        "'symptomsDate': " + SymptomsDate + 
                        "'visits': " + "[";

            Visits.ForEach(v => 
            {
                result += "{" +
                        "'id': " + v.ID +
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
