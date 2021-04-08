using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SafeEntranceApp.Models
{
    class CovidContact
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string PlaceID { get; set; }
        public string PlaceName { get; set; }
        public DateTime ContactDate { get; set; }
    }
}
