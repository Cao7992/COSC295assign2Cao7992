using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace COSC295assign2Cao7992.Models
{
    public class Game
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string GameName { get; set; }
        public string Description { get; set; }
        public double Rating { get; set; }
    }
}
