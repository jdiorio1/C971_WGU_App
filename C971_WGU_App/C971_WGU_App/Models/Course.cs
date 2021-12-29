using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace C971_WGU_App.Models
{
    [Table("course")]
   public class Course
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }

        [Column("Name"), MaxLength(250), NotNull]
        public string Name { get; set; }

        [Column("StartDate")]
        public DateTime StartDate { get; set; }

        [Column("EndDate")]
        public DateTime EndDate { get; set; }

        [Column("Status")]
        public string Status { get; set; }

        [Column("Notes"), MaxLength(1000)]
        public string Notes { get; set; }

        [Column("Alert")]
        public bool Alert { get; set; }

        [Column("TermID"), ForeignKey(typeof(Term))]
        public int TermId { get; set; }

        [Column("InstructorID"), ForeignKey(typeof(Instructor))]
        public int InstructorID { get; set; }
    }
}
