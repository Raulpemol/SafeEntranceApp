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
        public string CentralID { get; set; }
        public DateTime AlertDate { get; set; }
        public DateTime SymptomsDate { get; set; }
    }
}
