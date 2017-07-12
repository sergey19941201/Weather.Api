using System;
using System.Data;
using System.IO;
using SQLite;

namespace Weather.Droid.DB
{
    [Table("CelsiusFarenh_Indicator")]
    public class CelsiusFarenh_Indicator
    {
        [MaxLength(50)]//maximum length of the city 
        public string c_f { get; set; }
    }
}