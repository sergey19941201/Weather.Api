using System;
using System.Data;
using System.IO;
using SQLite;

namespace Weather.Droid.DB
{
    [Table("cityOrCoordIndicator")]
    public  class cityOrCoordIndicator
    {
        public string CityOrCoord { get; set; }
    }
}