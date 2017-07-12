using System;
using System.Data;
using System.IO;
using SQLite;

namespace Weather.Droid.DB
{
    [Table("city")]
    public class city
    {
        [PrimaryKey, AutoIncrement, Column("_Id")]
        public int Id { get; set; }
        [MaxLength(50)]//maximum length of the city 
        public string City { get; set; }
    }
}