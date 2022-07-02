using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Extras.Models
{
    public class Batch
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string BatchId { get; set; }
        public string ProjectName { get; set; }
        public Nullable<System.DateTime> DateSent { get; set; }
    }
}
