using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace C971_WGU_App.Models
{
    [Table("Assessment")]
    public class Assessment
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }

        [Column("Type"), NotNull]
        public string Type { get; set; }

        [Column("Name"), MaxLength(250)]
        public string Name { get; set; }

        [Column("StartDate")]
        public DateTime StartDate { get; set; }

        [Column("EndDate")]
        public DateTime EndDate { get; set; }

        [Column("Alert")]
        public bool Alert { get; set; }

        [Column("CourseID"), ForeignKey(typeof(Course))]
        public int CourseID { get; set; }
    }
}
