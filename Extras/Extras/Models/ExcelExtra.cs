using System;
using SQLite;

namespace Extras.Models
{
    public partial class ExcelExtra
    {
        public string JobSite { get; set; }
        //public string SiteArea { get; set; }
        public string Date { get; set; }
        public string Description { get; set; }
        public double Rate { get; set; }

        public int Men { get; set; }
        
        public double Hours { get; set; }
        
        public double Value
        {
            get
            {
                return this.Hours * this.Rate * this.Men;
            }
        }

        public ExcelExtra(string JobSite, int Men, string Description, double Hours, double Rate, string Date) {
            this.JobSite = JobSite;
            this.Men = Men;
            this.Description = Description;
            this.Hours = Hours;
            this.Rate = Rate;
            this.Date = Date;
        }
    }
}
