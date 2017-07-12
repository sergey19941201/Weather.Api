using System;
using System.Data;
using System.IO;
using SQLite;

namespace Weather.Droid.DB
{
    [Table("coordinates")]
    public class coordinates
    {
        [PrimaryKey, AutoIncrement, Column("_Id")]
        public int Id { get; set; }
        [MaxLength(50)]//maximum length of the X
        public string _X { get; set; }
        [MaxLength(50)]//maximum length of the Y 
        public string _Y { get; set; }
    }
}