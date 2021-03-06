using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Extras.Models
{
    public partial class Extra
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string MyId { get; set; }
        public string BatchId { get; set; }
        public string ProjectId { get; set; }
        public string JobSite { get; set; }
        public string SiteArea { get; set; }
        public int Men { get; set; }
        public string Description { get; set; }
        public double Hours { get; set; }
        public double Rate { get; set; }
        public Nullable<System.DateTime> Date { get; set; }

        public bool WasSent { get; set; }

        public double LaborCost
        {
            get
            {
                return this.Hours * this.Rate * this.Men;
            }
        }
    }
}
