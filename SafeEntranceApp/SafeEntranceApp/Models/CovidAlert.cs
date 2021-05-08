using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SafeEntranceApp.Models
{
    public enum AlertState { CREADA, VALIDADA }

    public class CovidAlert
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string CentralID { get; set; }
        public string Code { get; set; }
        public DateTime AlertDate { get; set; }
        public DateTime SymptomsDate { get; set; }
        public DateTime ValidationDate { get; set; }
        public AlertState State { get; set; }
    }
}
