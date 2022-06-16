using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Extras.Models
{
    public partial class Pics
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        [ForeignKey(typeof(Extra))]
        public string ExtraId { get; set; }
        public string FileName { get; set; }
        public byte[] Pic { get; set; }
    }
}
