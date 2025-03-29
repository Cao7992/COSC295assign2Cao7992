using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace COSC295assign2Cao7992.Models
{
    public class Match
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int OpponentID { get; set; }
        public DateTime Date { get; set; }
        public string Comments { get; set; }
        public int GameID { get; set; }
        public bool Win { get; set; }
    }
}
