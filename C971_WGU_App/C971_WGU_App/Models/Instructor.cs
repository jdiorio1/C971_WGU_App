using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using SQLite;

namespace C971_WGU_App.Models
{
    [Table("instructor")]
    public class Instructor
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }

        [Column("Name"), MaxLength(250), NotNull]
        public string Name { get; set; }

        [Column("Phone"), MaxLength(10), NotNull]
        public string Phone { get; set; }

        [Column("Email"), MaxLength(100), NotNull]
        public string Email { get; set; }
    }
}
