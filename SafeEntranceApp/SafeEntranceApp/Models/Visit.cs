using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace SafeEntranceApp.Models
{
    public class Visit
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string PlaceID { get; set; }
        public DateTime EnterDateTime { get; set; }
        public DateTime ExitDateTime { get; set; }
    }
}
