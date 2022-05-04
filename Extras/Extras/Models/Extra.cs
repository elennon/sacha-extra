using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Extras.Models
{
    public partial class Extra
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<double> Hours { get; set; }
        public Nullable<double> Rate { get; set; }
        public Nullable<System.DateTime> Date { get; set; }

        public List<byte[]> Image { get; set; }
    }
}
